namespace Engine.Tables
{
    public static class Pawns
    {
        public static readonly int[] PawnTable = new int[64]
        {
            0,  0,  0,  0,  0,  0,  0,  0,
            5,  5,  5,  6,  6,  5,  5,  5,
            1,  1,  2,  5,  5,  2,  1,  1,
            0,  0,  1,  4,  4,  1,  0,  0,
            0,  0,  0,  3,  3,  0,  0,  0,
            0, -1, -1,  0,  0, -1, -1,  0,
            0,  1,  1, -3, -3,  1,  1,  0,
            0,  0,  0,  0,  0,  0,  0,  0
        };
        public static int GetSquareWeight(int square, bool isWhite)
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