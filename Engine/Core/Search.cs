﻿//using Engine.Core;
//using System.Diagnostics;

//namespace Engine
//{
//    public static class Search
//    {
//        public static List<MoveObject> tempMoves = new List<MoveObject>();  
//        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
//        {
//            Stopwatch stopwatch = new Stopwatch();
//            stopwatch.Start();

//            // Implementing PV 
//            PrincipalVariation pv = new();
//            MoveObject bestMove = default;

//            var  allPossibleMoves = GetAllPossibleMoves(board, turn, true);

//            DetectStalemateAndCheckmates(board, turn, bestMove, allPossibleMoves);

//            int alpha = int.MinValue;
//            int beta = int.MaxValue;

//            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
//            {
//                foreach (var move in allPossibleMoves)
//                {
//                    int[] shadowBoard = ApplyMove(board, move);

//                    PrincipalVariation currentPv = new PrincipalVariation();

//                    int score = (turn == 0) ?
//                        AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move, currentPv) :
//                        AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move, currentPv);

//                    MoveHandler.RestoreStateFromSnapshot();

//                    if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

//                    if ((turn == 0 && score > alpha) || (turn == 1 && score < beta))
//                    {
//                        if (turn == 0) alpha = score;
//                        else beta = score;
//                        bestMove = move;
//                        bestMove.Score = score;

//                        // Save the PV
//                        pv.Moves.Clear();
//                        pv.AddMove(move);
//                        pv.Moves.AddRange(currentPv.Moves);
//                    }
//                    if (!tempMoves.Contains(bestMove))
//                    {
//                        tempMoves.Add(bestMove);
//                        Console.ResetColor();
//                        Console.ForegroundColor = ConsoleColor.Green;
//                        Console.WriteLine($"Candidate: {Globals.MoveToString(bestMove)} Score:{bestMove.Score} Depth: {currentDepth}");
//                    }


//                    if (stopwatch.Elapsed >= maxTime)
//                    {
//                        Console.WriteLine("Stopping search due to time limit.");
//                        return bestMove;
//                    }
//                }

//            }
//            Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");
//            return bestMove;
//        }

//        private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval, PrincipalVariation pv)
//        {
//            if (depth == 0)
//            {
//                pv.Count = 0;

//                return Quiescence(board, alpha, beta, turn);
//                // return Evaluators.EvaluatePosition(board, turn);
//            }

//            int bestScore = int.MinValue;
//            PrincipalVariation line = new PrincipalVariation();

//            foreach (var move in GetAllPossibleMoves(board, turn, true))
//            {
//                int[] shadowBoard = ApplyMove(board, move);
//                int score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, 1 - turn, move, line);
//                MoveHandler.RestoreStateFromSnapshot();

//                if (score > bestScore)
//                {
//                    bestScore = score;
//                    pv.Moves.Clear();
//                    pv.AddMove(move);
//                    pv.Moves.AddRange(line.Moves);
//                }
//                alpha = Math.Max(alpha, score);
//                if (beta <= alpha)
//                {
//                    break;
//                }
//            }
//            return bestScore;
//        }

//        private static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval, PrincipalVariation pv)
//        {
//            if (depth == 0)
//            {
//                pv.Count = 0;
//                // if (moveToEval.IsCapture || moveToEval.IsCheck)
//                return Quiescence(board, alpha, beta, turn);
//                // return Evaluators.EvaluatePosition(board, turn);
//            }

//            int bestScore = int.MaxValue;

//            PrincipalVariation line = new PrincipalVariation();

//            foreach (var move in GetAllPossibleMoves(board, turn, true))
//            {
//                int[] shadowBoard = ApplyMove(board, move);
//                int score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, 1 - turn, move, line);
//                MoveHandler.RestoreStateFromSnapshot();

//                if (score < bestScore)
//                {
//                    bestScore = score;
//                    pv.Moves.Clear();
//                    pv.AddMove(move);
//                    pv.Moves.AddRange(line.Moves);
//                }
//                beta = Math.Min(beta, score);
//                if (beta <= alpha)
//                {
//                    break;
//                }
//            }
//            return bestScore;
//        }

