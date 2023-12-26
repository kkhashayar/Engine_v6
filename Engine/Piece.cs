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
        return pieceValue >= 11 && pieceValue <= 109; 
    }
    public static bool IsWhite(int pieceValue)
    {
        return pieceValue >= 1 && pieceValue <= 99;
    }

    public static int GetColor(int pieceValue)
    {
        if (pieceValue >= 11 && pieceValue <= 109) return 2;
        else if (pieceValue >= 1 && pieceValue <= 99) return 1;
        return 0;   
    }
}

