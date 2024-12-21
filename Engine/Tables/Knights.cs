namespace Engine.Tables
{
    public static class Knights
    {
        public static readonly int[] KnightTable = new int[64]
        {
            -10,  -8,  -6,  -6,  -6,  -6,  -8, -10, // A8 to H8
             -8,  -6,  -4,  -2,  -2,  -4,  -6,  -8, // A7 to H7
             -6,  -4,  -1,   1,   1,  -1,  -4,  -6, // A6 to H6
             -6,  -2,   1,   3,   3,   1,  -2,  -6, // A5 to H5
             -6,  -2,   1,   3,   3,   1,  -2,  -6, // A4 to H4
             -6,  -4,  -1,   1,   1,  -1,  -4,  -6, // A3 to H3
             -8,  -6,  -4,  -2,  -2,  -4,  -6,  -8, // A2 to H2
            -10,  -8,  -6,  -6,  -6,  -6,  -8, -10  // A1 to H1
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

