namespace Engine.Tables
{
    public static class Knights
    {
        public static readonly int[] KnightTable = new int[64]
        {
            -50, -40, -30, -30, -30, -30, -40, -50,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -30, 0, 10, 15, 15, 10, 0, -30,
            -30, 5, 15, 20, 20, 15, 5, -30,
            -30, 0, 15, 20, 20, 15, 0, -30,
            -30, 5, 10, 15, 15, 10, 5, -30,
            -40, -20, 0, 0, 0, 0, -20, -40,
            -50, -40, -30, -30, -30, -30, -40, -50
        };

        public static int GetSquareWeight(int square, bool isWhite)
        {
            if (!isWhite)
            {
                square = 63 - square; 
            }
            return KnightTable[square];
        }
    }
}

