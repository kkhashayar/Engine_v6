using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Engine
{
    public static class Search
    {
        
        public static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
        {
            var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
            var orderedmoves = moves.OrderByDescending(m => m.Priority)
                            .ThenByDescending(m => m.IsCapture)
                            .ThenByDescending(m => m.IsCheck)
                            .ThenByDescending(m => m.IsPromotion)
                            .ThenByDescending(m => m.IsEnPassant)
                            .ToList();
            return orderedmoves;
        }

        // Find the best move for the given board, turn, depth, and time constraint
        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            // Initialize stopwatch to enforce maxTime
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            decimal alpha = decimal.MinValue;
            decimal beta = decimal.MaxValue;

            MoveObject bestMove = default;
            List<MoveObject> allPossibleMoves = GetAllPossibleMoves(board, turn, true);

            // Check for no possible moves and handle stalemate/checkmate
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

           
            // Iterative deepening
            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                foreach (var move in allPossibleMoves)
                {
                    // Check if the time limit is exceeded
                    if (stopwatch.Elapsed >= maxTime)
                    {
                        Console.WriteLine("Stopping search due to time limit.");
                        return bestMove;
                    }

                    // Clone the board and make the move
                    int[] shadowBoard = (int[])board.Clone();
                    MoveHandler.RegisterStaticStates();
                    MoveHandler.MakeMove(shadowBoard, move);

                    // Evaluate the move using alpha-beta pruning
                    decimal score;
                    if (turn == 0)
                    {
                        score = AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move);
                    }
                    else
                    {
                        score = AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move);
                    }

                    // Restore the board state
                    MoveHandler.RestoreStateFromSnapshot();

                    // Update the best move based on the score
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

                    // Check for decisive scores and return early
                    if (score >= 99999 || score <= -99999)
                    {
                        Console.WriteLine($"Best move over 99 in {currentDepth}: {MoveToString(bestMove)}");
                        Globals.PrincipalVariation.Add(bestMove);
                        return bestMove;
                    }

                    if (allPossibleMoves.Count == 1) return allPossibleMoves[0];
                }
                Console.WriteLine($"Best Move: {MoveToString(bestMove)} Depth: {currentDepth}");
            }
            Console.WriteLine($"Best Move: {MoveToString(bestMove)} ");
            Globals.PrincipalVariation.Add(bestMove);

            return bestMove;
        }

        //private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn, MoveObject moveToEval)
        //{

        //    if (depth == 0) 
        //    {
        //        int score = Evaluators.GetByMaterial(board, turn);
        //        return score += Evaluators.GetPositionalValue(board, moveToEval.EndSquare, turn);

        //    }


        //    decimal bestScore = decimal.MinValue;
        //    foreach (var move in GetAllPossibleMoves(board, turn, true))
        //    {
        //        int[] shadowBoard = (int[])board.Clone();
        //        MoveHandler.RegisterStaticStates();
        //        MoveHandler.MakeMove(shadowBoard, move);

        //        decimal score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
        //        MoveHandler.RestoreStateFromSnapshot();

        //        bestScore = Math.Max(bestScore, score);
        //        alpha = Math.Max(alpha, score);
        //        if (beta <= alpha)
        //        {
        //            break;
        //        }
        //    }

        //    return bestScore;
        //}

        //// Alpha-beta pruning min function
        //private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn, MoveObject moveToEval)
        //{
        //    if (depth == 0) 
        //    {
        //        int score = Evaluators.GetByMaterial(board, turn);
        //        return score += Evaluators.GetPositionalValue(board, moveToEval.EndSquare, turn);
        //    } 

        //    decimal bestScore = decimal.MaxValue;
        //    foreach (var move in GetAllPossibleMoves(board, turn, true))
        //    {
        //        int[] shadowBoard = (int[])board.Clone();
        //        MoveHandler.RegisterStaticStates();
        //        MoveHandler.MakeMove(shadowBoard, move);

        //        decimal score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
        //        MoveHandler.RestoreStateFromSnapshot();

        //        bestScore = Math.Min(bestScore, score);
        //        beta = Math.Min(beta, score);
        //        if (beta <= alpha)
        //        {
        //            break;
        //        }
        //    }

        //    return bestScore;
        //}

        private static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn, MoveObject moveToEval)
        {
            if (depth == 0)
            {
                return Evaluators.GetByMaterial(board, turn);
                
            }

            decimal bestScore = decimal.MinValue;
            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
                MoveHandler.RestoreStateFromSnapshot();

                bestScore = Math.Max(bestScore, score);
                alpha = Math.Max(alpha, score);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestScore;
        }

        private static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn, MoveObject moveToEval)
        {
            if (depth == 0)
            {
                return Evaluators.GetByMaterial(board, turn);
            
            }

            decimal bestScore = decimal.MaxValue;
            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                decimal score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, turn ^ 1, move);
                MoveHandler.RestoreStateFromSnapshot();

                bestScore = Math.Min(bestScore, score);
                beta = Math.Min(beta, score);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return bestScore;
        }


        // Convert a move to string 
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
