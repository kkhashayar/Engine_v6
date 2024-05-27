namespace Engine.Tables
{
    public static class Kings
    {
        public static readonly double[] WhiteKingTableEndgame = new double[64]
        {
            -3.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -3.0,
            -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  0.0,  0.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  1.0,  1.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  1.0,  1.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  0.0,  0.0,  0.0, -1.0, -2.0,
            -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0,
            -3.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -3.0
        };

        public static readonly double[] BlackKingTableEndgame = new double[64]
        {
            -3.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -3.0,
            -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  0.0,  0.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  1.0,  1.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  1.0,  1.0,  0.0, -1.0, -2.0,
            -2.0, -1.0,  0.0,  0.0,  0.0,  0.0, -1.0, -2.0,
            -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0,
            -3.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -3.0
        };

        public static double GetWhiteSquareWeight(int square)
        {
            return WhiteKingTableEndgame[square];
        }

        public static double GetBlackSquareWeight(int square)
        {
            return BlackKingTableEndgame[square];
        }
    }
}
