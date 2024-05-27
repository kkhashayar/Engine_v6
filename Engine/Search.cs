using System.Diagnostics;
using System.Security;

namespace Engine;

public static class Search
{
    public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        return MoveGenerator.GenerateAllMoves(board, turn, filter).OrderByDescending(m => m.Priority).ToList();
    }

    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        double alpha = double.NegativeInfinity;
        double beta = double.PositiveInfinity;

        MoveObject bestMove = default;
        List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

       
        if (!allPossibleMoves.Any())
        {
            if (turn == 0)
            {
                var blackMoves = GetAllPossibleMoves(board, 1, true);
                if (!blackMoves.Any()) Globals.Stalemate = true;
                Globals.CheckmateWhite = true;
                return bestMove;
            }
            else if (turn == 1)
            {
                var whiteMoves = GetAllPossibleMoves(board, 0, true);
                if (!whiteMoves.Any()) Globals.Stalemate = true;
                Globals.CheckmateBlack = true;
                return bestMove;
            }
        }
        // Two kings stalemate check
        bool onlyKings = board.All(p => p == MoveGenerator.whiteKing || p == MoveGenerator.blackKing || p == MoveGenerator.None) && board.Contains(MoveGenerator.whiteKing) && board.Contains(MoveGenerator.blackKing);
        if (onlyKings)
        {
            Globals.Stalemate = true; 
        }

        if (allPossibleMoves.Count == 1)
        {
            bestMove = allPossibleMoves[0];
            return bestMove;
        }

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            foreach (var move in allPossibleMoves)
            {
                if (stopwatch.Elapsed >= maxTime)
                {
                    Globals.PrincipalVariation.Add(bestMove);
                    break;
                }

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                double score;
    

                if (turn == 0)
                {
                    score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1);

                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                    }

                    if (score >= beta)
                    {
                        Console.WriteLine($"Pruning: Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
                        Globals.PrincipalVariation.Add(bestMove);   
                        return bestMove;
                    }
                }
                else
                {
                    score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);

                    if (score < beta)
                    {
                        beta = score;
                        bestMove = move;
                    }

                    if (score <= alpha)
                    {
                        Console.WriteLine($"Pruning: Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
                        Globals.PrincipalVariation.Add(bestMove);
                        return bestMove; 
                    }
                }
                
                MoveHandler.RestoreStateFromSnapshot(); 

            }
            Console.WriteLine($"Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
        }
        Console.WriteLine($"Final Best Move: {MoveToString(bestMove)}");
        Globals.PrincipalVariation.Add(bestMove);

        return bestMove;
    }

    private static double AlphaBetaMax(int depth, double alpha, double beta, int[] board, int turn)
    {
        var allPossibleMoves = GetAllPossibleMoves(board, turn, true);

        //if (depth == 0)
        //{
        //    Evaluators.GetGameStageAndPositionalFacts(board, turn, allPossibleMoves.Count);
        //    var materialEvaluation = Evaluators.GetByMaterial();
        //    var mobilityEvaluation = Evaluators.GetByMobility();
        //    var positionalWeight = Evaluators.whitePositionalWeight - Evaluators.blackPositionalWeight;

        //    // Define weights
        //    double materialWeight = 1.0;
        //    double mobilityWeight = 0.3;
        //    double positionalWeightFactor = 0.2;

        //    // Calculate total evaluation with weights
        //    return (double)(materialWeight * materialEvaluation +
        //                 mobilityWeight * mobilityEvaluation +
        //                 positionalWeightFactor * positionalWeight);
        //}

        if(depth == 0)
        {
            return Evaluators.GetByMaterial();  
        }

        double bestScore = double.NegativeInfinity;
        foreach (var move in allPossibleMoves)
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            double score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1);
            MoveHandler.RestoreStateFromSnapshot();

            if (score > bestScore)
            {
                bestScore = score;
                if (score >= beta)
                    return beta; // Beta cutoff
                if (score > alpha)
                    alpha = score; // Alpha update
            }
        }
        return bestScore;
    }

    private static double AlphaBetaMin(int depth, double alpha, double beta, int[] board, int turn)
    {
        var allPossibleMoves = GetAllPossibleMoves(board, turn, true);

        //if (depth == 0)
        //{
        //    Evaluators.GetGameStageAndPositionalFacts(board, turn, allPossibleMoves.Count);
        //    var materialEvaluation = Evaluators.GetByMaterial();
        //    var mobilityEvaluation = Evaluators.GetByMobility();
        //    var positionalWeight = Evaluators.whitePositionalWeight - Evaluators.blackPositionalWeight;

        //    // Define weights
        //    double materialWeight = 1.0;
        //    double mobilityWeight = 0.3;
        //    double positionalWeightFactor = 0.2;

        //    // Calculate total evaluation with weights
        //    return -(double)(materialWeight * materialEvaluation +
        //                 mobilityWeight * mobilityEvaluation +
        //                 positionalWeightFactor * positionalWeight);
        //}


        if(depth == 0)
        {
            return -Evaluators.GetByMaterial();
        }


        double bestScore = double.PositiveInfinity;
        foreach (var move in allPossibleMoves)
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            double score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1);
            MoveHandler.RestoreStateFromSnapshot();

            if (score < bestScore)
            {
                bestScore = score;
                if (score <= alpha)
                    return alpha; // Alpha cutoff
                if (score < beta)
                    beta = score; // Beta update
            }
        }
        return bestScore;
    }

    public static string MoveToString(MoveObject move)
    {
        string promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
        string castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";

        if (!string.IsNullOrEmpty(castle))
            return castle;

        return $"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}{promotion}";
    }
}
