namespace Engine.Tables
{
    public static class Knights
    {
        public static readonly int[] KnightTable = new int[64]
         {
            -15, -10, -5,  -5,  -5,  -5, -10, -15,
            -10,  -5,  0,   2,   2,   0,  -5, -10,
             -5,   0,  3,   5,   5,   3,   0,  -5,
             -5,   2,  5,   7,   7,   5,   2,  -5,
             -5,   2,  5,   7,   7,   5,   2,  -5,
             -5,   0,  3,   5,   5,   3,   0,  -5,
            -10,  -5,  0,   2,   2,   0,  -5, -10,
            -15, -10, -5,  -5,  -5,  -5, -10, -15
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

