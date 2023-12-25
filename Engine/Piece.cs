namespace Engine;

public class Piece
{
    public int Pawn { get; set; } = 1;
    public int Knight { get; set; } = 3;
    public int Bishop { get; set; } = 4;
    public int Rook { get; set; } = 5;
    public int Queen { get; set; } = 9;
    public int King { get; set; } = 99;
    public int None { get; set; } = 0;
    public int BlackPieceOffset { get; set; } = 10;

}

