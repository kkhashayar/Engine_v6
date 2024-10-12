using Engine.Core;
using System.Diagnostics;

namespace Engine
{
    public static class Search
    {

        public static MoveObject GetBestMove(int[] board, int turn, int maxDepth, TimeSpan maxTime)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int alpha = int.MinValue;
            int beta = int.MaxValue;

            MoveObject bestMove = default;
            List<MoveObject> allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);

            if (allPossibleMoves.Count == 1) return allPossibleMoves[0];

            if (!allPossibleMoves.Any())
            {
                if (turn == 0) Globals.CheckmateWhite = true;
                else Globals.CheckmateBlack = true;
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

                    int score = (turn == 0) ?
                        AlphaBetaMin(currentDepth - 1, alpha, beta, shadowBoard, 1) :
                        AlphaBetaMax(currentDepth - 1, alpha, beta, shadowBoard, 0);
                    MoveHandler.RestoreStateFromSnapshot();

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

                Console.WriteLine($"Depth {currentDepth}: Best Move Found - {MoveToString(bestMove)} with score {(turn == 0 ? alpha : beta)}");
                Console.WriteLine();
            }


            Console.WriteLine($"Best Move: {MoveToString(bestMove)}");
            Console.WriteLine();
            return bestMove;
        }
        public static int AlphaBetaMax(int depth, int alpha, int beta, int[] board, int turn)
        {
            List<MoveObject> allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return int.MinValue;

            var whiteAttacksNumber = MoveGenerator.GetTotalNumberOfAttackSquares(board, 0);
            var blackAttackNumber = MoveGenerator.GetTotalNumberOfAttackSquares(board, 1);
            if (depth == 0) return Evaluators.GetByMaterial(board, whiteAttacksNumber, blackAttackNumber);

            foreach (var move in allPossibleMoves)
            {
                MoveHandler.RegisterStaticStates();
                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

                MoveHandler.MakeMove(board, move);

                int score = AlphaBetaMin(depth - 1, alpha, beta, board, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if (score >= beta) return beta;
                if (score > alpha) alpha = score;
            }

            return alpha;
        }

        public static int AlphaBetaMin(int depth, int alpha, int beta, int[] board, int turn)
        {
            List<MoveObject> allPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn, true);
            if (allPossibleMoves == null || allPossibleMoves.Count == 0) return int.MaxValue;

            var whiteAttacksNumber = MoveGenerator.GetTotalNumberOfAttackSquares(board, 0);
            var blackAttackNumber = MoveGenerator.GetTotalNumberOfAttackSquares(board, 1);


            if (depth == 0) return Evaluators.GetByMaterial(board, whiteAttacksNumber, blackAttackNumber);

            foreach (var move in allPossibleMoves)
            {
                MoveHandler.RegisterStaticStates();

                var pieceMoving = move.pieceType;
                var targetSquare = board[move.EndSquare];
                var promotedTo = move.PromotionPiece;

                MoveHandler.MakeMove(board, move);

                int score = AlphaBetaMax(depth - 1, alpha, beta, board, turn ^ 1);

                MoveHandler.RestoreStateFromSnapshot();
                MoveHandler.UndoMove(board, move, pieceMoving, targetSquare, promotedTo);

                if (score <= alpha) return alpha;
                if (score < beta) beta = score;
            }

            return beta;
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