//        private static int Quiescence(int[] board, int alpha, int beta, int turn)
//        {
//            int maxDepth = 2;
//            return QuiescenceInternal(board, alpha, beta, turn, maxDepth);
//        }

//        private static int QuiescenceInternal(int[] board, int alpha, int beta, int turn, int depth)
//        {
//            if (depth == 0)
//            {
//                return Evaluators.EvaluatePosition(board, turn);
//            }

//            int standPat = Evaluators.EvaluatePosition(board, turn);
//            if (standPat >= beta)
//                return beta;
//            if (standPat > alpha)
//                alpha = standPat;

//            List<MoveObject> captures = GetAllPossibleMoves(board, turn, true)
//                .Where(m => m.IsCapture && m.IsCheck)
//                .OrderByDescending(m => m.IsCapture && m.IsCheck)
//                .ToList();


//            foreach (var move in captures)
//            {
//                int[] shadowBoard = ApplyMove(board, move);

//                int score = -QuiescenceInternal(shadowBoard, -beta, -alpha, turn ^ 1, depth - 1);
//                MoveHandler.RestoreStateFromSnapshot();

//                if (score >= beta)
//                {
//                    return beta;
//                }
//                if (score > alpha)
//                {
//                    alpha = score;
//                }
//            }

//            return alpha;
//        }

//        private static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
//        {
//            var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
//            return moves;
//        }

//        private static int[] ApplyMove(int[] board, MoveObject move)
//        {
//            int[] shadowBoard = (int[])board.Clone();
//            MoveHandler.RegisterStaticStates();
//            MoveHandler.MakeMove(shadowBoard, move);
//            return shadowBoard;
//        }

//        private static void DetectStalemateAndCheckmates(int[] board, int turn, MoveObject bestMove, List<MoveObject> allPossibleMoves)
//        {
//            if (!allPossibleMoves.Any())
//            {
//                if (turn == 0)
//                {
//                    var blackMoves = GetAllPossibleMoves(board, 1, true);
//                    if (!blackMoves.Any()) Globals.Stalemate = true;
//                    Globals.CheckmateWhite = true;
//                }
//                else if (turn == 1)
//                {
//                    var whiteMoves = GetAllPossibleMoves(board, 0, true);
//                    if (!whiteMoves.Any()) Globals.Stalemate = true;
//                    Globals.CheckmateBlack = true;
//                }
//            }
//        }
//    }

//    public struct ScoreInDept
//    {
//        public int Dept;
//        public int Score;
//    }
//}

using Engine.Core;
using System.Diagnostics;

namespace Engine
{
    public static class Search
    {
        public static List<MoveObject> tempMoves = new List<MoveObject>();

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            PrincipalVariation pv = new();
            MoveObject bestMove = default;

            var allPossibleMoves = GetAllPossibleMoves(board, turn, true);

            DetectStalemateAndCheckmates(board, turn, bestMove, allPossibleMoves);

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            for (int currentDepth = 1; currentDepth <= maxDepth; currentDepth++)
            {
                foreach (var move in allPossibleMoves)
                {
                    int[] shadowBoard = ApplyMove(board, move);

                    PrincipalVariation currentPv = new PrincipalVariation();

                    int score = (turn == 0) ?
                        AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, move, currentPv) :
                        AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, move, currentPv);

                    MoveHandler.RestoreStateFromSnapshot();

                    if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

                    if ((turn == 0 && score > alpha) || (turn == 1 && score < beta))
                    {
                        if (turn == 0) alpha = score;
                        else beta = score;
                        bestMove = move;
                        bestMove.Score = score;

                        // Save the PV
                        pv.Moves.Clear();
                        pv.AddMove(move);
                        pv.Moves.AddRange(currentPv.Moves);
                    }
                    if (!tempMoves.Contains(bestMove))
                    {
                        tempMoves.Add(bestMove);
                        Console.ResetColor();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Candidate: {Globals.MoveToString(bestMove)} Score:{bestMove.Score} Depth: {currentDepth}");
                    }

