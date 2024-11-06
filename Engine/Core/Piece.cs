namespace Engine.Core;

public static class Piece
{
    public static readonly int Pawn = 1;
    public static readonly int Knight = 3;
    public static readonly int Bishop = 4;
    public static readonly int Rook = 5;
    public static readonly int Queen = 9;
    public static readonly int King = 99;
    public static readonly int None = 0;
    public static readonly int BlackPieceOffset = 10;

    public static bool IsBlack(int pieceValue)
    {
        if (pieceValue == 11 || pieceValue == 13 || pieceValue == 14 || pieceValue == 15 || pieceValue == 19 || pieceValue == 109)
            return true;
        return false;

    }
    public static bool IsWhite(int pieceValue)
    {
        if (pieceValue == 1 || pieceValue == 3 || pieceValue == 4 || pieceValue == 5 || pieceValue == 9 || pieceValue == 99)
            return true;
        return false;
    }

    public static string GetColor(int pieceValue)
    {
        // Black pieces have specific values
        if (pieceValue == 11 || pieceValue == 13 || pieceValue == 14 || pieceValue == 15 || pieceValue == 19 || pieceValue == 109)
            return "Black";

        // White pieces have specific values
        if (pieceValue == 1 || pieceValue == 3 || pieceValue == 4 || pieceValue == 5 || pieceValue == 9 || pieceValue == 99)
            return "White";


        return "None";
    }

    public static int[] WhitePieces = new int[6] { 1, 3, 4, 5, 9, 99 };
    public static int[] BlackPieces = new int[6] { 11, 13, 14, 15, 19, 109 };


    public static string GetPieceName(int pieceValue)
    {
        switch (pieceValue)
        {
            case 1:
                return "P";
            case 3:
                return "N";
            case 4:
                return "B";
            case 5:
                return "R";
            case 9:
                return "Q";
            case 99:
                return "K";
            case 0:
                return " ";
            default:
                if (pieceValue == Pawn + BlackPieceOffset) return "p";
                if (pieceValue == Knight + BlackPieceOffset) return "n";
                if (pieceValue == Bishop + BlackPieceOffset) return "b";
                if (pieceValue == Rook + BlackPieceOffset) return "r";
                if (pieceValue == Queen + BlackPieceOffset) return "q";
                if (pieceValue == King + BlackPieceOffset) return "k";
                return " ";
        }
    }


}

