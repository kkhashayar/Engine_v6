using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Engine
{
    public static class NegaaxSearch
    {
        public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
        {
            var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
            return moves.OrderByDescending(m => m.Priority).ToList();
        }

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            decimal alpha = decimal.MinValue;
            decimal beta = decimal.MaxValue;

            MoveObject bestMove = MoveObject.Empty;
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);



            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                if (stopwatch.Elapsed >= maxTime)
                {
                    Console.WriteLine("Stopping search due to time limit.");
                    return bestMove;
                }

                foreach (var move in allPossibleMoves)
                {
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.RegisterStaticStates();
                    MoveHandler.MakeMove(shadowBoard, move);

                    decimal score = -Negamax(currentDepth - 1, -beta, -alpha, shadowBoard, turn ^ 1);

                    MoveHandler.RestoreStateFromSnapshot();
                    MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

                    if (score > alpha)
                    {
                        alpha = score;
                        bestMove = move;
                    }

                    if (alpha >= beta)
                        break;
                }

                Console.WriteLine($"Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
            }

            Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
            Globals.PrincipalVariation.Add(bestMove);

            return bestMove;
        }

        private static decimal Negamax(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            if (depth == 0)
                return Evaluators.EvaluatePosition(board, turn, 0, 0);

            decimal maxScore = decimal.MinValue;
            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score = -Negamax(depth - 1, -beta, -alpha, shadowBoard, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(shadowBoard, move, move.pieceType, shadowBoard[move.EndSquare], move.PromotionPiece);

                if (score > maxScore)
                {
                    maxScore = score;
                }

                if (maxScore > alpha)
                {
                    alpha = maxScore;
                }

                if (alpha >= beta)
                {
                    break;
                }
            }

            return maxScore;
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
}
