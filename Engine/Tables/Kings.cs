namespace Engine.Tables
{
    public static class Kings
    {
        public static readonly decimal[] WhiteKingTableEndgame = new decimal[64]
        {
            -3.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -3.0m,
            -2.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  0.0m,  0.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  1.0m,  1.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  1.0m,  1.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  0.0m,  0.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -2.0m,
            -3.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -3.0m
        };

        public static readonly decimal[] BlackKingTableEndgame = new decimal[64]
        {
            -3.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -3.0m,
            -2.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  0.0m,  0.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  1.0m,  1.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  1.0m,  1.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m,  0.0m,  0.0m,  0.0m,  0.0m, -1.0m, -2.0m,
            -2.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -1.0m, -2.0m,
            -3.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -2.0m, -3.0m
        };

        public static decimal GetWhiteSquareWeight(int square)
        {
            return WhiteKingTableEndgame[square];
        }

        public static decimal GetBlackSquareWeight(int square)
        {
            return BlackKingTableEndgame[square];
        }
    }
}
