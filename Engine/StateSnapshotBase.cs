namespace Engine;

public class StateSnapshotBase
{
    public string? Position { get; set; }
    public bool WhiteShortCastle { get; set; } = false;
    public bool WhiteLongCastle { get; set; } = false;
    public bool BlackShortCastle { get; set; } = false;
    public bool BlackLongCastle { get; set; } = false;

    public bool CheckmateWhite { get; set; } = false;
    public bool CheckmateBlack { get; set; } = false;
    public bool CheckWhite { get; set; } = false;
    public bool CheckBlack { get; set; } = false;

    public bool Stalemate { get; set; } = false;

    public bool LastMoveWasPawn { get; set; } = false;
     
    public int LastendSquare { get; set; } = -1;
    public int Turn { get; set; }
}
