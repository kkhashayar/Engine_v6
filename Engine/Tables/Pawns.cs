

namespace Engine.Tables;

public static class Pawns
{
    public static readonly int[] WhitePawnTable = new int[64]
    {
        0, 0, 0, 0, 0, 0, 0, 0,
        5, 5, 5, 5, 5, 5, 5, 5,
        2, 2, 4, 6, 6, 4, 2, 2,
        1, 1, 2, 5, 5, 2, 1, 1,
        1, 1, 1, 4, 4, 1, 1, 1,
        1, 1, 1, 2, 2, 1, 1, 1,
        1, 1, 1, 1, 1, 1, 1, 1,
        0, 0, 0, 0, 0, 0, 0, 0
    };


    public static readonly int[] BlackPawnTable = new int[64]
    {
         0,  0,  0,  0,  0,  0,  0,  0,
        -1, -1, -1, -1, -1, -1, -1, -1,
        -1, -1, -1, -2, -2, -1, -1, -1,
        -1, -1, -1, -4, -4, -1, -1, -1,
        -1, -1, -2, -5, -5, -2, -1, -1,
        -2, -2, -4, -6, -6, -4, -2, -2,
        -5, -5, -5, -5, -5, -5, -5, -5,
         0,  0,  0,  0,  0,  0,  0,  0
    };

    public static int GetWhiteSquareWeight(int square)
    {
        return WhitePawnTable[square];
    }

    public static int GetBlackSquareWeight(int square)
    {
        return BlackPawnTable[square];
    }
}
