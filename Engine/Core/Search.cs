using Engine.Core;
using System.Diagnostics;

namespace Engine;

public static class Search
{

    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
    {
        decimal alpha = -999999;
        decimal beta = 999999;

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        MoveObject bestMove = default;
        List<MoveObject> principalVariation = new();

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
            else if (turn == 1)
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

                List<MoveObject> line = new List<MoveObject>();
                decimal score = (turn == 0) ? AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, ref line) : AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, ref line);
                MoveHandler.RestoreStateFromSnapshot();

                if (turn == 0 && score > alpha)
                {
                    alpha = score;
                    bestMove = move;
                    principalVariation = new List<MoveObject> { move };
                    principalVariation.AddRange(line);
                }
                else if (turn != 0 && score < beta)
                {
                    beta = score;
                    bestMove = move;
                    principalVariation = new List<MoveObject> { move };
                    principalVariation.AddRange(line);
                }
            }

            //Console.WriteLine($"Depth {currentDepth}: Best Move Found - {Globals.MoveToString(bestMove)} with score {(turn == 0 ? alpha : beta)}");
            Console.WriteLine("Principal Variation: " + string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m))));
        }

        //Console.WriteLine($"Best Move: {Globals.MoveToString(bestMove)}");
        Console.WriteLine("Principal Variation: " + string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m))));
        return bestMove;
    }

    public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {

        if (depth == 0)
        {
            pvLine.Clear();
            return Evaluators.GetByMaterial(board, turn);
            //return Quiescence(board, alpha, beta, turn, ref pvLine, 2);
        }
        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return decimal.MinValue;
        }
        allPossibleMoves = allPossibleMoves
            .OrderByDescending(mo => mo.IsCapture)
            .ThenByDescending(mo => mo.IsCheck)
            .ToList();

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;
            int[] shadowBoard = (int[])board.Clone();
            MoveHandler.MakeMove(board, move);

            List<MoveObject> line = new List<MoveObject>();
            decimal score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1, ref line);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            if (score >= beta)
            {
                pvLine = new List<MoveObject> { move };
                pvLine.AddRange(line);
                return beta;
            }
            if (score > alpha)
            {
                alpha = score;
                bestLine = new List<MoveObject> { move };
                bestLine.AddRange(line);
            }
        }

        pvLine = bestLine;
        return alpha;
    }

    public static decimal AlphaBetaMin(int depth, decimal alpha, decimal beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {

        if (depth == 0)
        {
            pvLine.Clear();
            return Evaluators.GetByMaterial(board, turn);
            //return Quiescence(board, alpha, beta, turn, ref pvLine, 2);
        }
        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return decimal.MaxValue;
        }

        allPossibleMoves = allPossibleMoves
            .OrderByDescending(mo => mo.IsCapture)
            .ThenByDescending(mo => mo.IsCheck)
            .ToList();

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            MoveHandler.MakeMove(board, move);

            List<MoveObject> line = new List<MoveObject>();
            decimal score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1, ref line);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            if (score <= alpha)
            {
                pvLine = new List<MoveObject> { move };
                pvLine.AddRange(line);
                return alpha;
            }
            if (score < beta)
            {
                beta = score;
                bestLine = new List<MoveObject> { move };
                bestLine.AddRange(line);
            }
        }

        pvLine = bestLine;
        return beta;
    }

    public static decimal Quiescence(int[] board, decimal alpha, decimal beta, int turn, ref List<MoveObject> pvLine, int depth)
    {
        decimal standPat = Evaluators.GetByMaterial(board, turn);

        if (depth == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        if (turn == 0)
        {
            if (standPat >= beta)
            {
                return beta;
            }
            if (standPat > alpha)
            {
                alpha = standPat;
            }
        }
        else
        {
            if (standPat <= alpha)
            {
                return alpha;
            }
            if (standPat < beta)
            {
                beta = standPat;
            }
        }

        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true)
            .Where(mo => mo.IsCapture || mo.IsCheck)
            .OrderByDescending(mo => mo.IsCheck)
            .ThenByDescending(mo => mo.IsCapture).ToList();    
        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        List<MoveObject> bestLine = new List<MoveObject>();
        bool isPVNode = false;

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;
            MoveHandler.MakeMove(board, move);

            List<MoveObject> line = new List<MoveObject>();
            decimal score = Quiescence(board, alpha, beta, turn ^ 1, ref line, depth - 1);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            if (turn == 0)
            {
                if (score >= beta)
                {
                    return beta;
                }
                if (score > alpha)
                {
                    alpha = score;
                    isPVNode = true;
                    bestLine = new List<MoveObject> { move };
                    bestLine.AddRange(line);
                }
            }
            else
            {
                if (score <= alpha)
                {
                    return alpha;
                }
                if (score < beta)
                {
                    beta = score;
                    isPVNode = true;
                    bestLine = new List<MoveObject> { move };
                    bestLine.AddRange(line);
                }
            }
        }

        if (isPVNode)
        {
            pvLine = bestLine;
        }
        else
        {
            pvLine.Clear();
        }

        return (turn == 0) ? alpha : beta;
    }
}
