namespace Engine.Tables
{
    public static class Pawns
    {
        public static readonly decimal[] PawnTable = new decimal[64]
        {
            0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m,
            0.50m,  0.50m,  0.50m,  0.50m,  0.50m,  0.50m,  0.50m,  0.50m,
            0.10m,  0.10m,  0.20m,  0.30m,  0.30m,  0.20m,  0.10m,  0.10m,
            0.05m,  0.05m,  0.10m,  0.27m,  0.27m,  0.10m,  0.05m,  0.05m,
            0.00m,  0.00m,  0.00m,  0.25m,  0.25m,  0.00m,  0.00m,  0.00m,
            0.05m, -0.05m, -0.10m, 0.00m,  0.00m, -0.10m, -0.05m, 0.05m,
            0.05m,  0.10m,  0.10m, -0.25m, -0.25m,  0.10m,  0.10m, 0.05m,
            0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m,  0.00m, 0.00m
        };
        public static decimal GetSquareWeight(int square, bool isWhite)
        {
            if (!isWhite)
            {
                square = 63 - square;
            }
            return PawnTable[square];
        }
    }
}


// TODO: pawn square weights should be little bit higher than Knight