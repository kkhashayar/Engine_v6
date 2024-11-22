namespace Engine.Tables
{
    public static class Bishops
    {
        public static readonly int[] BishopTable = new int[64]
        {
            -2, -1, -1, -1, -1, -1, -1, -2,
            -1,  0,  0,  0,  0,  0,  0, -1,
            -1,  0,  1,  1,  1,  1,  0, -1,
            -1,  1,  1,  1,  1,  1,  1, -1,
            -1,  0,  1,  1,  1,  1,  0, -1,
            -1,  1,  1,  1,  1,  1,  1, -1,
            -1,  1,  0,  0,  0,  0,  1, -1,
            -2, -1, -4, -1, -1, -4, -1, -2
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