                    if (stopwatch.Elapsed >= maxTime)
                    {
                        Console.WriteLine("Stopping search due to time limit.");
                        return bestMove;
                    }
                }
            }
            Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)} ");
            return bestMove;
        }

        private static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval, PrincipalVariation pv)
        {
            if (depth == 0)
            {
                pv.Count = 0;
                return Quiescence(board, alpha, beta, turn);
            }

            int bestScore = int.MinValue;
            PrincipalVariation line = new PrincipalVariation();

            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = AlphaBetaMin(depth - 1, alpha, beta, shadowBoard, 1 - turn, move, line);
                MoveHandler.RestoreStateFromSnapshot();

                if (score > bestScore)
                {
                    bestScore = score;
                    pv.Moves.Clear();
                    pv.AddMove(move);
                    pv.Moves.AddRange(line.Moves);
                }
                alpha = Math.Max(alpha, score);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestScore;
        }

        private static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn, MoveObject moveToEval, PrincipalVariation pv)
        {
            if (depth == 0)
            {
                pv.Count = 0;
                return Quiescence(board, alpha, beta, turn);
            }

            int bestScore = int.MaxValue;
            PrincipalVariation line = new PrincipalVariation();

            foreach (var move in GetAllPossibleMoves(board, turn, true))
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = AlphaBetaMax(depth - 1, alpha, beta, shadowBoard, 1 - turn, move, line);
                MoveHandler.RestoreStateFromSnapshot();

                if (score < bestScore)
                {
                    bestScore = score;
                    pv.Moves.Clear();
                    pv.AddMove(move);
                    pv.Moves.AddRange(line.Moves);
                }
                beta = Math.Min(beta, score);
                if (beta <= alpha)
                {
                    break;
                }
            }
            return bestScore;
        }

        private static int Quiescence(int[] board, int alpha, int beta, int turn)
        {
            int standPat = Evaluators.EvaluatePosition(board, turn);
            if (standPat >= beta)
                return beta;
            if (standPat > alpha)
                alpha = standPat;

            List<MoveObject> captures = GetAllPossibleMoves(board, turn, true)
                .Where(m => m.IsCapture)
                .OrderByDescending(m => m.IsCapture)
                .ToList();

            foreach (var move in captures)
            {
                int[] shadowBoard = ApplyMove(board, move);
                int score = -Quiescence(shadowBoard, -beta, -alpha, turn ^ 1);
                MoveHandler.RestoreStateFromSnapshot();

                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                }
            }

            return alpha;
        }

        private static List<MoveObject> GetAllPossibleMoves(int[] board, int turn, bool filter)
        {
            var moves = MoveGenerator.GenerateAllMoves(board, turn, filter);
            return moves;
        }

        private static int[] ApplyMove(int[] board, MoveObject move)
        {
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.RegisterStaticStates();
            MoveHandler.MakeMove(shadowBoard, move);
            return shadowBoard;
        }

        private static void DetectStalemateAndCheckmates(int[] board, int turn, MoveObject bestMove, List<MoveObject> allPossibleMoves)
        {
            if (!allPossibleMoves.Any())
            {
                if (turn == 0)
                {
                    var blackMoves = GetAllPossibleMoves(board, 1, true);
                    if (!blackMoves.Any()) Globals.Stalemate = true;
                    Globals.CheckmateWhite = true;
                }
                else if (turn == 1)
                {
                    var whiteMoves = GetAllPossibleMoves(board, 0, true);
                    if (!whiteMoves.Any()) Globals.Stalemate = true;
                    Globals.CheckmateBlack = true;
                }
            }
        }
    }

    public struct ScoreInDeptForPosition
    {
        public int turn;
        public string position;
        public int Dept;
        public int Score;
    }
}
