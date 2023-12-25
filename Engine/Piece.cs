namespace Engine;

public class Piece
{
    public int Pawn     = 1;
    public int Knight   = 3;
    public int Bishop   = 4;
    public int Rook     = 5;
    public int Queen    = 9;
    public int King     = 99;
    public int None     = 0;
    public int BlackPieceOffset = 10;

    public static bool IsBlack(int pieceValue)
    {
        return pieceValue >= 11 && pieceValue <= 109; 
    }
    public static bool IsWhite(int pieceValue)
    {
        return pieceValue >= 1 && pieceValue <= 99;
    }

}

