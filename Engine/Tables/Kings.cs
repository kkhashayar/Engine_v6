using Engine.Core;
using Engine.Enums;

namespace Engine.Tables
{
    public static class Kings
    {

        public static readonly int[] WhiteKingTableEndgame = new int[64]
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

        public static readonly int[] BlackKingTableEndgame = new int[64]
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

        public static readonly int[] WhiteKingTableMiddleGame = new int[64]
        {
             0,  0, -4, -4, -4, -4,  0,  0,
             0,  0, -3, -3, -3, -3,  0,  0,
             0,  0, -2, -2, -2, -2,  0,  0,
             0,  0, -1, -1, -1, -1,  0,  0,
             1,  1,  0,  0,  0,  0,  1,  1,
             2,  2,  1,  1,  1,  1,  2,  2,
             2,  3,  2,  1,  1,  2,  3,  2,
             2,  2,  5,  0,  0,  1,  5,  2
        };

        public static readonly int[] BlackKingTableMiddleGame = new int[64]
        {
             2,  2,  5,  0,  0,  1,  5,  2,
             2,  3,  2,  1,  1,  2,  3,  2,
             2,  2,  1,  1,  1,  1,  2,  2,
             1,  1,  0,  0,  0,  0,  1,  1,
             0,  0, -1, -1, -1, -1,  0,  0,
             0,  0, -2, -2, -2, -2,  0,  0,
             0,  0, -3, -3, -3, -3,  0,  0,
             0,  0, -4, -4, -4, -4,  0,  0
        };

        public static int GetWhiteSquareWeight(int[] board, int square)
        {
            var endGameType = Globals.GetEndGameType(board);
            var kingAttackSquares = MoveGenerator.GetKingAttacks(board, 0);
            GetGamePhaseForKing(board);

            if (Globals.GameStateForWhiteKing == GamePhase.EndGame)
            {
                return WhiteKingTableEndgame[square];
            }
            return WhiteKingTableMiddleGame[square];
        }

        public static int GetBlackSquareWeight(int[] board, int square)
        {
            var endGameType = Globals.GetEndGameType(board);
            var kingAttackSquares = MoveGenerator.GetKingAttacks(board, 1);
            GetGamePhaseForKing(board);

            if (Globals.GameStateForBlackKing == GamePhase.EndGame)
            {
                return BlackKingTableEndgame[square];
            }
            return BlackKingTableMiddleGame[square];
        }

        private static void GetGamePhaseForKing(int[] board)
        {

            if (board.Any(square => square == MoveGenerator.whiteQueen))
            {
                Globals.GameStateForBlackKing = GamePhase.MiddleGame;
            }
            else
            {
                Globals.GameStateForBlackKing = GamePhase.EndGame;
            }

            if (board.Any(square => square == MoveGenerator.blackQueen))
            {
                Globals.GameStateForWhiteKing = GamePhase.MiddleGame;
            }
            else
            {
                Globals.GameStateForWhiteKing = GamePhase.EndGame;
            }
        }
    }
}
