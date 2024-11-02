using Engine.Core;
using Engine.Enums;

namespace Engine.Tables
{
    public static class Kings
    {
        public static readonly decimal[] KingTableEndgame = new decimal[64]
        {
            -0.03m, -0.02m, -0.02m, -0.02m, -0.02m, -0.02m, -0.02m, -0.03m,
            -0.02m, -0.01m, -0.01m, -0.01m, -0.01m, -0.01m, -0.01m, -0.02m,
            -0.02m, -0.01m,  0.00m,  0.00m,  0.00m,  0.00m, -0.01m, -0.02m,
            -0.02m, -0.01m,  0.00m,  0.01m,  0.01m,  0.00m, -0.01m, -0.02m,
            -0.02m, -0.01m,  0.00m,  0.01m,  0.01m,  0.00m, -0.01m, -0.02m,
            -0.02m, -0.01m,  0.00m,  0.00m,  0.00m,  0.00m, -0.01m, -0.02m,
            -0.02m, -0.01m, -0.01m, -0.01m, -0.01m, -0.01m, -0.01m, -0.02m,
            -0.03m, -0.02m, -0.02m, -0.02m, -0.02m, -0.02m, -0.02m, -0.03m
        };

        public static readonly decimal[] KingTableMiddleGame = new decimal[64]
        {
             0.00m,  0.00m, -0.04m, -0.04m, -0.04m, -0.04m,  0.00m,  0.00m,
             0.00m,  0.00m, -0.03m, -0.03m, -0.03m, -0.03m,  0.00m,  0.00m,
             0.00m,  0.00m, -0.02m, -0.02m, -0.02m, -0.02m,  0.00m,  0.00m,
             0.00m,  0.00m, -0.01m, -0.01m, -0.01m, -0.01m,  0.00m,  0.00m,
             0.01m,  0.01m,  0.00m,  0.00m,  0.00m,  0.00m,  0.01m,  0.01m,
             0.02m,  0.02m,  0.01m,  0.01m,  0.01m,  0.01m,  0.02m,  0.02m,
             0.02m,  0.03m,  0.02m,  0.01m,  0.01m,  0.02m,  0.03m,  0.02m,
             0.02m,  0.02m,  0.08m,  0.00m,  0.00m,  0.01m,  0.08m,  0.02m
        };



        public static decimal GetEndGameWeight(int Square, bool isWhite)
        {
            return KingTableEndgame[Square];    
        }


        public static decimal GetSquareWeight(int[] board, int square, bool isWhite)
        {
            var endGameType = Globals.GetEndGameType(board);
            var kingAttackSquares = MoveGenerator.GetKingAttacks(board, isWhite ? 0 : 1);
            GetGamePhaseForKing(board);

            decimal[] kingTable = (isWhite ? Globals.GameStateForWhiteKing : Globals.GameStateForBlackKing) == GamePhase.EndGame
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
