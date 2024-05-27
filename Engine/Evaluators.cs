using System;

namespace Engine
{
    internal static class Evaluators
    {
        public static decimal GetByMaterial(int[] chessBoard)
        {
            int numberOfWhitePieces = 0;
            int numberOfBlackPieces = 0;

            decimal score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += 1 + Tables.Pawns.GetWhiteSquareWeight(i);
                    numberOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += 3 + Tables.Knights.GetWhiteSquareWeight(i);
                    numberOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += 3.2m + Tables.Bishops.GetWhiteSquareWeight(i);
                    numberOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += 5;
                    numberOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += 9;
                    numberOfWhitePieces++;
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += 0.5m + Tables.Kings.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= 1 - Tables.Pawns.GetBlackSquareWeight(i);
                    numberOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= 3 - Tables.Knights.GetBlackSquareWeight(i);
                    numberOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= 3.2m - Tables.Bishops.GetBlackSquareWeight(i);
                    numberOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= 5;
                    numberOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= 9;
                    numberOfBlackPieces++;
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= 0.5m - Tables.Kings.GetBlackSquareWeight(i);
                    numberOfBlackPieces++;
                }
            }
            decimal pieceCountFactor = 0.1m; // Adjust this value based on testing
            score += pieceCountFactor * (numberOfWhitePieces - numberOfBlackPieces);
            return score;
        }
    }
}
