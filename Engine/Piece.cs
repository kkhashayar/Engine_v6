namespace Engine;

public static class Piece
{
    public static readonly int Pawn        = 1;
    public static readonly int Knight      = 3;
    public static readonly int Bishop      = 4;
    public static readonly int Rook        = 5;
    public static readonly int Queen       = 9;
    public static readonly int King        = 99;
    public static readonly int None        = 0;
    public static readonly int BlackPieceOffset = 10;

    public static bool IsBlack(int pieceValue)
    {
        if(pieceValue == 11 || pieceValue == 13 || pieceValue == 14 || pieceValue == 15 || pieceValue == 19 || pieceValue == 109)
            return true;
        return false;
         
    }
    public static bool IsWhite(int pieceValue)
    {
        if (pieceValue == 1 || pieceValue == 3 || pieceValue == 4 || pieceValue == 5 || pieceValue == 9 || pieceValue == 1)
            return true;
        return false;
    }
}

