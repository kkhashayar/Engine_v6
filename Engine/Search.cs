using System.Diagnostics;

namespace Engine;

public static class Search
{
    public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        return MoveGenerator.GenerateAllMoves(board, turn, filter);
    }
    public static List<MoveObject> GetPrincipalVariation(int[] board, int turn, int maxDepth)
    {
        List<MoveObject> principalVariation = new List<MoveObject>();
        decimal alpha = decimal.MinValue;
        decimal beta = decimal.MaxValue;
        decimal score;

        for (int depth = 1; depth <= maxDepth; depth++)
        {
            List<MoveObject> tempPV = new List<MoveObject>();
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

            foreach (var move in allPossibleMoves)
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                if (turn == 0)
                {
                    score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, 1, tempPV);
                    if (score > alpha)
                    {
                        alpha = score;
                        principalVariation = new List<MoveObject> { move };
                        principalVariation.AddRange(tempPV);
                    }
                }
                else
                {
                    score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, 0, tempPV);
                    if (score < beta)
                    {
                        beta = score;
                        principalVariation = new List<MoveObject> { move };
                        principalVariation.AddRange(tempPV);
                    }
                }

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);
            }
        }

        return principalVariation;
    }
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        List<MoveObject> emptyPV = new List<MoveObject>();


        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        decimal alpha = decimal.MinValue;
        decimal beta = decimal.MaxValue;

        MoveObject bestMove = default;

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
            if (!allPossibleMoves.Any())
            {
                if (turn == 0) Globals.CheckmateWhite = true;
                else Globals.CheckmateBlack = true;
                return bestMove;
            }

            foreach (var move in allPossibleMoves)
            {
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    return bestMove;
                }

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score;
                if (turn == 0)
                {
                    score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, emptyPV);
                }
                else
                {
                    score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, emptyPV);
                }

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

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
            }

            Console.WriteLine($"Depth {currentDepth}: Best Move Found - {MoveToString(bestMove)} with score {(turn == 0 ? alpha : beta)}");
        }

        Globals.MovePrincipals.Add(bestMove);
        Console.WriteLine($"Best Move: {MoveToString(bestMove)}");
        return bestMove;
    }
  
    private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn, List<MoveObject> pv)
    {
        List<MoveObject> localPV = new List<MoveObject>();

        if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

        decimal bestScore = decimal.MinValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1, localPV);
            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

            if (score > bestScore)
            {
                bestScore = score;
                pv.Clear();
                pv.Add(move);
                pv.AddRange(localPV);

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
        }

        return alpha;
    }

  
    private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn, List<MoveObject> pv)
    {
        List<MoveObject> localPV = new List<MoveObject>();

        if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

        decimal bestScore = decimal.MaxValue;
        foreach (var move in GetAllPossibleMoves(board, turn, true))
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);

            decimal score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1, localPV);
            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

            if (score < bestScore)
            {
                bestScore = score;
                pv.Clear();
                pv.Add(move);
                pv.AddRange(localPV);

                if (score <= alpha)
                    return alpha;
                if (score < beta)
                    beta = score;
            }
        }

        return beta;
    }
    public static string MoveToString(MoveObject move)
    {
        string promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
        string castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";

        if (!string.IsNullOrEmpty(castle))
        {
            return castle;
        }

        return $"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}{promotion}";
    }
}


// OLD METHODS 
//public static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
//{


//    List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
//    if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MaxValue;

//    if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

//    foreach (var move in allPossibleMoves)
//    {
//        MoveHandler.RegisterStaticStates();

//        var pieceMoving = move.pieceType;
//        var targetSquare = board[move.EndSquare];
//        var promotedTo = move.PromotionPiece;

//        MoveHandler.MakeMove(board, move);

//        decimal score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1);

//        MoveHandler.RestoreStateFromSnapshot();
//        MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

//        if (score <= alpha) return alpha;
//        if (score < beta) beta = score;
//    }

//    return beta;
//}

//public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
//{
//    List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
//    if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MinValue;
//    // allPossibleMoves.Where(mo =>  Piece.IsWhite(mo.pieceType)).Count(), allPossibleMoves.Where(mo => Piece.IsBlack(mo.pieceType)).Count()
//    if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

//    foreach (var move in allPossibleMoves)
//    {
//        MoveHandler.RegisterStaticStates();

//        var pieceMoving = move.pieceType;
//        var targetSquare = board[move.EndSquare];
//        var promotedTo = move.PromotionPiece;

//        MoveHandler.MakeMove(board, move);

//        decimal score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1);

//        MoveHandler.RestoreStateFromSnapshot();
//        MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

//        if (score >= beta) return beta;
//        if (score > alpha) alpha = score;
//    }

//    return alpha;
//}
