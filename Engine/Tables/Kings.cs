using Engine.Core;
using Engine.Enums;

namespace Engine.Tables
{
    public static class Kings
    {
        public static readonly int[] KingTableEndgame = new int[64]
        {
            -3, -2, -2, -2, -2, -2, -2, -3,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  1,  2,  2,  1, -1, -2,
            -2, -1,  1,  2,  2,  1, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -3, -2, -2, -2, -2, -2, -2, -3
        };


        public static readonly int[] KingTableMiddleGame = new int[64]
        {
             1,   1,   1,   1,   1,   1,  20,   1, // A8 to H8
             1,   1,   1,   1,   1,   1,  10,   1, // A7 to H7
             1,   1,   1,   1,   1,   1,   1,   1, // A6 to H6
             1,   1,   1,   1,   1,   1,   1,   1, // A5 to H5
             1,   1,   1,   1,   1,   1,   1,   1, // A4 to H4
             1,   1,   1,   1,   1,   1,   1,   1, // A3 to H3
             1,   1,   1,   1,   1,   1,  10,   1, // A2 to H2
             1,   1,  20,   1,   1,   1,  30,   1  // A1 to H1
        };






        public static int GetEndGameWeight(int square, bool isWhite)
        {
            return KingTableEndgame[square];
        }

        public static int GetMiddleGameWeight(int square, bool isWhite)
        {
            return KingTableMiddleGame[square];
        }
    }
}
