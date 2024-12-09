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
             0,  0, -3, -3, -3, -3,  0,  0,
             0,  0, -2, -2, -2, -2,  0,  0,
             0,  0, -1, -1, -1, -1,  0,  0,
             0,  0,  0,  1,  1,  0,  0,  0,
             1,  1,  1,  2,  2,  1,  1,  1,
             2,  2,  2,  3,  3,  2,  2,  2,
             2,  3,  3,  3,  3,  3,  3,  2,
             2,  2,  3,  2,  2,  3,  2,  2
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
