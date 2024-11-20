using Engine.Core;
using System.Diagnostics;
namespace Engine;

public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxtime)
    {
        decimal alpha = -999999;
        decimal beta = 999999;
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        MoveObject bestMove = default;
        List<MoveObject> principalVariation = new();

        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);

        if (allPossibleMoves.Count == 1) return allPossibleMoves[0]; // Only one legal move
        if (allPossibleMoves.Count == 0)
        {
            // Handle checkmate or stalemate
            if (turn == 0) Globals.CheckmateWhite = true;
            else if (turn == 1) Globals.CheckmateBlack = true;
            else Globals.Stalemate = true;
            return bestMove;
        }

        // Iterative deepening with timing control - New Changes Start Here
        TimeSpan allocatedTime = maxtime;
        for (int currentDepth = 2; currentDepth <= maxDepth; currentDepth += 2)
        {
            if (stopwatch.Elapsed >= allocatedTime)
            {
                break; // Stop if we've run out of time
            }

            alpha -=1;
            beta += 1;
            MoveObject currentBestMove = default;
            List<MoveObject> currentPV = new();

            foreach (var move in allPossibleMoves)
            {
                if (stopwatch.Elapsed >= allocatedTime)
                {
                    return bestMove;
                }

                int[] shadowBoard = (int[])board.Clone();
                MoveHandler.RegisterStaticStates();
                MoveHandler.MakeMove(shadowBoard, move);

                List<MoveObject> line = new();
                decimal score = (turn == 0)
                    ? AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1, ref line)
                    : AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0, ref line);

                MoveHandler.RestoreStateFromSnapshot();

                if (turn == 0 && score > alpha) // White maximizes
                {
                    alpha = score;
                    currentBestMove = move;
                    currentPV = new List<MoveObject> { move };
                    currentPV.AddRange(line);
                }
                else if (turn == 1 && score < beta) // Black minimizes
                {
                    beta = score;
                    currentBestMove = move;
                    currentPV = new List<MoveObject> { move };
                    currentPV.AddRange(line);
                }
            }

            if (currentBestMove != default)
            {
                bestMove = currentBestMove; // Update the best move
                principalVariation = currentPV; // Update the PV
            }

            // Reduce allocated time as we go deeper to avoid time overrun
            allocatedTime = maxtime - stopwatch.Elapsed;

            Console.WriteLine($"Depth {currentDepth / 2} score {(turn == 0 ? alpha : beta)}");
            Console.WriteLine("PV: " + string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m))));
        }
        // New Changes End Here

        return bestMove;
    }

    public static decimal AlphaBetaMax(int depth, decimal alpha, decimal beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {
        if (depth == 0)
        {
            pvLine.Clear();
            return Evaluators.GetByMaterial(board, turn);
        }
        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return decimal.MinValue;
        }

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            MoveHandler.MakeMove(board, move);

            // Clone the board before passing it to the recursive call
            int[] boardCopy = (int[])board.Clone();

            List<MoveObject> line = new List<MoveObject>();
            decimal score = AlphaBetaMin(depth - 1, alpha, beta, boardCopy, turn ^ 1, ref line);

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
        }
        var allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (allPossibleMoves == null || allPossibleMoves.Count == 0)
        {
            pvLine.Clear();
            return decimal.MaxValue;
        }

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in allPossibleMoves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            MoveHandler.MakeMove(board, move);

            // Clone the board before passing it to the recursive call
            int[] boardCopy = (int[])board.Clone();

            List<MoveObject> line = new List<MoveObject>();
            decimal score = AlphaBetaMax(depth - 1, alpha, beta, boardCopy, turn ^ 1, ref line);

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

