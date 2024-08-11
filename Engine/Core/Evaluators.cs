//namespace Engine;
//public static class Evaluators
//{
//    private static readonly int PawnValue = 10;
//    private static readonly int KnightValue = 320;
//    private static readonly int BishopValue = 325;
//    private static readonly int RookValue = 500;
//    private static readonly int QueenValue = 975;
//    private static readonly int KingValue = 32767;

//    public static int EvaluatePosition(int[] board, int turn)
//    {
//        int score = 0;

//        for (int i = 0; i < board.Length; ++i)
//        {
//            int piece = board[i];
//            if (piece == MoveGenerator.whitePawn)
//            {                
//                score += PawnValue;
//                if (turn == 0) score += Tables.Pawns.GetSquareWeight(i, true);
//            }
//            else if (piece == MoveGenerator.blackPawn)
//            {  
//                score += PawnValue;
//                if (turn == 1) score -= Tables.Pawns.GetSquareWeight(i, false);
//            }
//            else if (piece == MoveGenerator.whiteKnight)
//            {  
//                score += KnightValue;
//                if (turn == 0) score += Tables.Knights.GetSquareWeight(i, true);
//            }
//            else if (piece == MoveGenerator.blackKnight)
//            {
//                score += KnightValue;
//                if (turn == 1) score -= Tables.Knights.GetSquareWeight(i, false);
//            }
//            else if (piece == MoveGenerator.whiteBishop)
//            {
//                score += BishopValue;
//                if (turn == 0) score += Tables.Bishops.GetSquareWeight(i, true);
//            }
//            else if (piece == MoveGenerator.blackBishop)
//            {
//                score += BishopValue;
//                if (turn == 1) score -= Tables.Bishops.GetSquareWeight(i, false);
//            }
//            else if (piece == MoveGenerator.whiteRook)
//            {   
//                score += RookValue;
//            }
//            else if (piece == MoveGenerator.blackRook)
//            {
//                score += RookValue;
//            }
//            else if (piece == MoveGenerator.whiteQueen)
//            {
//                score += QueenValue;
//            }
//            else if (piece == MoveGenerator.blackQueen)
//            {   
//                score += QueenValue;
//            }
//            else if (piece == MoveGenerator.whiteKing)
//            {
//                score += KingValue;
//                if (turn == 0) score += Tables.Kings.GetSquareWeight(board, i, true);
//            }
//            else if (piece == MoveGenerator.blackKing)
//            {
//                score += KingValue;
//                if (turn == 1) score -= Tables.Kings.GetSquareWeight(board, i, false);
//            }
//        }
//        if(turn == 1) return -score;
//        return score;
//    }
//}

using Engine.Core;
using Engine.Enums;

namespace Engine
{
    public static class Evaluators
    {
        private static readonly int PawnValue = 100;
        private static readonly int KnightValue = 320;
        private static readonly int BishopValue = 330;
        private static readonly int RookValue = 500;
        private static readonly int QueenValue = 900;

        // King value is not static; it changes based on the game's phase
        private static int KingValue => Globals.GetGamePhase() == GamePhase.EndGame ? 20000 : 10000;

        public static int EvaluatePosition(int[] board, int turn)
        {
            int score = 0;

            for (int i = 0; i < board.Length; ++i)
            {
                int piece = board[i];
                if (piece == MoveGenerator.whitePawn)
                {
                    score += PawnValue + Tables.Pawns.GetSquareWeight(i, true);
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= PawnValue + Tables.Pawns.GetSquareWeight(i, false);
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += KnightValue + Tables.Knights.GetSquareWeight(i, true);
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= KnightValue + Tables.Knights.GetSquareWeight(i, false);
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += BishopValue + Tables.Bishops.GetSquareWeight(i, true);
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= BishopValue + Tables.Bishops.GetSquareWeight(i, false);
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += RookValue;
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= RookValue;
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += QueenValue;
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= QueenValue;
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += KingValue + Tables.Kings.GetSquareWeight(board, i, true);
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= KingValue + Tables.Kings.GetSquareWeight(board, i, false);
                }
            }

            // Adjust score based on the turn
            return turn == 0 ? score : -score;
        }
    }
}