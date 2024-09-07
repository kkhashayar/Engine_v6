using System.Diagnostics;
using Engine.Core;

namespace Engine;
public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        MoveObject bestMove = default;
        List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
        DetectStalemateAndCheckmates(board, turn, allPossibleMoves);

        int alpha = int.MinValue;
        int beta = int.MaxValue;

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            int tempAlpha = alpha;
            int tempBeta = beta;

            foreach (var move in allPossibleMoves)
            {
                int[] shadowBoard = ApplyMove(board, move);

                int score = (turn == 0) ?
                      AlphaBetaMin(shadowBoard, currentDepth - 1, tempAlpha, tempBeta, 1)
                    : AlphaBetaMax(shadowBoard, currentDepth - 1, tempAlpha, tempBeta, 0);

                MoveHandler.RestoreStateFromSnapshot();

                if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

                if (turn == 0 && score > tempAlpha)
                {
                    tempAlpha = score;
                    bestMove = move;
                    bestMove.Score = score;
                }
                if (turn == 1 && score < tempBeta)
                {
                    tempBeta = score;
                    bestMove = move;
                    bestMove.Score = score;
                }

                if (stopwatch.Elapsed >= maxTime)
                {
                    bestMove.Score = score;
                    return bestMove;
                }

                if (score >= 99999 || score <= -99999)
                {
                    bestMove = move;
                    bestMove.Score = score;
                    return bestMove;
                }
            }

            if (tempAlpha != alpha || tempBeta != beta)
            {
                alpha = tempAlpha;
                beta = tempBeta;

                Console.WriteLine($"Depth {currentDepth} completed. Best move: {Globals.MoveToString(bestMove)} with score {bestMove.Score}");
            }
        }

        return bestMove;
    }

    private static int AlphaBetaMax(int[] board, int depth, int alpha, int beta, int turn)
    {
        if (depth == 0)
        {
            //return Quiescence(board, alpha, beta, turn);
            return Evaluators.GetByMaterial(board);
        }

        var possibleMoves = GetAllPossibleMoves(board, turn, true);
        foreach (var move in possibleMoves)
        {
            int[] shadowBoard = ApplyMove(board, move);
            int score = AlphaBetaMin(shadowBoard, depth - 1, alpha, beta, 1 - turn);
            MoveHandler.RestoreStateFromSnapshot();

            if (score > alpha)
            {
                alpha = score;
                if (alpha >= beta) break;
            }
        }
        return alpha;
    }

    private static int AlphaBetaMin(int[] board, int depth, int alpha, int beta, int turn)
    {
        if (depth == 0)
        {
            // return Quiescence(board, alpha, beta, turn);
            return Evaluators.GetByMaterial(board);
        }

        var possibleMoves = GetAllPossibleMoves(board, turn, true);
        foreach (var move in possibleMoves)
        {
            int[] shadowBoard = ApplyMove(board, move);
            int score = AlphaBetaMax(shadowBoard, depth - 1, alpha, beta, 1 - turn);
            MoveHandler.RestoreStateFromSnapshot();

            if (score < beta)
            {
                beta = score;
                if (beta <= alpha) break;
            }
        }
        return beta;
    }
    private static int Quiescence(int[] board, int alpha, int beta, int turn)
    {
        int maxDepth = 3;
        return QuiescenceInternal(board, alpha, beta, turn, maxDepth);
    }
    private static int QuiescenceInternal(int[] board, int alpha, int beta, int turn, int depth)
    {
        if (depth == 0)
        {
            return Evaluators.GetByMaterial(board);
        }

        int standPat = Evaluators.GetByMaterial(board);
        if (standPat >= beta)
            return beta;
        if (standPat > alpha)
            alpha = standPat;

        List<MoveObject> priorityMoves = GetAllPossibleMoves(board, turn, true)
               .Where(move => (move.IsCapture && move.IsCheck) || move.IsCapture || move.IsCheck)
               .OrderByDescending(move => (move.IsCapture && move.IsCheck))
               .ToList();

        foreach (var move in priorityMoves)
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
        // Console.WriteLine($"Quiescence: {Globals.MoveToString(move)} Score: {score} Depth: {depth}");
        return alpha;
    }

    private static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        // Have to implement better move_priority in move generator.
        var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
        // return moves.OrderByDescending(m => m.Priority).ToList();

        return moves;
    }

    private static int[] ApplyMove(int[] board, MoveObject move)
    {
        int[] shadowBoard = (int[])board.Clone();
        MoveHandler.RegisterStaticStates();
        MoveHandler.MakeMove(shadowBoard, move);
        return shadowBoard;
    }

    private static void DetectStalemateAndCheckmates(int[] board, int turn, List<MoveObject> allPossibleMoves)
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

