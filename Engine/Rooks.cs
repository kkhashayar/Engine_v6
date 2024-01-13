
namespace Engine;

internal static class Rooks
{
    public static List<int>? DefendingSquares { get; set; }
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

        List<MoveObject> moves = new();


        return moves;   
    }



    public static List<int> GetMasksForSquare(int square)
    {
        List<int> squares = new();
        
        int[] directions = { 1, -1, 8, -8 }; // right, left, up, down
        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && !Globals.IsVerHorBreaksMask(currentSquare, direction, originalRank, originalFile))
            {
                squares.Add(currentSquare);
                currentSquare += direction;
            }
        }

        return squares; 
    }
}
