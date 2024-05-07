using System;
using System.Collections.Generic;

namespace Engine
{
    public static class Search
    {

        public static MoveObject FindBestMove(int[] board, int turn, int depth)
        {
            decimal bestScore = turn == 0 ? decimal.MinValue : decimal.MaxValue;
            MoveObject bestMove = default;

            List<MoveObject> moves = MoveGenerator.GenerateAllMoves(board, turn, true);
            foreach (var move in moves)
            {
                MoveHandler.RegisterStaticStates();
                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

                MoveHandler.MakeMove(board, move);
                decimal score = -AlphaBeta(depth - 1, decimal.MinValue, decimal.MaxValue, board, turn ^ 1);
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if ((turn == 0 && score > bestScore) || (turn == 1 && score < bestScore))
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }
        public static decimal AlphaBeta(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            if (depth == 0)
            {
                return Evaluators.GetByMaterial(board);
            }

            List<MoveObject> moves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (moves.Count == 0)
            {
                return turn == 0 ? -9999m : 9999m; // Checkmate or stalemate scenarios
            }

            if (turn == 0) // Maximizing player (White)
            {
                decimal maxEval = decimal.MinValue;
                foreach (var move in moves)
                {
                    // Making a move 
                    MoveHandler.RegisterStaticStates();
                    var pieceMoving = move.pieceType;
                    var targetSquare = board[move.EndSquare];
                    var promotedTo = move.PromotionPiece;

                    MoveHandler.MakeMove(board, move);

                    decimal eval = AlphaBeta(depth - 1, alpha, beta, board, 1);

                    // Undo move 
                    MoveHandler.RestoreStateFromSnapshot();
                    MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                    maxEval = Math.Max(maxEval, eval);
                    alpha = Math.Max(alpha, eval);
                    if (beta <= alpha)
                    {
                        break; // cutoff 
                    }
                }
                return maxEval;
            }
            else // Minimizing player (Black)
            {
                decimal minEval = decimal.MaxValue;
                foreach (var move in moves)
                {
                    // make a move 
                    MoveHandler.RegisterStaticStates();
                    var pieceMoving = move.pieceType;
                    var targetSquare = board[move.EndSquare];
                    var promotedTo = move.PromotionPiece;

                    MoveHandler.MakeMove(board, move);
                    decimal eval = AlphaBeta(depth - 1, alpha, beta, board, 0);

                    // Undo move 
                    MoveHandler.RestoreStateFromSnapshot(); 
                    MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                    minEval = Math.Min(minEval, eval);
                    beta = Math.Min(beta, eval);
                    if (beta <= alpha)
                    {
                        break; // α cutoff
                    }
                }
                return minEval;
            }
        }
    }
}
