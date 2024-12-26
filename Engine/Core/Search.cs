//using Engine.Core;
//using System.Diagnostics;
//using Engine.Enums;
//namespace Engine;

//public static class Search
//{
//    public static MoveObject GetBestMove(int[] board, int turn)
//    {
//        MoveObject bestMove = default;
//        List<MoveObject> principalVariation = new();

//        Stopwatch stopwatch = new Stopwatch();
//        stopwatch.Start();

//        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
//        TimeSpan maxtime = TimeSpan.FromSeconds(moveGenResult.CalculationTime);

//        Console.WriteLine(moveGenResult.GamePhase);

//        if (moveGenResult.Moves.Count == 1) return moveGenResult.Moves[0]; // Only one legal move
//        if (moveGenResult.Moves.Count == 0)
//        {
//            // Handle checkmate or stalemate
//            if (turn == 0) Globals.CheckmateWhite = true;
//            else if (turn == 1) Globals.CheckmateBlack = true;
//            else Globals.Stalemate = true;
//            return bestMove;
//        }

//        // Iterative deepening with timing control
//        for (int currentDepth = 2; currentDepth <= 20; currentDepth += 2)
//        {
//            int alpha = -999999;
//            int beta = 999999;
//            if (stopwatch.Elapsed >= maxtime)
//            {
//                break; // Stop if we've run out of time
//            }


//            MoveObject currentBestMove = default;
//            List<MoveObject> currentPV = new();

//            foreach (var move in moveGenResult.Moves)
//            {
//                if (stopwatch.Elapsed >= maxtime)
//                {
//                    return bestMove;
//                }

//                MoveHandler.RegisterStaticStates();

//                // Save necessary information to undo the move
//                var pieceMoving = move.pieceType;
//                var targetSquare = board[move.EndSquare];
//                var promotedTo = move.PromotionPiece;


//                MoveHandler.MakeMove(board, move, turn);

//                List<MoveObject> line = new();
//                int score = -Negamax(currentDepth - 1, -beta, -alpha, board, turn ^ 1, ref line);

//                // Restore any static state
//                MoveHandler.RestoreStateFromSnapshot();

//                // Undo the move using your method
//                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

//                if (score >= 999990 || score <= -999990)
//                {
//                    currentBestMove = move;
//                    principalVariation = new List<MoveObject> { move };
//                    principalVariation.AddRange(line);
//                    return currentBestMove;
//                }

//                if (score > alpha)
//                {
//                    alpha = score;
//                    currentBestMove = move;
//                    currentPV = new List<MoveObject> { move };
//                    currentPV.AddRange(line);
//                }
//            }

//            if (currentBestMove != default)
//            {
//                bestMove = currentBestMove; // Update the best move
//                principalVariation = currentPV; // Update the PV
//            }

//            Console.Beep(400,200);
//            Console.WriteLine($"Depth:{currentDepth} score:{alpha} Time:{stopwatch.Elapsed.TotalSeconds} PV:{string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m)))}");


//        }

//        return bestMove;
//    }

//    public static int Negamax(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine)
//    {
//        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
//        if (depth == 0)
//        {
//            pvLine.Clear();
//            if (Globals.QuQuiescenceSwitch is true) return Quiescence(board, alpha, beta, turn, ref pvLine, 2);
//            return Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);
//        }

//        if (moveGenResult.Moves == null || moveGenResult.Moves.Count == 0)
//        {
//            pvLine.Clear();
//            return -999999 + depth;
//        }

//        List<MoveObject> bestLine = new List<MoveObject>();
//        MoveObject bestMove = default;
//        int originalAlpha = alpha;

//        foreach (var move in moveGenResult.Moves)
//        {
//            MoveHandler.RegisterStaticStates();

//            var pieceMoving = move.pieceType;
//            var targetSquare = board[move.EndSquare];
//            var promotedTo = move.PromotionPiece;

//            // Make the move
//            MoveHandler.MakeMove(board, move, turn);
//            List<MoveObject> line = new List<MoveObject>();

//            int score = -Negamax(depth - 1, -beta, -alpha, board, turn ^ 1, ref line);

