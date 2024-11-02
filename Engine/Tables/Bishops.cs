namespace Engine.Tables
{
    public static class Bishops
    {
        public static readonly decimal[] BishopTable = new decimal[64]
        {
            -0.20m, -0.10m, -0.10m, -0.10m, -0.10m, -0.10m, -0.10m, -0.20m,
            -0.10m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m, -0.10m,
            -0.10m,  0.00m,  0.05m,  0.10m,  0.10m,  0.05m,  0.00m, -0.10m,
            -0.10m,  0.05m,  0.05m,  0.10m,  0.10m,  0.05m,  0.05m, -0.10m,
            -0.10m,  0.00m,  0.10m,  0.10m,  0.10m,  0.10m,  0.00m, -0.10m,
            -0.10m,  0.10m,  0.10m,  0.10m,  0.10m,  0.10m,  0.10m, -0.10m,
            -0.10m,  0.05m,  0.00m,  0.00m,  0.00m,  0.00m,  0.05m, -0.10m,
            -0.20m, -0.10m, -0.40m, -0.10m, -0.10m, -0.40m, -0.10m, -0.20m
        };


        public static decimal GetSquareWeight(int square, bool isWHite)
        {
            if (!isWHite)
            {
                square = 63 - square;
            }
            return BishopTable[square];
        }
    }
}
