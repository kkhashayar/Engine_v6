using System.Diagnostics;

namespace Engine;

public static class Search
{
    public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
    {
        return MoveGenerator.GenerateAllMoves(board, turn, filter);
    }
    
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        decimal alpha = decimal.MinValue;
        decimal beta = decimal.MaxValue;

        MoveObject bestMove = default;

        for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
        {
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
            
            
            // Check if any side is checkmated // TODO draw by move into check  
            if (!allPossibleMoves.Any())
            {
                if (turn == 0) Globals.CheckmateWhite = true;
                else Globals.CheckmateBlack = true;
                return bestMove;
            }

            for (int i = 0; i < allPossibleMoves.Count; i++)
            {
                var move = allPossibleMoves[i];

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
                    score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1);
                }
                else
                {
                    score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);
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
        Console.WriteLine($"Best Move: {MoveToString(bestMove)}");
        return bestMove;
    }
  
    private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {
      
        if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

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
                
                if(bestScore >= 99) return bestScore;   

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
        }

        return alpha;
    }

    private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
    {
    
        if (depth == 0) return Evaluators.GetByMaterial(board, 0, 0);

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
                
                if(bestScore < -100) return bestScore;

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
