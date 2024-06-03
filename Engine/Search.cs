using System;
using System.Diagnostics;

namespace Engine
{
    public static class Search
    {
        public static MoveObject GetBestMove(int[] board, int turn, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int maxDepth = 1;  // Start with a depth of 1 and increase iteratively
            MoveObject bestMove = default;
            int bestScore = (turn == 0) ? int.MinValue : int.MaxValue;

            while (true)
            {
                MoveObject currentBestMove = default;
                int currentBestScore = (turn == 0) ? int.MinValue : int.MaxValue;

                var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
                Console.WriteLine($"Depth {maxDepth}: Evaluating {allPossibleMoves.Count} moves.");

                foreach (var move in allPossibleMoves)
                {
                    MoveHandler.RegisterStaticStates();
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.MakeMove(shadowBoard, move);
                    MoveHandler.RestoreStateFromSnapshot(); 

                    int score = AlphaBeta(shadowBoard, maxDepth, int.MinValue, int.MaxValue, 1 - turn);

                    if (turn == 0 && score > currentBestScore) // Maximizing for white
                    {
                        currentBestScore = score;
                        currentBestMove = move;
                    }
                    else if (turn == 1 && score < currentBestScore) // Minimizing for black
                    {
                        currentBestScore = score;
                        currentBestMove = move;
                    }

                    Console.WriteLine($"Move: {Globals.MoveToString(move)}, Score: {score}");
                }

                if ((turn == 0 && currentBestScore > bestScore) || (turn == 1 && currentBestScore < bestScore))
                {
                    bestScore = currentBestScore;
                    bestMove = currentBestMove;
                }

                Console.WriteLine($"Best Move at depth {maxDepth}: {Globals.MoveToString(bestMove)}, Score: {bestScore}");

                if (stopwatch.Elapsed >= maxTime || maxDepth == 1 && allPossibleMoves.Count == 1)
                {
                    Console.WriteLine("Time limit reached or only one move available at depth 1. Ending search.");
                    break;
                }

                maxDepth++;  // Increase the depth for the next iteration
            }

            return bestMove;
        }

        private static int AlphaBeta(int[] board, int depth, int alpha, int beta, int turn)
        {
            if (depth == 0)
            {
                return Evaluators.GetByMaterial(board, turn);
            }

            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (turn == 0)  // Maximizing player
            {
                int maxEval = int.MinValue;
                foreach (var move in allPossibleMoves)
                {
                    MoveHandler.RegisterStaticStates();
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.MakeMove(shadowBoard, move);
                    MoveHandler.RestoreStateFromSnapshot(); 


                    int eval = AlphaBeta(shadowBoard, depth - 1, alpha, beta, 1 - turn);
                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break;  // Alpha cut-off
                    }
                }
                return maxEval;
            }
            else  // Minimizing player
            {
                int minEval = int.MaxValue;
                foreach (var move in allPossibleMoves)
                {
                    MoveHandler.RegisterStaticStates();
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.MakeMove(shadowBoard, move);
                    MoveHandler.RestoreStateFromSnapshot();

                    int eval = AlphaBeta(shadowBoard, depth - 1, alpha, beta, 1 - turn);
                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break;  // Beta cut-off
                    }
                }
                return minEval;
            }
        }
    }
}
