namespace Engine.Tables
{
    public static class Knights
    {
        public static readonly int[] KnightTable = new int[64]
        {
            -30, -20, -10, -10, -10, -10, -20, -30,
            -20,  0,   5,   5,   5,   5,   0, -20,
            -10,  5,  10,  15,  15,  10,   5, -10,
            -10,  5,  15,  20,  20,  15,   5, -10,
            -10,  5,  15,  20,  20,  15,   5, -10,
            -10,  5,  10,  15,  15,  10,   5, -10,
            -20,  0,   5,   5,   5,   5,   0, -20,
            -30, -20, -10, -10, -10, -10, -20, -30
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

