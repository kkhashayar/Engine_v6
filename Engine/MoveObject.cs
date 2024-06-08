
namespace Engine;

public struct MoveObject
{
    public int pieceType { get; set; }
    public int CapturedPiece { get; set; }
    public int StartSquare { get; set; }
    public int EndSquare { get; set; } 
    public int PromotionPiece { get; set; } 
    public bool ShortCastle { get; set; }
    public bool LongCastle { get; set; }
    public bool IsCapture { get; set; }
    public bool IsEnPassant { get; set; }
    public bool IsPromotion { get; set; }
    public bool IsCheck { get; set; }
    public decimal Priority { get; set; } 
    public int Score { get; set; }
    public int Depth { get; set; }  

    public static MoveObject Empty => new MoveObject
    {
        pieceType = 0,
        CapturedPiece = 0,
        StartSquare = -1,
        EndSquare = -1,
        PromotionPiece = 0,
        ShortCastle = false,
        LongCastle = false,
        IsCapture = false,
        IsEnPassant = false,
        IsPromotion = false,
        IsCheck = false,
        Priority = 0,
        Score = 0
    };
}
