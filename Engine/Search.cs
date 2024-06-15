using System.Diagnostics;

namespace Engine;
public static class Search
{
    static GamePhase gamePhase = new(); 
    static OpeningManager openingManager = new OpeningManager();   
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        // Initialize stopwatch to enforce maxTime
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        int alpha = int.MinValue;
        int beta = int.MaxValue;

        MoveObject bestMove = default;
        List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

        // Check for no possible moves and handle stalemate/checkmate
        DetectStalemateAndCheckmates(board, turn, bestMove, allPossibleMoves);

        // Iterative deepening
        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i];

                // Clone the board and make the move
                int[] shadowBoard = ApplyMove(board, move);

                int score = (turn == 0) ?
                    AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move) :
                    AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move);

                // Restore the board state
                MoveHandler.RestoreStateFromSnapshot();

                

                //Update the best move based on the score
                if ((turn == 0 && score > alpha) || (turn == 1 && score < beta))
                {
                    if (turn == 0) alpha = score;
                    else beta = score;
                    bestMove = move;
                }

                //Check for forced mate move and  return early
                if (score >= 999999 || score <= -999999)
                {
                    Console.WriteLine($"Forced mate move{currentDepth}: {Globals.MoveToString(bestMove)}");
                    return bestMove;
                }

                if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

                // Check if the time limit is exceeded
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    return bestMove;
                }

            }
            Console.WriteLine($"Best in Depth {currentDepth}: {Globals.MoveToString(bestMove)}");
        }

        Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");


        // maybe I should add a logic here, to check if it is material loss?
        //if (turn == 0)
        //{

        //}

        //else
        //{

        //}

        return bestMove;
    }


    private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
    {
        if (depth == 0)
        {
            return Evaluators.EvaluatePosition(board, turn);
        }
        int bestScore = int.MinValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = ApplyMove(board, move);

            int score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
            MoveHandler.RestoreStateFromSnapshot();

            bestScore = Math.Max(bestScore, score);
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
            return Evaluators.EvaluatePosition(board, turn);
        }

        int bestScore = int.MaxValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = ApplyMove(board, move);

            int score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
            MoveHandler.RestoreStateFromSnapshot();

            bestScore = Math.Min(bestScore, score);
            beta = Math.Min(beta, score);
            if (beta <= alpha)
            {
                break;
            }
        }

        return bestScore;
    }

    private static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
        var orderedmoves = moves.OrderByDescending(m => m.Priority)
                        .ThenByDescending(m => m.IsCapture)
                        .ThenByDescending(m => m.IsCheck)
                        .ThenByDescending(m => m.IsPromotion)
                        .ThenByDescending(m => m.IsEnPassant)
                        .ToList();
        return orderedmoves;
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

    private static int[] ApplyMove(int[] board, MoveObject move)
    {
        int[] shadowBoard = (int[])board.Clone();
        MoveHandler.RegisterStaticStates();
        MoveHandler.MakeMove(shadowBoard, move);
        return shadowBoard;
    }
}
