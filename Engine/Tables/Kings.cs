namespace Engine.Tables
{
    public static class Kings
    {
        public static readonly int[] WhiteKingTableEndgame = new int[64]
        {
            -3, -2, -2, -2, -2, -2, -2, -3,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -3, -2, -2, -2, -2, -2, -2, -3
        };

        public static readonly int[] BlackKingTableEndgame = new int[64]
        {
            -3, -2, -2, -2, -2, -2, -2, -3,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -3, -2, -2, -2, -2, -2, -2, -3
        };

        public static int GetWhiteSquareWeight(int square)
        {
            return WhiteKingTableEndgame[square];
        }

        public static int GetBlackSquareWeight(int square)
        {
            return BlackKingTableEndgame[square];
        }
    }
}
