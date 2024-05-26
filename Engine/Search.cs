using System.Diagnostics;

namespace Engine;

public static class Search
{
    public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);

        var orderedmoves = moves.OrderByDescending(m => m.Priority).ToList();

        return orderedmoves;
    }

    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        decimal alpha = decimal.MinValue;
        decimal beta = decimal.MaxValue;

        MoveObject bestMove = default;
        List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

        // Handling stalemate and checkmates 
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

        if (allPossibleMoves.Count == 1)
        {
            bestMove = allPossibleMoves[0];
            return bestMove;
        }

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            if (stopwatch.Elapsed >= maxTime) break;
           
            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i];



                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score;
                if (turn == 0)
                {
                    score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1);
                }
                else
                {
                    score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);
                }

                MoveHandler.RestoreStateFromSnapshot();


                if (turn == 0 && score > alpha)
                {
                    alpha = score;
                    bestMove = move;
                }
                else if (turn != 0 && score < beta)
                {
                    beta = score;
                    bestMove = move;
                }

                // Return immediately if a decisive score is found
                //if (score >= 109 || score <= -109)
                //{
                //    Console.WriteLine();
                //    Console.WriteLine($"Best move over 99 in {currentDepth}: {MoveToString(bestMove)}");
                //    Globals.PrincipalVariation.Add(bestMove);
                //    return bestMove;
                //}
            }
            Console.WriteLine($"Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
        }
        Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
        Globals.PrincipalVariation.Add(bestMove);

        return bestMove;
    }


    private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {

        if (depth == 0) return Evaluators.GetByMaterial(board, turn);

        decimal bestScore = decimal.MinValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1);
            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);


            if (score > bestScore)
            {
                bestScore = score;

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
        }

        return bestScore;
    }

    private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {

        if (depth == 0) return -Evaluators.GetByMaterial(board, turn);

        decimal bestScore = decimal.MaxValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1);
            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

            if (score < bestScore)
            {
                bestScore = score;

                if (score <= alpha)
                    return alpha;
                if (score < beta)
                    beta = score;
            }
        }

        return bestScore;
    }


    public static string MoveToString(MoveObject move)
    {
        string promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
        string castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";

        if (!string.IsNullOrEmpty(castle))
        {
            return castle;
        }

        return $"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}{promotion} ";
    }
}


