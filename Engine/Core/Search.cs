using Engine.Core;
using System.Diagnostics;
namespace Engine;

public static class Search
{
    private static ulong zobristHash;
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxtime)
    {
        
        MoveObject bestMove = default;
        List<MoveObject> principalVariation = new();

        // Determine game phase and set appropriate allocated time
        int whitePieces = board.Where(p => Piece.IsWhite(p)).Count();
        int blackPieces = board.Where(p => Piece.IsBlack(p)).Count();
        int totalPiecesOnTheBoard = whitePieces + blackPieces;

        TimeSpan allocatedTime;

        if (totalPiecesOnTheBoard == 32)
        {
            // Opening phase
            allocatedTime = TimeSpan.FromSeconds(8);
        }
        else if (totalPiecesOnTheBoard < 32 && totalPiecesOnTheBoard > 10)
        {
            // Middle game phase
            allocatedTime = TimeSpan.FromSeconds(60);
        }
        else
        {
            // Endgame phase
            allocatedTime = TimeSpan.FromSeconds(5);
        }

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // Adjust search depth if it's not the initial turn
        int adjustedMaxDepth = Globals.InitialTurn == turn ? maxDepth : maxDepth + 3;

        // Start Zobrist Hash
        zobristHash = Zobrist.CalculateHash(board, turn);

        var allPossibleMoves = MoveGenerator
            .GenerateAllMoves(board, turn, true)
            .OrderByDescending(m => m.IsCapture)
            .ThenByDescending(m => m.IsCheck).ToList();

        if (allPossibleMoves.Count == 1) return allPossibleMoves[0]; // Only one legal move
        if (allPossibleMoves.Count == 0)
        {
            // Handle checkmate or stalemate
            if (turn == 0) Globals.CheckmateWhite = true;
            else if (turn == 1) Globals.CheckmateBlack = true;
            else Globals.Stalemate = true;
            return bestMove;
        }

        // Iterative deepening with timing control
        for (int currentDepth = 2; currentDepth <= adjustedMaxDepth; currentDepth += 2)
        {
            if (stopwatch.Elapsed >= allocatedTime)
            {
                break; // Stop if we've run out of time
            }
            int alpha = -999999;
            int beta = 999999;
            MoveObject currentBestMove = default;
            List<MoveObject> currentPV = new();

            foreach (var move in allPossibleMoves)
            {
                if (stopwatch.Elapsed >= allocatedTime)
                {
                    return bestMove;
                }

                // Update Zobrist hash for the move
                zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                List<MoveObject> line = new();
                int score = (turn == 0)
                    ? AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, ref line)
                    : AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, ref line);

                MoveHandler.RestoreStateFromSnapshot();

                // Revert Zobrist hash after undoing move
                zobristHash = Zobrist.UpdateHash(zobristHash, shadowBoard, move, turn);

                if (score >= 1000000 || score <= -1000000)
                {
                    currentBestMove = move;
                    currentPV = new List<MoveObject> { move };
                    currentPV.AddRange(line);
                    return currentBestMove;
                }

                if (turn == 0 && score > alpha) // White maximizes
                {
                    alpha = score;
                    currentBestMove = move;
                    currentPV = new List<MoveObject> { move };
                    currentPV.AddRange(line);
                }
                else if (turn == 1 && score < beta) // Black minimizes
                {
                    beta = score;
                    currentBestMove = move;
                    currentPV = new List<MoveObject> { move };
                    currentPV.AddRange(line);
                }
            }

            if (currentBestMove != default)
            {
                bestMove = currentBestMove; // Update the best move
                principalVariation = currentPV; // Update the PV
            }

            allocatedTime -= stopwatch.Elapsed;

            Console.WriteLine($"Depth {currentDepth / 2} score {(turn == 0 ? alpha : beta)}");
            Console.WriteLine("PV: " + string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m))));
        }

        return bestMove;
    }



    public static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {
        if (depth == 0)
        {
            
            pvLine.Clear();
            //return Evaluators.GetByMaterial(board, turn);
            return Quiescence(board, alpha, beta, turn, ref pvLine, 4);
        }
        var allPossibleMoves = MoveGenerator
            .GenerateAllMoves(board, turn, true)
            .OrderByDescending(m => m.IsCapture)
            .ThenByDescending(m => m.IsCheck).ToList();

        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return -999999;
        }

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            // Update Zobrist hash for the move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            MoveHandler.MakeMove(board, move);

            // Clone the board before passing it to the recursive call
            int[] boardCopy = (int[])board.Clone();

            List<MoveObject> line = new List<MoveObject>();
            int score = AlphaBetaMin(depth - 1, alpha, beta, boardCopy, turn ^ 1, ref line);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            // Revert Zobrist hash after undoing move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            if (score >= beta)
            {
                pvLine = new List<MoveObject> { move };
                pvLine.AddRange(line);
                return beta;
            }
            if (score > alpha)
            {
                alpha = score;
                bestLine = new List<MoveObject> { move };
                bestLine.AddRange(line);
            }
        }

        pvLine = bestLine;
        return alpha;
    }

    public static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {
        if (depth == 0)
        {
            pvLine.Clear();
            //return Evaluators.GetByMaterial(board, turn);
            return Quiescence(board, alpha, beta, turn, ref pvLine, 4); 
        }
        var allPossibleMoves = MoveGenerator
            .GenerateAllMoves(board, turn, true)
            .OrderByDescending(m => m.IsCapture)
            .ThenByDescending(m => m.IsCheck).ToList();

        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return 999999;
        }

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            // Update Zobrist hash for the move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            MoveHandler.MakeMove(board, move);

            // Clone the board before passing it to the recursive call
            int[] boardCopy = (int[])board.Clone();

            List<MoveObject> line = new List<MoveObject>();
            int score = AlphaBetaMax(depth - 1, alpha, beta, boardCopy, turn ^ 1, ref line);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            // Revert Zobrist hash after undoing move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            if (score <= alpha)
            {
                pvLine = new List<MoveObject> { move };
                pvLine.AddRange(line);
                return alpha;
            }
            if (score < beta)
            {
                beta = score;
                bestLine = new List<MoveObject> { move };
                bestLine.AddRange(line);
            }
        }

        pvLine = bestLine;
        return beta;
    }

    public static int Quiescence(int[] board, int alpha, int beta, int turn, ref List<MoveObject> pvLine, int depth)
    {
        int standPat = Evaluators.GetByMaterial(board, turn);

        if (depth == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        if (turn == 0)
        {
            if (standPat >= beta)
            {
                return beta;
            }
            if (standPat > alpha)
            {
                alpha = standPat;
            }
        }
        else
        {
            if (standPat <= alpha)
            {
                return alpha;
            }
            if (standPat < beta)
            {
                beta = standPat;
            }
        }

        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true)
            .Where(mo => mo.IsCapture || mo.IsCheck).ToList();

        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        List<MoveObject> bestLine = new List<MoveObject>();
        bool isPVNode = false;

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            // Update Zobrist hash for the move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;
            MoveHandler.MakeMove(board, move);

            List<MoveObject> line = new List<MoveObject>();
            int score = Quiescence(board, alpha, beta, turn ^ 1, ref line, depth - 1);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            // Revert Zobrist hash after undoing move
            zobristHash = Zobrist.UpdateHash(zobristHash, board, move, turn);

            if (turn == 0)
            {
                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                    isPVNode = true;
                    bestLine = new List<MoveObject> { move };
                    bestLine.AddRange(line);
                }
            }
            else
            {
                if (score <= alpha)
                {
                    return alpha;
                }
                if (score < beta)
                {
                    beta = score;
                    isPVNode = true;
                    bestLine = new List<MoveObject> { move };
                    bestLine.AddRange(line);
                }
            }
        }

        if (isPVNode)
        {
            pvLine = bestLine;
        }
        else
        {
            pvLine.Clear();
        }

        return (turn == 0) ? alpha : beta;
    }
}