//            MoveHandler.RestoreStateFromSnapshot();
//            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

//            if (score >= beta)
//            {
//                pvLine = new List<MoveObject> { move };
//                pvLine.AddRange(line);
//                return beta;
//            }
//            if (score > alpha)
//            {
//                alpha = score;
//                bestMove = move;
//                bestLine = new List<MoveObject> { move };
//                bestLine.AddRange(line);
//            }
//        }
//        pvLine = bestLine;
//        return alpha;
//    }

//    public static int Quiescence(int[] board, int alpha, int beta, int turn, ref List<MoveObject> pvLine, int depth)
//    {

//        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
//        int standPat = Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);

//        var captures = moveGenResult.Moves.Where(m => m.IsCapture).ToList();

//        if (depth == 0)
//        {
//            pvLine.Clear();
//            return standPat;
//        }

//        if (standPat >= beta) return beta;
//        if (standPat > alpha) alpha = standPat;

//        if (captures == null || captures.Count == 0)
//        {
//            pvLine.Clear();
//            return standPat;
//        }

//        List<MoveObject> bestLine = new List<MoveObject>();

//        foreach (var move in captures)
//        {


//            MoveHandler.RegisterStaticStates();

//            var pieceMoving = move.pieceType;
//            var targetSquare = board[move.EndSquare];
//            var promotedTo = move.PromotionPiece;

//            if (moveGenResult.GamePhase == GamePhase.Opening)
//            {

//                if (move.pieceType > targetSquare) continue;
//            }
//            MoveHandler.MakeMove(board, move, turn);

//            List<MoveObject> line = new List<MoveObject>();
//            int score = -Quiescence(board, -beta, -alpha, turn ^ 1, ref line, depth - 1);

//            MoveHandler.RestoreStateFromSnapshot();
//            MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

//            if (score >= beta)
//            {
//                return beta;
//            }
//            if (score > alpha)
//            {
//                alpha = score;
//                bestLine = new List<MoveObject> { move };
//                bestLine.AddRange(line);
//            }
//        }

//        pvLine = bestLine;
//        return alpha;
//    }
//}


using Engine.Core;
using System.Diagnostics;
using Engine.Enums;

namespace Engine;

public static class Search
{
    public static MoveObject GetBestMove(int[] board, int turn)
    {
        MoveObject bestMove = default;
        List<MoveObject> principalVariation = new();

        Stopwatch stopwatch = Stopwatch.StartNew();

        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
        TimeSpan maxTime = TimeSpan.FromSeconds(moveGenResult.CalculationTime);

        Console.WriteLine(moveGenResult.GamePhase);

        // Handle trivial cases (single legal move or no moves)
        if (moveGenResult.Moves.Count == 1)
            return moveGenResult.Moves[0];

        if (moveGenResult.Moves.Count == 0)
        {
            HandleGameEnd(turn);
            return bestMove;
        }

        for (int currentDepth = 2; currentDepth <= 20; currentDepth += 2)
        {
            if (stopwatch.Elapsed >= maxTime)
                break;

            int alpha = -999999;
            int beta = 999999;

            MoveObject currentBestMove = default;
            List<MoveObject> currentPV = new();

            foreach (var move in moveGenResult.Moves)
            {
                if (stopwatch.Elapsed >= maxTime)
                    return bestMove;

                ExecuteMove(board, move, turn, out var undoState);

                List<MoveObject> line = new();
                int score = -Negamax(currentDepth - 1, -beta, -alpha, board, turn ^ 1, ref line);

                UndoMove(board, move, undoState);

                if (UpdateBestMove(ref alpha, ref currentBestMove, ref currentPV, move, line, score))
                    break;
            }

            if (currentBestMove != default)
            {
                bestMove = currentBestMove;
                principalVariation = currentPV;
            }

            Console.Beep(400, 200);
            Console.WriteLine($"Depth:{currentDepth} score:{alpha} Time:{stopwatch.Elapsed.TotalSeconds} PV:{string.Join(" ", principalVariation.Select(m => Globals.MoveToString(m)))}");
        }

        return bestMove;
    }

