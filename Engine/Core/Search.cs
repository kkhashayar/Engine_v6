using Engine.Core;
using System.Diagnostics;
namespace Engine;

public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn)
    {
        MoveObject bestMove = default;
        List<MoveObject> principalVariation = new();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
        TimeSpan maxtime = TimeSpan.FromSeconds(moveGenResult.CalculationTime);

        Console.WriteLine(moveGenResult.GamePhase);

        if (moveGenResult.Moves.Count == 1) return moveGenResult.Moves[0]; // Only one legal move
        if (moveGenResult.Moves.Count == 0)
        {
            // Handle checkmate or stalemate
            if (turn == 0) Globals.CheckmateWhite = true;
            else if (turn == 1) Globals.CheckmateBlack = true;
            else Globals.Stalemate = true;
            return bestMove;
        }

        // Iterative deepening with timing control
        for (int currentDepth = 2; currentDepth <= 20; currentDepth += 2)
        {
            if (stopwatch.Elapsed >= maxtime)
            {
                break; // Stop if we've run out of time
            }

            int alpha = -999999;
            int beta = 999999;
            MoveObject currentBestMove = default;
            List<MoveObject> currentPV = new();

            foreach (var move in moveGenResult.Moves)
            {
                if (stopwatch.Elapsed >= maxtime)
                {
                    return bestMove;
                }

                MoveHandler.RegisterStaticStates();

                // Save necessary information to undo the move
                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

            
                MoveHandler.MakeMove(board, move, turn);

                List<MoveObject> line = new();
                int score = -Negamax(currentDepth - 1, -beta, -alpha, board, turn ^ 1, ref line);

                // Restore any static state
                MoveHandler.RestoreStateFromSnapshot();

                // Undo the move using your method
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if (score >= 999990 || score <= -999990)
                {
                    currentBestMove = move;
                    principalVariation = new List<MoveObject> { move };
                    principalVariation.AddRange(line);
                    return currentBestMove;
                }

                if (score > alpha)
                {
                    alpha = score;
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

            Console.WriteLine($"Depth:{currentDepth} score:{alpha} Time:{stopwatch.Elapsed.TotalSeconds} PV:{string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m)))}");


        }

        return bestMove;
    }

    public static int Negamax(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {
        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
        if (depth == 0)
        {
            pvLine.Clear();
            if (Globals.QuQuiescenceSwitch is true) return Quiescence(board, alpha, beta, turn, ref pvLine, 2);
            return Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);
        }

        if (moveGenResult.Moves == null || moveGenResult.Moves.Count == 0)
        {
            pvLine.Clear();
            return -999999 + depth;
        }

        List<MoveObject> bestLine = new List<MoveObject>();
        MoveObject bestMove = default;
        int originalAlpha = alpha;

        foreach (var move in moveGenResult.Moves)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;

            // Make the move
            MoveHandler.MakeMove(board, move, turn);
            List<MoveObject> line = new List<MoveObject>();

            int score = -Negamax(depth - 1, -beta, -alpha, board, turn ^ 1, ref line);
            
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
                bestMove = move;
                bestLine = new List<MoveObject> { move };
                bestLine.AddRange(line);
            }
        }
        pvLine = bestLine;
        return alpha;
    }

    public static int Quiescence(int[] board, int alpha, int beta, int turn, ref List<MoveObject> pvLine, int depth)
    {
        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
        int standPat = Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);

        if (depth == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        if (standPat >= beta)
        {
            return beta;
        }
        if (standPat > alpha)
        {
            alpha = standPat;
        }


        var captures = moveGenResult.Moves.Where(m => m.IsCapture).ToList();

        if (captures == null || captures.Count == 0)
        {
            pvLine.Clear();
            return standPat;
        }

        List<MoveObject> bestLine = new List<MoveObject>();

        foreach (var move in captures)
        {
            MoveHandler.RegisterStaticStates();

            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            var promotedTo = move.PromotionPiece;
            MoveHandler.MakeMove(board, move, turn);

            List<MoveObject> line = new List<MoveObject>();
            int score = -Quiescence(board, -beta, -alpha, turn ^ 1, ref line, depth - 1);

            MoveHandler.RestoreStateFromSnapshot();
            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

            if (score >= beta)
            {
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
}

