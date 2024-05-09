using System;
using System.Collections.Generic;
using System.Threading;

namespace Engine
{
    public static class Search
    {
        private static readonly string Green = "\u001b[32m";
        private static readonly string Red = "\u001b[31m";
        private static readonly string Reset = "\u001b[0m";

        public static MoveObject GetBestMove(int[] board, int turn, int depth)
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
                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                string color = (turn == 0 && score > bestScore) || (turn == 1 && score < bestScore) ? Green : Red;
                Console.WriteLine($"{color}{MoveToString(move)}: {score}{Reset}");

                if ((turn == 0 && score > bestScore) || (turn == 1 && score < bestScore))
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            Console.WriteLine($"{Green}Best Move: {MoveToString(bestMove)} with Score: {bestScore}{Reset}");
            Console.WriteLine($"{Green}Best Move is: {MoveToString(bestMove)}{Reset}");
            Thread.Sleep(1000);

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
                if (IsCheckmate(turn, board))
                {
                    return Evaluators.GetCheckmateScore(turn); // Checkmate
                }
                else
                {
                    return Evaluators.GetStalemateScore(); // Stalemate
                }
            }

            if (turn == 0) // Maximizing player (White)
            {
                decimal maxEval = decimal.MinValue;
                foreach (var move in moves)
                {
                    // Make a move
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
                        break; // Alpha cutoff
                    }
                }
                return maxEval;
            }
            else // Minimizing player (Black)
            {
                decimal minEval = decimal.MaxValue;
                foreach (var move in moves)
                {
                    // Make a move
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
                        break; // Beta cutoff
                    }
                }
                return minEval;
            }
        }

        private static string MoveToString(MoveObject move)
        {
            string promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
            string castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";

            if (!string.IsNullOrEmpty(castle))
            {
                return castle;
            }

            return $"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}{promotion}";
        }

        private static bool IsCheckmate(int turn, int[] board)
        {
            // Determine if the current player is in checkmate
            List<MoveObject> moves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (moves.Count == 0)
            {
                if ((turn == 0 && Globals.CheckWhite) || (turn == 1 && Globals.CheckBlack))
                {
                    return true; // Checkmate
                }
            }
            return false; // Not a checkmate
        }
    }
}
