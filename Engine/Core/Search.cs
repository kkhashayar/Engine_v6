using System.Diagnostics;
using Engine.Core;

namespace Engine;
public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        MoveObject bestMove = null;
        List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
        DetectStalemateAndCheckmates(board, turn, bestMove, allPossibleMoves);

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
            if(bestMove is not null)
            {
                Console.WriteLine($"Best in Depth {currentDepth}: {Globals.MoveToString(bestMove)} Score: {bestMove.Score}");
            }
            
        }
        Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");

        return bestMove;
    }

    private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
    {
        if (depth == 0)
        {
            // return Quiescence(board, alpha, beta, turn);
            return Evaluators.EvaluatePosition(board, turn);
        }

        int bestScore = int.MinValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = ApplyMove(board, move);
            int score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, 1 - turn, move);
            MoveHandler.RestoreStateFromSnapshot();

            if (score > bestScore)
            {
                bestScore = score;
            }
            alpha = Math.Max(alpha, score);
            if (beta <= alpha)
            {
                break;
            }
        }
        return bestScore;
    }

    private static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
    {
        if (depth == 0)
        {
            // return Quiescence(board, alpha, beta, turn);
            return Evaluators.EvaluatePosition(board, turn);
        }

        int bestScore = int.MaxValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = ApplyMove(board, move);
            int score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, 1 - turn, move);
            MoveHandler.RestoreStateFromSnapshot();

            if (score < bestScore)
            {
                bestScore = score;
            }
            beta = Math.Min(beta, score);
            if (beta <= alpha)
            {
                break;
            }
        }
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
        if (standPat >= beta)
            return beta;
        if (standPat > alpha)
            alpha = standPat;

        List<MoveObject> captures = GetAllPossibleMoves(board, turn, true).Where(m => m.IsCapture).ToList();

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

    private static void DetectStalemateAndCheckmates(int[] board, int turn, MoveObject bestMove, List<MoveObject> allPossibleMoves)
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