    private static void HandleGameEnd(int turn)
    {
        if (turn == 0)
            Globals.CheckmateWhite = true;
        else if (turn == 1)
            Globals.CheckmateBlack = true;
        else
            Globals.Stalemate = true;
    }

    private static bool UpdateBestMove(ref int alpha, ref MoveObject currentBestMove, ref List<MoveObject> currentPV, MoveObject move, List<MoveObject> line, int score)
    {
        if (score >= 999990 || score <= -999990)
        {
            currentBestMove = move;
            currentPV = new List<MoveObject> { move };
            currentPV.AddRange(line);
            return true; // Early exit for decisive moves
        }

        if (score > alpha)
        {
            alpha = score;
            currentBestMove = move;
            currentPV = new List<MoveObject> { move };
            currentPV.AddRange(line);
        }

        return false;
    }

    private static void ExecuteMove(int[] board, MoveObject move, int turn, out (int piece, int targetSquare, int promotionPiece) undoState)
    {
        MoveHandler.RegisterStaticStates();
        undoState = (move.pieceType, board[move.EndSquare], move.PromotionPiece);
        MoveHandler.MakeMove(board, move, turn);
    }

    private static void UndoMove(int[] board, MoveObject move, (int piece, int targetSquare, int promotionPiece) undoState)
    {
        MoveHandler.RestoreStateFromSnapshot();
        MoveHandler.UndoMove(board, move, undoState.piece, undoState.targetSquare, undoState.promotionPiece);
    }

    public static int Negamax(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine)
    {
        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);

        if (depth == 0)
            return PerformEvaluation(board, turn, ref pvLine, alpha, beta, moveGenResult);

        if (moveGenResult.Moves.Count == 0)
        {
            pvLine.Clear();
            return -999999 + depth;
        }

        return SearchMoves(depth, alpha, beta, board, turn, ref pvLine, moveGenResult);
    }

    private static int PerformEvaluation(int[] board, int turn, ref List<MoveObject> pvLine, int alpha, int beta, Result moveGenResult)
    {
        pvLine.Clear();
        if (Globals.QuQuiescenceSwitch)
            return Quiescence(board, alpha, beta, turn, ref pvLine, 2);

        return Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);
    }

    private static int SearchMoves(int depth, int alpha, int beta, int[] board, int turn, ref List<MoveObject> pvLine, Result moveGenResult)
    {
        List<MoveObject> bestLine = new();

        foreach (var move in moveGenResult.Moves)
        {
            ExecuteMove(board, move, turn, out var undoState);

            List<MoveObject> line = new();
            int score = -Negamax(depth - 1, -beta, -alpha, board, turn ^ 1, ref line);

            UndoMove(board, move, undoState);

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

    public static int Quiescence(int[] board, int alpha, int beta, int turn, ref List<MoveObject> pvLine, int depth)
    {
        var moveGenResult = MoveGenerator.GenerateAllMoves(board, turn, true);
        int standPat = Evaluators.GetByMaterial(board, turn, moveGenResult.WhiteMovesCount, moveGenResult.BlackMovesCount, moveGenResult.GamePhase);

        if (depth == 0 || moveGenResult.Moves.All(m => !m.IsCapture))
        {
            pvLine.Clear();
            return standPat;
        }

        if (standPat >= beta)
            return beta;

        if (standPat > alpha)
            alpha = standPat;

        return EvaluateCaptures(board, alpha, beta, turn, ref pvLine, depth, moveGenResult);
    }

    private static int EvaluateCaptures(int[] board, int alpha, int beta, int turn, ref List<MoveObject> pvLine, int depth, Result moveGenResult)
    {
        List<MoveObject> bestLine = new();

        foreach (var move in moveGenResult.Moves.Where(m => m.IsCapture))
        {
            ExecuteMove(board, move, turn, out var undoState);

            List<MoveObject> line = new();
            int score = -Quiescence(board, -beta, -alpha, turn ^ 1, ref line, depth - 1);

            UndoMove(board, move, undoState);

            if (score >= beta)
                return beta;

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
