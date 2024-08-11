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
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  1,  1,  0, -1, -2,
            -2, -1,  0,  0,  0,  0, -1, -2,
            -2, -1, -1, -1, -1, -1, -1, -2,
            -3, -2, -2, -2, -2, -2, -2, -3
        };

        public static readonly int[] KingTableMiddleGame = new int[64]
        {
             0,  0, -4, -4, -4, -4,  0,  0,
             0,  0, -3, -3, -3, -3,  0,  0,
             0,  0, -2, -2, -2, -2,  0,  0,
             0,  0, -1, -1, -1, -1,  0,  0,
             1,  1,  0,  0,  0,  0,  1,  1,
             2,  2,  1,  1,  1,  1,  2,  2,
             2,  3,  2,  1,  1,  2,  3,  2,
             2,  2,  8,  0,  0,  1,  8,  2
        };

        public static int GetSquareWeight(int[] board, int square, bool isWhite)
        {
            var endGameType = Globals.GetEndGameType(board);
            var kingAttackSquares = MoveGenerator.GetKingAttacks(board, isWhite ? 0 : 1);
            GetGamePhaseForKing(board);

            int[] kingTable = (isWhite ? Globals.GameStateForWhiteKing : Globals.GameStateForBlackKing) == GamePhase.EndGame
                ? KingTableEndgame
                : KingTableMiddleGame;

            if (!isWhite)
            {
                square = 63 - square; // Transform the index for black pieces
            }

            return kingTable[square];
        }

        private static void GetGamePhaseForKing(int[] board)
        {
            Globals.GameStateForBlackKing = board.Any(square => square == MoveGenerator.whiteQueen)
                ? GamePhase.MiddleGame
                : GamePhase.EndGame;

            Globals.GameStateForWhiteKing = board.Any(square => square == MoveGenerator.blackQueen)
                ? GamePhase.MiddleGame
                : GamePhase.EndGame;
        }
    }
}
