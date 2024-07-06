using System.Collections.Concurrent;
using System.Diagnostics;
using Engine;
using Engine.Core;

namespace Engine
{
    public static class Search
    {
        private static readonly TranspositionTable TranspositionTable = new TranspositionTable();

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            MoveObject bestMove = null;
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
            DetectStalemateAndCheckmates(board, turn, ref bestMove, allPossibleMoves);

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                foreach (var move in allPossibleMoves)
                {
                    int[] shadowBoard = ApplyMove(board, move);

                    int score = (turn == 0) ?
                        AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move) :
                        AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move);

                    MoveHandler.RestoreStateFromSnapshot();

                    if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

                    if ((turn == 0 && score > alpha) || (turn == 1 && score < beta))
                    {
                        if (turn == 0) alpha = score;
                        else beta = score;
                        bestMove = move;
                        bestMove.Score = score;
                    }

                    if (stopwatch.Elapsed >= maxTime)
                    {
                        Console.WriteLine("Stopping search due to time limit.");
                        return bestMove;
                    }
                }
                if (bestMove != null)
                {
                    Console.WriteLine($"Best in Depth {currentDepth}: {Globals.MoveToString(bestMove)} Score: {bestMove.Score}");
                }
            }
            Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");

            return bestMove;
        }

        private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
        {
            ulong key = Zobrist.ComputeZobristHash(board, turn);

            if (TranspositionTable.TryRetrieve(key, out TranspositionEntry entry))
            {
                if (entry.Depth >= depth)
                {
                    if (entry.Alpha >= beta) return entry.Alpha;
                    if (entry.Beta <= alpha) return entry.Beta;
                    alpha = Math.Max(alpha, entry.Alpha);
                    beta = Math.Min(beta, entry.Beta);
                }
            }

            if (depth == 0)
            {
                return Quiescence(board, alpha, beta, turn);
                // return Evaluators.EvaluatePosition(board, turn);
            }

            int bestScore = int.MinValue;
            MoveObject bestMove = null;
            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, 1 - turn, move);
                MoveHandler.RestoreStateFromSnapshot();

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                alpha = Math.Max(alpha, score);
                if (beta <= alpha) break;
            }

            TranspositionTable.Store(key, new TranspositionEntry { Depth = depth, Score = bestScore, Alpha = alpha, Beta = beta, BestMove = bestMove });
            return bestScore;
        }

        private static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
        {
            ulong key = Zobrist.ComputeZobristHash(board, turn);

            if (TranspositionTable.TryRetrieve(key, out TranspositionEntry entry))
            {
                if (entry.Depth >= depth)
                {
                    if (entry.Alpha >= beta) return entry.Alpha;
                    if (entry.Beta <= alpha) return entry.Beta;
                    alpha = Math.Max(alpha, entry.Alpha);
                    beta = Math.Min(beta, entry.Beta);
                }
            }

            if (depth == 0)
            {
                return Quiescence(board, alpha, beta, turn);
                // return Evaluators.EvaluatePosition(board, turn);
            }

            int bestScore = int.MaxValue;
            MoveObject bestMove = null;
            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, 1 - turn, move);
                MoveHandler.RestoreStateFromSnapshot();

                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                beta = Math.Min(beta, score);
                if (beta <= alpha) break;
            }

            TranspositionTable.Store(key, new TranspositionEntry { Depth = depth, Score = bestScore, Alpha = alpha, Beta = beta, BestMove = bestMove });
            return bestScore;
        }

        private static int Quiescence(int[] board, int alpha, int beta, int turn)
        {
            int maxDepth = 2;
            return QuiescenceInternal(board, alpha, beta, turn, maxDepth);
        }

        private static int QuiescenceInternal(int[] board, int alpha, int beta, int turn, int depth)
        {
            if (depth == 0)
            {
                return Evaluators.EvaluatePosition(board, turn);
            }

            int standPat = Evaluators.EvaluatePosition(board, turn);
            if (standPat >= beta) return beta;
            if (standPat > alpha) alpha = standPat;

            List<MoveObject> captures = GetAllPossibleMoves(board, turn, true)
                .Where(m => m.Priority >= 2).ToList();

            foreach (var move in captures)
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = -QuiescenceInternal(shadowBoard, -beta, -alpha, turn ^ 1, depth - 1);
                MoveHandler.RestoreStateFromSnapshot();

                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }
            }

            return alpha;
        }

        private static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
        {
            var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
            var orderedmoves = moves.OrderByDescending(m => m.Priority >= 2)
                            .ThenByDescending(m => m.ShortCastle || m.LongCastle).ToList();
            return orderedmoves;
        }

        private static int[] ApplyMove(int[] board, MoveObject move)
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);
            return shadowBoard;
        }

        private static void DetectStalemateAndCheckmates(int[] board, int turn, ref MoveObject bestMove, List<MoveObject> allPossibleMoves)
        {
            if (!allPossibleMoves.Any())
            {
                if (turn == 0)
                {
                    var blackMoves = GetAllPossibleMoves(board, 1, true);
                    if (!blackMoves.Any()) Globals.Stalemate = true;
                    Globals.CheckmateWhite = true;
                }
                else if (turn == 1)
                {
                    var whiteMoves = GetAllPossibleMoves(board, 0, true);
                    if (!whiteMoves.Any()) Globals.Stalemate = true;
                    Globals.CheckmateBlack = true;
                }
            }
        }
    }

    public class TranspositionTable
    {
        private readonly ConcurrentDictionary<ulong, TranspositionEntry> _table = new();

        public void Store(ulong key, TranspositionEntry entry)
        {
            _table[key] = entry;
        }

        public bool TryRetrieve(ulong key, out TranspositionEntry entry)
        {
            return _table.TryGetValue(key, out entry);
        }
    }

    public class TranspositionEntry
    {
        public int Depth { get; set; }
        public int Score { get; set; }
        public int Alpha { get; set; }
        public int Beta { get; set; }
        public MoveObject BestMove { get; set; }
    }
}


