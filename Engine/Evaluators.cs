using System;

namespace Engine
{
    internal static class Evaluators
    {

        public static int numbersOfWhitePieces { get; set; }
        public static int numbersOfBlackPieces { get; set; }    
        public static decimal GetByMaterial(int[] chessBoard)
        {


            numbersOfWhitePieces = 0;
            numbersOfBlackPieces = 0;

            decimal score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += 1 + Tables.Pawns.GetWhiteSquareWeight(i);
                    numbersOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += 3 + Tables.Knights.GetWhiteSquareWeight(i);
                    numbersOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += 3.2m + Tables.Bishops.GetWhiteSquareWeight(i);
                    numbersOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += 5 + Tables.Rooks.GetWhiteSquareWeight(i);
                    numbersOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += 9;
                    numbersOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += 0.5m + Tables.Kings.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= 1 - Tables.Pawns.GetBlackSquareWeight(i);
                    numbersOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= 3 - Tables.Knights.GetBlackSquareWeight(i);
                    numbersOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= 3.2m - Tables.Bishops.GetBlackSquareWeight(i);
                    numbersOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= 5 - Tables.Rooks.GetBlackSquareWeight(i);
                    numbersOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= 9;
                    numbersOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= 0.5m - Tables.Kings.GetBlackSquareWeight(i);
                    numbersOfBlackPieces++;
                }
            }
            decimal pieceCountFactor = 0.1m; // Adjust this value based on testing
            score += pieceCountFactor * (numbersOfWhitePieces - numbersOfBlackPieces);
            return score;
        }
    }
}
