using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

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

        if (!allPossibleMoves.Any())
        {
            if(turn == 0)
            {
                var blackPossiblemoves = GetAllPossibleMoves(board, 1, true);
                if (blackPossiblemoves.Any(mo => mo.EndSquare == Globals.GetWhiteKingSquare(board))) Globals.CheckmateWhite = true;
                else
                {
                    Globals.Stalemate = true;
                    return bestMove;
                }
            }

            else if(turn == 1)
            {
                var whitePossiblemoves = GetAllPossibleMoves(board, 0, true);
                if (whitePossiblemoves.Any(mo => mo.EndSquare == Globals.GetBlackKingSquare(board))) Globals.CheckmateBlack = true;
                else
                {
                    Globals.Stalemate = true;
                    return bestMove;
                }
            }
        }

        if(allPossibleMoves.Count == 1)
        {
            bestMove = allPossibleMoves[0]; 
            return bestMove;
        }

        
        if(turn == 0)
        {
            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i]; 
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    break;
                }

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score;
                score = AlphaBetaMin(maxDepth - 1, alpha, beta, shadowBoard, 1);
               
                if (score > alpha)
                {
                    alpha = score;
                    bestMove = move;
                }

                MoveHandler.RestoreStateFromSnapshot();
                //MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);
                Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
            }
        }

        else if(turn == 1)
        {
            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i];
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    break;
                }

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score ;
                score = AlphaBetaMax(maxDepth - 1, alpha, beta, shadowBoard, 0);
                
                if (score < beta)
                {
                    beta = score;
                    bestMove = move;
                }
                MoveHandler.RestoreStateFromSnapshot();
                //MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);
                Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
            }
        }

        
        return bestMove;
    }


    private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {
      
        if (depth == 0) return Evaluators.EvaluatePosition(board, turn, 0, 0);

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

                if (score >= beta) return beta;
              
                if (score > alpha) alpha = score;
            }
        }

        return bestScore;
    }

    private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {
      
        if (depth == 0) return Evaluators.EvaluatePosition(board, turn, 0, 0);  

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

                if (score <= alpha) return alpha;
                if (score < beta) beta = score;
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
