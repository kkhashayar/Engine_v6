
using System.Diagnostics.CodeAnalysis;

namespace Engine;

internal static class Evaluators
{
    public static int numbersOfWhitePieces { get; set; }
    public static int numbersOfBlackPieces { get; set; }
    public static int whiteStaticScore { get; set; }
    public static int blackStaticScore { get; set; }

    public static decimal GetByMaterial(int[] board, int turn)
    {

        numbersOfWhitePieces = 0;
        numbersOfBlackPieces = 0;
        whiteStaticScore = 0;
        blackStaticScore = 0;


        decimal score = 0;
        for (int i = 0; i < 64; i++)
        {
            int piece = board[i];
            if (piece == 0) continue;
            if (piece == MoveGenerator.whitePawn)
            {
                whiteStaticScore += 1;
                numbersOfWhitePieces++;
            }
            else if (piece == MoveGenerator.whiteKnight)
            {
                whiteStaticScore += 3;
                numbersOfWhitePieces++;
            }
            else if (piece == MoveGenerator.whiteBishop)
            {
                whiteStaticScore += 4;
                numbersOfWhitePieces++;
            }
            else if (piece == MoveGenerator.whiteRook)
            {
                whiteStaticScore += 5;
                numbersOfWhitePieces++;
            }
            else if (piece == MoveGenerator.whiteQueen)
            {
                whiteStaticScore += 9;
                numbersOfWhitePieces++;
            }
            else if (piece == MoveGenerator.whiteKing)
            {
                whiteStaticScore += 0;
            }
            else if (piece == MoveGenerator.blackPawn)
            {
                blackStaticScore += 1;
                numbersOfBlackPieces++;
            }
            else if (piece == MoveGenerator.blackKnight)
            {
                blackStaticScore += 3;
                numbersOfBlackPieces++;
            }
            else if (piece == MoveGenerator.blackBishop)
            {
                blackStaticScore += 4;
                numbersOfBlackPieces++;
            }
            else if (piece == MoveGenerator.blackRook)
            {
                blackStaticScore += 5;
                numbersOfBlackPieces++;
            }
            else if (piece == MoveGenerator.blackQueen)
            {
                blackStaticScore += 9;
                numbersOfBlackPieces++;
            }
            else if (piece == MoveGenerator.blackKing)
            {
                blackStaticScore += 0;
            }
        }
       if(turn == 0) return whiteStaticScore - blackStaticScore;
       return blackStaticScore - whiteStaticScore;
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