public static class Zobrist
{
    private static ulong[,] pieceSquareHashes = new ulong[12, 64]; // 12 piece types, 64 squares
    private static ulong whiteToMoveHash;

    // Initialize random hash values for all pieces on all squares and for the side to move
    static Zobrist()
    {
        Random rng = new Random();

        for (int piece = 0; piece < 12; piece++)
        {
            for (int square = 0; square < 64; square++)
            {
                pieceSquareHashes[piece, square] = GenerateRandomUlong(rng);
            }
        }

        whiteToMoveHash = GenerateRandomUlong(rng);
    }

    // Generate a random 64-bit number
    private static ulong GenerateRandomUlong(Random rng)
    {
        byte[] buffer = new byte[8];
        rng.NextBytes(buffer);
        return BitConverter.ToUInt64(buffer, 0);
    }

    // Calculate the Zobrist hash for the initial board state
    public static ulong CalculateHash(int[] board, int turn)
    {
        ulong hash = 0;

        for (int square = 0; square < 64; square++)
        {
            int piece = board[square];
            int pieceIndex = PieceToIndex(piece);

            if (pieceIndex == -1) continue; // Skip empty squares

            hash ^= pieceSquareHashes[pieceIndex, square];
        }

        if (turn == 0) // White to move
        {
            hash ^= whiteToMoveHash;
        }

        return hash;
    }

    // Update the Zobrist hash after a move has been made
    public static ulong UpdateHash(ulong currentHash, int[] board, MoveObject move, int turn)
    {
        int startSquare = move.StartSquare;
        int endSquare = move.EndSquare;

        // Remove piece from start square
        int movingPiece = board[startSquare];
        int movingPieceIndex = PieceToIndex(movingPiece);

        if (movingPieceIndex != -1) // Only hash if it's not an empty square
        {
            currentHash ^= pieceSquareHashes[movingPieceIndex, startSquare];
        }

        // Remove any captured piece from end square
        int capturedPiece = board[endSquare];
        int capturedPieceIndex = PieceToIndex(capturedPiece);

        if (capturedPieceIndex != -1) // Only hash if it's not an empty square
        {
            currentHash ^= pieceSquareHashes[capturedPieceIndex, endSquare];
        }

        // Add piece to end square
        if (movingPieceIndex != -1) // Only hash if it's not an empty square
        {
            currentHash ^= pieceSquareHashes[movingPieceIndex, endSquare];
        }

        // Flip hash for side to move
        currentHash ^= whiteToMoveHash;

        return currentHash;
    }

    // Convert the piece value to an index for hashing purposes
    private static int PieceToIndex(int piece)
    {
        switch (piece)
        {
            case 0:
                return -1; // Indicating this is an empty square, skip hashing
            case 1: return 0;  // White Pawn
            case 3: return 1;  // White Knight
            case 4: return 2;  // White Bishop
            case 5: return 3;  // White Rook
            case 9: return 4;  // White Queen
            case 99: return 5; // White King
            case 11: return 6; // Black Pawn
            case 13: return 7;  // Black Knight
            case 14: return 8;  // Black Bishop
            case 15: return 9;  // Black Rook
            case 19: return 10; // Black Queen
            case 109: return 11; // Black King
            default:
                throw new ArgumentException($"Invalid piece code provided: {piece}");
        }
    }
}
