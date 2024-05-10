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

        public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
        {
            return MoveGenerator.GenerateAllMoves(board, turn, filter); 
        }
        public static MoveObject GetBestMove(int[] board, int turn, int depth)
        {
            decimal alpha = decimal.MinValue;
            decimal beta = decimal.MaxValue;

            MoveObject bestMove = default;

            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

            if (!allPossibleMoves.Any())
            {
                if(turn == 0) Globals.CheckmateWhite = true;
                else Globals.CheckmateBlack = true;
            }

            Console.WriteLine($"Candidate moves for {(turn == 0 ? "White" : "Black")}:");

            if(turn == 0)
            {
                foreach (var move in allPossibleMoves)
                {
                    // int[] shadowBoard = (int[])board.Clone();
                    ApplyMove(board, move);
                    decimal score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1);

                    if(score > alpha)
                    {
                        alpha = score;
                        bestMove = move;    
                    }
                }
                bestMove.Score = alpha; 
            }

            else
            {
                foreach(var move in allPossibleMoves)
                {
                    ApplyMove(board, move); 
                    decimal score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1);
                    
                    if(score < beta)
                    {
                        beta = score;
                        bestMove = move; 
                    }   
                
                }
                bestMove.Score = beta;
            }
            

            Console.WriteLine($"{Green}Best Move: {MoveToString(bestMove)}{Reset}");
            Console.WriteLine($"{Green}Best Move is: {MoveToString(bestMove)}{Reset}");
            Thread.Sleep(1000);

            return bestMove;
        }

        

        public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            if (depth == 0)  return Evaluators.GetByMaterial(board);
            

            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            
            if (allPossibleMoves.Count == 0 || allPossibleMoves == null) return decimal.MaxValue;
            
            foreach(var move in allPossibleMoves)
            {
                ApplyMove(board, move);
                decimal score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1);

                if(score >= beta) return beta;
                if (score > alpha) alpha = score; 
            }
            
            return alpha;
        }

        public static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            if(depth == 0) return - Evaluators.GetByMaterial(board);    

            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);

            if (allPossibleMoves.Count == 0 || allPossibleMoves == null) return decimal.MinValue;

            foreach (var move in allPossibleMoves)
            {
                ApplyMove(board, move);
                decimal score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1);

                if (score <= alpha) return alpha;

                if (score < beta) beta = score;
            }

            return beta;    
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
        private static void ApplyMove(int[] board, MoveObject move)
        {
            MoveHandler.RegisterStaticStates();
            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;
            MoveHandler.MakeMove(board, move);
        }
    }
}
