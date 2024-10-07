namespace Engine.Tables
{
    public static class Bishops
    {
        public static readonly int[] BishopTable = new int[64]
        {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10, 0, 0, 0, 0, 0, 0,-10,
            -10, 0, 5, 10, 10, 5, 0,-10,
            -10, 5, 5, 10, 10, 5, 5,-10,
            -10, 0, 10, 10, 10, 10, 0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10, 5, 0, 0, 0, 0, 5,-10,
            -20,-10,-40,-10,-10,-40,-10,-20,
        };

        public static int GetSquareWeight(int square, bool isWHite)
        {
            if (!isWHite)
            {
                square = 63 - square;
            }
            return BishopTable[square];
        }
    }
}
