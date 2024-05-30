using System.Diagnostics;

namespace Engine;

public static class Search
{
    public static List<MoveObject>IgnoringMoves = new List<MoveObject>();
    public static MoveObject MinMax(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        decimal alpha = decimal.MinValue;
        decimal beta = decimal.MaxValue;
        decimal score = 0;
        MoveObject bestMove = new();

        decimal previousBestScore = decimal.MinValue; // To track the best score of the previous depth
        int scoreStagnationDepth = 3; // Depth at which to start checking for score improvements

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            var moves = MoveGenerator.GenerateAllMoves(board, turn, true);
            var orderedMoves = moves.OrderByDescending(m => m.IsCapture == true);

            DetectCheckMateAndStalemate(board, turn, bestMove, out List<MoveObject> allPossibleMoves);

            bool scoreImproved = false;

            foreach (var move in orderedMoves)
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                if (turn == 0)
                {
                    score = Min(shadowBoard, 1, currentDepth - 1, alpha, beta);
                }
                else
                {
                    score = Max(shadowBoard, 0, currentDepth - 1, alpha, beta);
                }
                MoveHandler.RestoreStateFromSnapshot();

                if (turn == 0)
                {
                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                        scoreImproved = true;
                    }
                }
                else
                {
                    if (score <= beta)
                    {
                        beta = score;
                        bestMove = move;
                        scoreImproved = true;
                    }
                }

                // When time limit is reached, return the best move found so far
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Time limit reached, stopping search.");
                    Globals.PrincipalVariation.Add(bestMove);
                    return bestMove;
                }

                if (score >= 99999 || score <= -99999)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Best move over 99 in {currentDepth}: {MoveToString(bestMove)}");
                    Globals.PrincipalVariation.Add(bestMove);
                    return bestMove;
                }

                if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

                if (alpha >= beta)
                {
                    break;
                }
            }

            // Check if the best score has improved after the scoreStagnationDepth
            if (currentDepth > scoreStagnationDepth && !scoreImproved)
            {
                Console.WriteLine($"No improvement after depth {scoreStagnationDepth}, moving to next move.");
                QuiescenceSearch(board, alpha, beta, true);
                //continue; // Move to the next move


            }

            // Update previousBestScore for the next iteration
            previousBestScore = alpha;

            Console.WriteLine($"Best Move: {MoveToString(bestMove)} Depth: {currentDepth} Score: {score}");
        }
        Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
        Globals.PrincipalVariation.Add(bestMove);
        return bestMove;
    }


    public static decimal Max(int[] board, int turn, int depth, decimal alpha, decimal beta)
    {
        var moves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (depth == 0)
        {
            return QuiescenceSearch(board, alpha, beta, true);
        }

        
        var orderedMoves = OrderMoves(moves, board, turn);

        decimal maxScore = decimal.MinValue;

        foreach (var move in orderedMoves)
        {
            if (IgnoringMoves.Contains(move)) continue;

            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = Min(shadowBoard, turn ^ 1, depth - 1, alpha, beta);
            maxScore = Math.Max(maxScore, score);

            MoveHandler.RestoreStateFromSnapshot();

            if (maxScore >= beta)
            {
                return maxScore;
            }

            alpha = Math.Max(alpha, maxScore);
        }

        return maxScore;
    }


    public static decimal Min(int[] board, int turn, int depth, decimal alpha, decimal beta)
    {
        if (depth == 0)
        {
            return QuiescenceSearch(board, alpha, beta, false);
        }

        var moves = MoveGenerator.GenerateAllMoves(board, turn, true);
        var orderedMoves = OrderMoves(moves, board, turn);

        decimal minScore = decimal.MaxValue;

        foreach (var move in orderedMoves)
        {
            if (IgnoringMoves.Contains(move)) continue;

            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = Max(shadowBoard, turn ^ 1, depth - 1, alpha, beta);
            minScore = Math.Min(minScore, score);

            MoveHandler.RestoreStateFromSnapshot();

            if (minScore <= alpha)
            {
                return minScore;
            }

            beta = Math.Min(beta, minScore);
        }

        return minScore;
    }

    private static List<MoveObject> OrderMoves(IEnumerable<MoveObject> moves, int[] board, int turn)
    {
        Random random = new Random();

        var orderedMoves = moves.OrderByDescending(m => m.IsCapture)
                                .ThenByDescending(m => m.Priority)
                                .ThenByDescending(m => Evaluators.EvaluateMoveImpact(board, m))
                                .ThenBy(_ => random.Next())
                                .ToList();

        return orderedMoves;
    }



    private static decimal QuiescenceSearch(int[] board, decimal alpha, decimal beta, bool isMaximizing)
    {
        decimal standPat = Evaluators.GetByMaterial(board);

        if (isMaximizing)
        {
            if (standPat >= beta)
            {
                return beta;
            }
            if (alpha < standPat)
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
            if (beta > standPat)
            {
                beta = standPat;
            }
        }

        var moves = MoveGenerator.GenerateAllMoves(board, isMaximizing ? 0 : 1).Where(m => m.IsCapture);
        var orderedMoves = OrderMoves(moves, board, isMaximizing ? 0 : 1);

        foreach (var move in orderedMoves)
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = Evaluators.EvaluateMoveImpact(board, move) + (isMaximizing ? QuiescenceSearch(shadowBoard, alpha, beta, false) : QuiescenceSearch(shadowBoard, alpha, beta, true));

            if (isMaximizing)
            {
                if (score >= beta)
                {
                    MoveHandler.RestoreStateFromSnapshot();
                    return beta;
                }
                alpha = Math.Max(alpha, score);
            }
            else
            {
                if (score <= alpha)
                {
                    MoveHandler.RestoreStateFromSnapshot();
                    return alpha;
                }
                beta = Math.Min(beta, score);
            }

            MoveHandler.RestoreStateFromSnapshot();
        }

        return isMaximizing ? alpha : beta;
    }


    private static void DetectCheckMateAndStalemate(int[] board, int turn, MoveObject bestMove, out List<MoveObject> allPossibleMoves)
    {
        allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (!allPossibleMoves.Any())
        {
            if (turn == 0)
            {
                var blackMoves = MoveGenerator.GenerateAllMoves(board, 1, true);
                if (!blackMoves.Any()) Globals.Stalemate = true;
                Globals.CheckmateWhite = true;
            }
            else if (turn == 1)
            {
                var whiteMoves = MoveGenerator.GenerateAllMoves(board, 0, true);
                if (!whiteMoves.Any()) Globals.Stalemate = true;
                Globals.CheckmateBlack = true;
            }
        }
        bool onlyKingsRemain = board.All(p => p == MoveGenerator.whiteKing || p == MoveGenerator.blackKing || p == 0);
        if (onlyKingsRemain) Globals.Stalemate = true;
    }

    public static string MoveToString(MoveObject move)
    {
        string promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
        string castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";

        if (!string.IsNullOrEmpty(castle))
        {
            return castle;
        }
        if (move.StartSquare == move.EndSquare) return 0.ToString();
        return $"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}{promotion} ";
    }
}
