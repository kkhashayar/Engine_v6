﻿
namespace Engine;

public class MoveObject
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
    public int Score { get; set; }
    public int Depth { get; set; }  
}
