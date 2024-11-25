using Engine.Enums;

namespace Engine.Core;

public struct Result
{
    public List<MoveObject> Moves { get; set; }
    public int WhiteMovesCount { get; set; }
    public int BlackMovesCount { get; set; }
    public GamePhase GamePhase { get; set; }
    public int CalculationTime { get; set; }

}
