

namespace Engine.Tables;

public static class Pawns
{
    public static readonly double[] WhitePawnTable = new double[64]
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


    public static readonly double[] BlackPawnTable = new double[64]
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

    public static double GetWhiteSquareWeight(int square)
    {
        return WhitePawnTable[square];
    }

    public static double GetBlackSquareWeight(int square)
    {
        return BlackPawnTable[square];
    }
}
