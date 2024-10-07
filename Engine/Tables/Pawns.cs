namespace Engine.Tables
{
    public static class Pawns
    {
        public static readonly int[] PawnTable = new int[64]
        {
            0,  0,  0,  0,  0,  0,  0,  0,
            50, 50, 50, 50, 50, 50, 50, 50,
            10, 10, 20, 30, 30, 20, 10, 10,
            5,  5,  10, 27, 27, 10, 5, 5,
            0,  0,  0,  25, 25, 0, 0, 0,
            5, -5, -10, 0, 0, -10, -5, 5,
            5, 10, 10, -25, -25, 10, 10, 5,
            0,  0,  0,  0,  0,  0,  0,  0
        };

        public static int GetSquareWeight(int square, bool isWhite)
        {
            if (!isWhite)
            {
                square = 63 - square; 
            }
            return PawnTable[square];
        }
    }
}
