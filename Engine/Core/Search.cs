using Engine.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine
{
    public static class Search
    {

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            //decimal alpha = decimal.MinValue;
            //decimal beta = decimal.MaxValue;

            decimal alpha = -999999;
            decimal beta = 999999;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            MoveObject bestMove = default;

            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

            allPossibleMoves = allPossibleMoves
                .OrderByDescending(mo => mo.IsCapture)
                .ThenByDescending(mo => mo.IsCheck)
                .ToList();

           
            if (allPossibleMoves.Count == 0)
            {
                if (turn == 0) 
                {
                    Globals.CheckmateWhite = true;
                } 
                else if(turn == 1)
                {
                    Globals.CheckmateBlack = true;
                }
                
                else Globals.Stalemate = true;  
                return bestMove;
            }

            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {

                
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

                    decimal score = (turn == 0) ? AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1) : AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);
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

                Console.WriteLine($"Depth {currentDepth}: Best Move Found - {Globals.MoveToString(bestMove)} with score {(turn == 0 ? alpha : beta)}");
            }

            
            Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)}");
            return bestMove;
        }
        public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
   
            if (depth == 0) return Evaluators.GetByMaterial(board, turn);
            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MinValue;
            allPossibleMoves = allPossibleMoves
                .OrderByDescending(mo => mo.IsCapture)
                .ThenByDescending(mo => mo.IsCheck)
                .ToList();

            foreach (var move in allPossibleMoves)
            {
                MoveHandler.RegisterStaticStates();

                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

                MoveHandler.MakeMove(board, move);

                decimal score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if (score >= beta) return beta;
                if (score > alpha) alpha = score;
            }

            return alpha;
        }

        public static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            
            if (depth == 0) return Evaluators.GetByMaterial(board, turn);
            var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MaxValue;

            allPossibleMoves = allPossibleMoves
                .OrderByDescending(mo => mo.IsCapture)
                .ThenByDescending(mo => mo.IsCheck)
                .ToList();
            foreach (var move in allPossibleMoves)
            {
                MoveHandler.RegisterStaticStates();

                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

                MoveHandler.MakeMove(board, move);

                decimal score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if (score <= alpha) return alpha;
                if (score < beta) beta = score;
            }

            return beta;
        }
      
    }
}