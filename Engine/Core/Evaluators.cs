
namespace Engine
{
    internal static class Evaluators
    {
        public static int GetByMaterial(int[] chessBoard, int numberOfWhiteMoves, int numberOfBlackMoves)
        {
            int score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += 10; // - Tables.Pawns.WhitePawnTable[i];
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
                    score += 5;
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= 10; // - Tables.Pawns.GetBlackSquareWeight(i);
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
                    score -= 5;
                }
            }
            int mobilityScore = (numberOfWhiteMoves - numberOfBlackMoves) * 1;
            score += mobilityScore;
            return score;
        }
    }
}