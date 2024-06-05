

namespace Engine.Tables;

public static class Pawns
{
    public static readonly int[] WhitePawnTable = new int[64]
    {
        0, 0, 0, 0, 0, 0, 0, 0,
        5, 10, 10, -20, -20, 10, 10, 5,
        5, -5, -10, 0, 0, -10, -5, 5,
        0,  0,  0,  20, 20, 0, 0, 0,
        5,  5,  10, 50, 50, 10, 5, 5,
        10, 10, 20, 25, 25, 20, 10, 10,
        50, 50, 50, 50, 50, 50, 50, 50,
        0,  0,  0,  0,  0,  0,  0,  0
    };

    public static readonly int[] BlackPawnTable = new int[64]
    {
        0, 0, 0, 0, 0, 0, 0, 0,
        50, 50, 50, 50, 50, 50, 50, 50,
        10, 10, 20, 25, 25, 20, 10, 10,
        5, 5, 10, 50, 50, 10, 5, 5,
        0, 0, 0, 20, 20, 0, 0, 0,
        5, -5, -10, 0, 0, -10, -5, 5,
        5, 10, 10, -20, -20, 10, 10, 5,
        0, 0, 0, 0, 0, 0, 0, 0
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
