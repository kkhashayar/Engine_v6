namespace Engine.Tables
{
    public static class Pawns
    {
        public static readonly int[] WhitePawnTable = new int[64]
        {
             0,  0,  0,  0,  0,  0,  0,  0, // A8 to H8
             5,  5,  5,  5,  5,  5,  5,  5, // A7 to H7
             1,  1,  2,  3,  3,  2,  1,  1, // A6 to H6
             0,  0,  2,  4,  4,  2,  0,  0, // A5 to H5
             0,  2,  4, 200,200, 4,  2,  0, // A4 to H4
             1,  1,  1,  2,  2,  1,  1,  1, // A3 to H3
             2,  2,  2,  0,  0,  2,  2,  2, // A2 to H2
             0,  0,  0,  0,  0,  0,  0,  0  // A1 to H1
        };

        public static readonly int[] BlackPawnTable = new int[64]
        {
             0,  0,  0,  0,  0,  0,  0,  0, // A8 to H8
             2,  2,  2,  0,  0,  2,  2,  2, // A7 to H7
             1,  1,  1,  2,  2,  1,  1,  1, // A6 to H6
             0,  2,  4, 200,200, 4,  2,  0, // A5 to H5
             0,  0,  2,  4,  4,  2,  0,  0, // A4 to H4
             1,  1,  2,  3,  3,  2,  1,  1, // A3 to H3
             5,  5,  5,  5,  5,  5,  5,  5, // A2 to H2
             0,  0,  0,  0,  0,  0,  0,  0  // A1 to H1
        };

        public static int GetSquareWeight(int square, bool isWhite)
        {
            if (!isWhite)
            {
                return BlackPawnTable[square];
            }
            return WhitePawnTable[square];
        }
    }
}