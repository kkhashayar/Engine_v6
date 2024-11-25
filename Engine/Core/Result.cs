namespace Engine.Core;

public struct Result
{
    public List<MoveObject> LegalMoves { get; set; }
    public List<MoveObject> PseudoWhiteMoves { get; set; }
    public List<MoveObject> PseudoBlackMoves { get; set; }
    public List<MoveObject> PseudoMoves { get; set; }
    public int WhiteMovesCount { get; set; }
    public int BlackMovesCount { get; set; }
    public int TotalMovesCount { get; set; }
}
