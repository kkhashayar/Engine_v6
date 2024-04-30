namespace Engine;

public static class StateSnapshotBase
{
    public static string? Position { get; set; }
    public static bool WhiteShortCastle { get; set; } = false;
    public static bool WhiteLongCastle { get; set; } = false;
    public static bool WhiteKingRookMoved { get; set; } = false;
    public static bool WhiteQueenRookMoved { get; set; } = false;

    public static bool BlackShortCastle { get; set; } = false;
    public static bool BlackLongCastle { get; set; } = false;
    public static bool BlackKingRookMoved { get; set; } = false;    
    public static bool BlackQueenRookMoved { get; set; } = false; 
    public static bool CheckmateWhite { get; set; } = false;
    public static bool CheckmateBlack { get; set; } = false;
    public static bool CheckWhite { get; set; } = false;
    public static bool CheckBlack { get; set; } = false;

    public static bool Stalemate { get; set; } = false;

    public static bool LastMoveWasPawn { get; set; } = false;
     
    public static int LastEndSquare { get; set; } = -1;
    public static int Turn { get; set; }
}
