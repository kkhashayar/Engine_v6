namespace Engine.Tables
{
    internal class Rooks
    {
        public static readonly int[] WhiteRookTable = new int[64]
        {
            0, 0, 0, 0, 0, 0, 0, 0,
            30, 60, 60, 60, 60, 60, 60, 30,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            25, 38, 38, 38, 38, 38, 38, 25,
            50, 50, 50, 50, 50, 50, 50, 50
        };

        public static readonly int[] BlackRookTable = new int[64]
        {
            50, 50, 50, 50, 50, 50, 50, 50,
            25, 38, 38, 38, 38, 38, 38, 25,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            13, 25, 25, 25, 25, 25, 25, 13,
            30, 60, 60, 60, 60, 60, 60, 30,
            0, 0, 0, 0, 0, 0, 0, 0
        };



        public static int GetWhiteSquareWeight(int index)
        {
            return WhiteRookTable[index];
        }

        public static int GetBlackSquareWeight(int index)
        {
            return BlackRookTable[index];
        }
    }
}
