using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            decimal alpha = decimal.MinValue;
            decimal beta = decimal.MaxValue;

            MoveObject bestMove = default;
            MoveObject moveAtCurrentDepth = default;

            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
                if (!allPossibleMoves.Any())
                {
                    if (turn == 0) Globals.CheckmateWhite = true;
                    else Globals.CheckmateBlack = true;
                    return bestMove;
                }
              
                if (turn == 0)
                {
                    foreach (var move in allPossibleMoves)
                    {
                        int[] shadowBoard = (int[])board.Clone();
                        MoveHandler.RegisterStaticStates();

                        var pieceMoving = move.pieceType;
                        var targetSquare = shadowBoard[move.EndSquare];
                        var promotedTo = move.PromotionPiece;

                        MoveHandler.MakeMove(shadowBoard, move);
                        decimal score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1);

                        MoveHandler.RestoreStateFromSnapshot();
                        MoveHandler.UndoMove(shadowBoard, move, pieceMoving, targetSquare, promotedTo);

                        if (score > alpha)
                        {
                            alpha = score;
                            moveAtCurrentDepth = move;
                        }
                    }
                }
                else
                {
                    foreach (var move in allPossibleMoves)
                    {
                        int[] shadowBoard = (int[])board.Clone();
                        MoveHandler.RegisterStaticStates();

                        var pieceMoving = move.pieceType;
                        var targetSquare = shadowBoard[move.EndSquare];
                        var promotedTo = move.PromotionPiece;

                        MoveHandler.MakeMove(shadowBoard, move);
                        decimal score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);

                        MoveHandler.RestoreStateFromSnapshot();
                        MoveHandler.UndoMove(shadowBoard, move, pieceMoving, targetSquare, promotedTo);

                        if (score < beta)
                        {
                            beta = score;
                            moveAtCurrentDepth = move;
                        }
                    }
                }

                if (stopwatch.Elapsed >= maxTime)
                {
                    return bestMove;
                }

                // Update bestMove with the best found at the current depth
                bestMove = moveAtCurrentDepth;
                Console.WriteLine($"Depth {currentDepth}: Best Move Found - {MoveToString(bestMove)} with score {(turn == 0 ? alpha : beta)}");
            }
            Globals.MovePrincipals.Add(bestMove);
            Console.WriteLine($"{Green}Best Move: {MoveToString(bestMove)}{Reset}");
            return bestMove;
        }
        public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn)
        {
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MinValue;

            if (depth == 0) return 
                    Evaluators.GetByMaterial(board, allPossibleMoves.Where(mo =>  Piece.IsWhite(mo.pieceType)).Count(), allPossibleMoves.Where(mo => Piece.IsBlack(mo.pieceType)).Count());
           // if(depth ==  1 ) return QuiescenceSearch(board, alpha, beta, turn);   
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
            
          
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return decimal.MaxValue;

            if (depth == 0) return
                    Evaluators.GetByMaterial(board, allPossibleMoves.Where(mo => Piece.IsWhite(mo.pieceType)).Count(), allPossibleMoves.Where(mo => Piece.IsBlack(mo.pieceType)).Count());

            //if (depth == 1) return QuiescenceSearch(board, alpha, beta, turn);
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

        public static decimal QuiescenceSearch(int[] board, decimal alpha, decimal beta, int turn)
        {
            List<MoveObject> moves = GetAllPossibleMoves(board, turn, true);
            
            decimal standPat = Evaluators.GetByMaterial(board, moves.Where(mo => Piece.IsWhite(mo.pieceType)).Count(), moves.Where(mo => Piece.IsBlack(mo.pieceType)).Count());

            if (standPat >= beta)
                return beta;
            if (alpha < standPat)
                alpha = standPat;
            
            foreach (var move in moves)
            {
                if (!move.IsCapture)
                    continue;

                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(board, move);

                decimal score = -QuiescenceSearch(board, -beta, -alpha, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, move.pieceType, board[move.EndSquare], move.PromotionPiece);

                if (score >= beta)
                    return beta;
                if (score > alpha)
                    alpha = score;
            }
            return alpha;
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
}
