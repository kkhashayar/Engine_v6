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

                int count = 0;
                foreach (var move in moves)
                {   
                    count++;
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.RegisterStaticStates();
                    MoveHandler.MakeMove(shadowBoard, move);

                    // Evaluate the board after making the move
                    decimal baseScore = Evaluators.GetByMaterial(shadowBoard, turn);
                    decimal score = baseScore -Negamax(shadowBoard, turn ^ 1, currentDepth - 1, -beta, -alpha);

                    MoveHandler.RestoreStateFromSnapshot();
                    if(count<=12)
                    Console.WriteLine($"Move: {Globals.MoveToString(move)}, Score: {score} Depth: {currentDepth}");

                    else if(count == 13)
                    {
                        count = 0;
                        Console.Clear();
                    }   

                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                    }

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
            decimal maxScore = MIN_SCORE;

            foreach (var move in moves)
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score = -Negamax(shadowBoard, turn ^ 1, depth - 1, -beta, -alpha);
                maxScore = Math.Max(maxScore, score);

                MoveHandler.RestoreStateFromSnapshot();

                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            }
            return maxScore;
        }
    }
}
