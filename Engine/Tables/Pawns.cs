namespace Engine.Tables
{
    public static class Pawns
    {
        public static readonly int[] PawnTable = new int[64]
        {
            0,  0,  0,  0,  0,  0,  0,  0, // A8 to H8
            0,  0,  0,  2,  2,  0,  0,  0, // A7 to H7
            0,  0,  2,  4,  4,  2,  0,  0, // A6 to H6
            0,  2,  4,  16, 16, 4,  2,  0, // A5 to H5
            2,  4,  8,  50, 50, 8,  4,  2, // A4 to H4
            2,  2,  2,  10,  10,  2,  2,  2, // A3 to H3
            0,  0,  0,  0,  0,  0,  0,  0, // A2 to H2
            0,  0,  0,  0,  0,  0,  0,  0  // A1 to H1
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