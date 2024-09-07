using System;

namespace Engine
{
    internal static class Evaluators
    {
        public static int GetByMaterial(int[] chessBoard)
        {
            int score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += 10; 
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += 30;
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += 32;
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += 50;
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += 90;
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += 1000;
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= 10; 
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= 30;
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= 32;
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= 50;
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= 90;
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= 1000;
                }
            }
            
            return score;
        }
    }
}