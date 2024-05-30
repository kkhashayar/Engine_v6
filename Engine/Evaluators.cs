
namespace Engine;

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
                score += 999999 + Tables.Kings.GetWhiteSquareWeight(i);
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
                score -= 999999 - Tables.Kings.GetBlackSquareWeight(i);
            }
        }
        decimal pieceCountFactor = 0.6m; // Adjust this value based on testing
        score += pieceCountFactor * (numbersOfWhitePieces - numbersOfBlackPieces);
        return score;
    }

    // Evaluate the move by considering immediate material changes
    public static decimal EvaluateMoveImpact(int[] chessBoard, MoveObject move)
    {
         int pieceMoving = chessBoard[move.StartSquare];
         int pieceCaptured = chessBoard[move.EndSquare];

        decimal materialGainLoss = 0;

        // Consider the value of captured piece
        if (pieceCaptured != 0)
        {
            materialGainLoss += GetPieceValue(pieceCaptured);
        }

        // Consider the loss if the piece is captured
        materialGainLoss -= GetPieceValue(pieceMoving);

        return materialGainLoss;
    }


    private static decimal GetPieceValue(int piece)
    {

        if (piece == MoveGenerator.whitePawn || piece == MoveGenerator.blackPawn) return 1;
        if (piece == MoveGenerator.whiteKnight || piece == MoveGenerator.blackKnight) return 3;
        if (piece == MoveGenerator.whiteBishop || piece == MoveGenerator.blackBishop) return 3.2m;
        if (piece == MoveGenerator.whiteRook || piece == MoveGenerator.blackRook) return 5;
        if (piece == MoveGenerator.whiteQueen || piece == MoveGenerator.blackQueen) return 9;
        if (piece == MoveGenerator.whiteKing || piece == MoveGenerator.blackKing) return 999999;
        return 0;   
       
    }
}

