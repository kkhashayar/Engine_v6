using System.Diagnostics;

namespace Engine
{
    public static class Search
    {
        private const decimal MAX_SCORE = 100000m;
        private const decimal MIN_SCORE = -100000m;

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            decimal alpha = MIN_SCORE;
            decimal beta = MAX_SCORE;
            MoveObject bestMove = new();

            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                var moves = MoveGenerator.GenerateAllMoves(board, turn, true);

                if (moves.Count == 0) 
                {
                    Globals.CheckmateBlack = true;
                    Globals.CheckmateWhite = true;
                }
                      

                    
                if(moves.Count == 1) return moves[0];

                Console.WriteLine($"Depth: {currentDepth}");


                foreach (var move in moves)
                {
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.RegisterStaticStates();
                    MoveHandler.MakeMove(shadowBoard, move);

                    // Evaluate the board after making the move
                    decimal baseScore = Evaluators.GetByMaterial(shadowBoard, turn);
                    decimal score;

                    // Switching the turn using an if-else structure
                    if (turn == 0)
                    {
                        score = baseScore - Negamax(shadowBoard, 1, currentDepth - 1, -beta, -alpha);
                    }
                    else
                    {
                        score = baseScore - Negamax(shadowBoard, 0, currentDepth - 1, -beta, -alpha);
                    }

                    MoveHandler.RestoreStateFromSnapshot();
                   
                    Console.WriteLine($"Move: {Globals.ConvertMoveToString(move)}, Score: {score} Depth: {currentDepth}");

                    // Updating alpha if a better score is found
                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                    }

                    // Termination condition based on time or alpha-beta cutoff
                    if (stopwatch.Elapsed >= maxTime || alpha >= beta)
                    {
                        return bestMove;
                    }
                }

            }
            return bestMove;
        }

        private static decimal Negamax(int[] board, int turn, int depth, decimal alpha, decimal beta)
        {
            if (depth == 0)
                return Evaluators.GetByMaterial(board, turn);

            var moves = MoveGenerator.GenerateAllMoves(board, turn, true);
            decimal maxScore = decimal.MinValue; // Ensure you define MIN_SCORE as the smallest possible value for decimal

            foreach (var move in moves)
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score;
                // Manually switch turns using if-else
                if (turn == 0)
                { // Assuming 0 is white and 1 is black
                    score = -Negamax(shadowBoard, 1, depth - 1, -beta, -alpha);
                }
                else
                {
                    score = -Negamax(shadowBoard, 0, depth - 1, -beta, -alpha);
                }

                maxScore = Math.Max(maxScore, score);

                MoveHandler.RestoreStateFromSnapshot();

                // Alpha-beta pruning
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            }
            return maxScore;
        }

    }
}
