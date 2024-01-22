
namespace Engine;

public struct MoveObject
{
    public int pieceType { get; set; }
    public int CapturedPiece { get; set; }
    public int StartSquare { get; set; }
    public int EndSquare { get; set; }
    public bool IsEnPassant { get; set; } 
    public bool IsPromotion { get; set; } 
    public bool ShortCastle { get; set; }
    public bool LongCastle { get; set; }
}
