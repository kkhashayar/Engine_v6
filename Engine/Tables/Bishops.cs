namespace Engine.Tables
{
    public static class Bishops
    {
        public static readonly int[] BishopTable = new int[64]
        {
            -10, -5, -5, -5, -5, -5, -5, -10,
             -5,  0,  0,  0,  0,  0,  0,  -5,
             -5,  0,  2,  3,  3,  2,  0,  -5,
             -5,  2,  3,  5,  5,  3,  2,  -5,
             -5,  0,  3,  5,  5,  3,  0,  -5,
             -5,  2,  3,  3,  3,  3,  2,  -5,
             -5,  3,  0,  0,  0,  0,  3,  -5,
            -10, -5, -5, -5, -5, -5, -5, -10
        };


        public static int GetSquareWeight(int square, bool isWhite)
        {
            if (!isWhite)
            {
                square = 63 - square;
            }
            return BishopTable[square];
        }
    }
}