public static class Zobrist
{
    public static ulong[,] ZobristKeys = new ulong[64, 13]; // [square, piece type]
    public static ulong ZobristBlackToMove;

    static Zobrist()
    {
        Random random = new Random(0);
        for (int square = 0; square < 64; square++)
        {
            for (int piece = 0; piece < 13; piece++) // 13 to account for all piece types including black pieces
            {
                ZobristKeys[square, piece] = GenerateRandom64BitNumber(random);
            }
        }
        ZobristBlackToMove = GenerateRandom64BitNumber(random);
    }

    private static ulong GenerateRandom64BitNumber(Random random)
    {
        byte[] buffer = new byte[8];
        random.NextBytes(buffer);
        return BitConverter.ToUInt64(buffer, 0);
    }

    // Compute Zobrist hash for a given board position
    public static ulong ComputeZobristHash(int[] board, int turn)
    {
        ulong hash = 0;
        for (int square = 0; square < 64; square++)
        {
            int piece = board[square];
            if (piece != Piece.None)
            {
                int pieceType = GetPieceTypeIndex(piece);
                if (pieceType != -1)
                {
                    hash ^= ZobristKeys[square, pieceType];
                }
            }
        }

        if (turn == 1)
        {
            hash ^= ZobristBlackToMove;
        }

        return hash;
    }

    // Corrected GetPieceTypeIndex method to return indices 0 to 12
    private static int GetPieceTypeIndex(int piece)
    {
        if (piece == MoveGenerator.whitePawn) return 0;
        if (piece == MoveGenerator.whiteKnight) return 1;
        if (piece == MoveGenerator.whiteBishop) return 2;
        if (piece == MoveGenerator.whiteRook) return 3;
        if (piece == MoveGenerator.whiteQueen) return 4;
        if (piece == MoveGenerator.whiteKing) return 5;
        if (piece == MoveGenerator.blackPawn) return 6;
        if (piece == MoveGenerator.blackKnight) return 7;
        if (piece == MoveGenerator.blackBishop) return 8;
        if (piece == MoveGenerator.blackRook) return 9;
        if (piece == MoveGenerator.blackQueen) return 10;
        if (piece == MoveGenerator.blackKing) return 11;
        return -1; // None or invalid piece value
    }
}
