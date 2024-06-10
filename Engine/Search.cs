using System.Diagnostics;

namespace Engine;
public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        var bestMoveHistory = new List<MoveObject>();

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
            // Create a dictionary to store moves with their favorability score based on historical data
            var favorabilityScores = new Dictionary<int, bool>();
            foreach (var history in bestMoveHistory)
            {
                if (history.Depth == currentDepth - 1 &&
                    ((turn == 0 && history.Score > alpha) || (turn == 1 && history.Score < beta)))
                {
                    favorabilityScores[history.Score] = true;
                }
            }

            // Reordering moves based on historical data
            allPossibleMoves = allPossibleMoves.OrderByDescending(m => favorabilityScores.ContainsKey(m.Score) && favorabilityScores[m.Score]).ToList();

            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i];

                // Check if the time limit is exceeded
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    return bestMove;
                }

                // Clone the board and make the move
                int[] shadowBoard = ApplyMove(board, move);

                // Evaluate the move using alpha-beta pruning
                int score = (turn == 0) ?
                    AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move) :
                    AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move);

                // Restore the board state
                MoveHandler.RestoreStateFromSnapshot();

                // Update the best move based on the score
                if ((turn == 0 && score > alpha) || (turn == 1 && score < beta))
                {
                    if (turn == 0) alpha = score;
                    else beta = score;

                    bestMove = move;
                    bestMove.Score = score;
                    bestMove.Depth = currentDepth;
                    bestMoveHistory.Add(bestMove);
                }

                // Check for decisive scores and return early
                if (score >= 99999 || score <= -99999)
                {
                    Console.WriteLine($"Decisive best move in {currentDepth}: {Globals.MoveToString(bestMove)}");
                    return bestMove;
                }

                if (allPossibleMoves.Count == 1) return allPossibleMoves[0];
            }
            Console.WriteLine($"Best Move at end of Depth {currentDepth}: {Globals.MoveToString(bestMove)}");
        }

        Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");


        // maybe I should add a logic here, to check if it is material loss?
        if(turn == 0)
        {

        }

        else
        {

        }


        return bestMove;
    }


    private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval)
    {
        //if(depth == 5)
        //{
        //    return Quiescence(board, alpha, beta, turn);
        //}
        if (depth == 0)
        {
            return Evaluators.GetByMaterial(board, turn);
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
        //if (depth == 5)
        //{
        //    return Quiescence(board, alpha, beta, turn);
        //}
        if (depth == 0)
        {
            return Evaluators.GetByMaterial(board, turn);

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

    private static int Quiescence(int[] board, int alpha, int beta, int turn)
    {
        int standPat = Evaluators.GetByMaterial(board, turn);
        if (standPat >= beta)
            return beta;
        if (standPat > alpha)
            alpha = standPat;

        List<MoveObject> captures = GetAllPossibleMoves(board, turn, true).Where(m => m.Priority >= 3).ToList();

        foreach (var move in captures)
        {
            int[] shadowBoard = ApplyMove(board, move);

            int score = -Quiescence(shadowBoard, -beta, -alpha, turn ^ 1);
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
