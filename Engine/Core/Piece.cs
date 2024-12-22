using System.Reflection.Metadata;

namespace Engine.Core;

public static class Piece
{
    // New piece system   
    public static readonly int P = 1;
    public static readonly int N = 3;
    public static readonly int B = 4;
    public static readonly int R = 5;
    public static readonly int Q = 9;
    public static readonly int K = 1000;

    public static readonly int p = -1;
    public static readonly int n = -3;
    public static readonly int b = -4;
    public static readonly int r = -5;
    public static readonly int q = -9;
    public static readonly int k = -1000;

    public static readonly int None = 0;



    public static bool IsBlack(int pieceValue)
    {
        if (pieceValue == -1 || pieceValue == -3 || pieceValue == -4 || pieceValue == -5 || pieceValue == -9 || pieceValue == -1000)
            return true;
        return false;

    }
    public static bool IsWhite(int pieceValue)
    {
        if (pieceValue == 1 || pieceValue == 3 || pieceValue == 4 || pieceValue == 5 || pieceValue == 9 || pieceValue == 1000)
            return true;
        return false;
    }

    public static string GetColor(int pieceValue)
    {
        if (pieceValue == -1 || pieceValue == -3 || pieceValue == -4 || pieceValue == -5 || pieceValue == -9 || pieceValue == -1000) return "Black";
        if (pieceValue ==  1 || pieceValue ==  3 || pieceValue ==  4 || pieceValue ==  5 || pieceValue ==  9 || pieceValue ==  1000) return "White";
        
        return "None";
    }

    public static int[] WhitePieces = new int[6] { 1, 3, 4, 5, 9, 1000 };
    public static int[] BlackPieces = new int[6] { -1, -3, -4, -5, -9, -1000 };

    public static string GetPieceName(int pieceValue)
    {
        switch (pieceValue)
        {
            case 1:        return "P";
            case 3:        return "N";
            case 4:        return "B";
            case 5:        return "R";
            case 9:        return "Q";
            case 1000:     return "K";
            
            case -1:       return "p";
            case -3:       return "n";
            case -4:       return "b";
            case -5:       return "r";
            case -9:       return "q";
            case -1000:    return "k";
            
            case 0:        return " "; 
            default:       return " ";
        }
    }
}

