using System.Diagnostics.CodeAnalysis;

namespace Engine
{
    internal static class Evaluators
    {
        // Piece weights
        private static readonly int PawnWeight = 10;
        private static readonly int KnightWeight = 30;
        private static readonly int BishopWeight = 35;
        private static readonly int RookWeight = 50;
        private static readonly int QueenWeight = 90;
        private static readonly int KingWeight = 10000;


        public static int NumberOfWhitePieces { get; private set; }
        public static int NumberOfBlackPieces { get; private set; }
        public static int WhiteStaticScore { get; private set; }
        public static int BlackStaticScore { get; private set; }
        public static int PieceValueOnPosition { get; set; }


        public static int GetByMaterial(int[] board, int turn)
        {
            ResetScores();
            Globals.GamePhase = GetGamePhase();

            for (int i = 0; i < board.Length; ++i)
            {
                int square = board[i];
                if (square == 0) continue;

                if (square == MoveGenerator.whitePawn)
                {
                    WhiteStaticScore += PawnWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackPawn)
                {
                    BlackStaticScore += PawnWeight;

                    NumberOfBlackPieces++;
                }
                else if (square == MoveGenerator.whiteKnight)
                {
                    WhiteStaticScore += KnightWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackKnight)
                {
                    BlackStaticScore += KnightWeight;
                    NumberOfBlackPieces++;
                }
                else if (square == MoveGenerator.whiteBishop)
                {
                    WhiteStaticScore += BishopWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackBishop)
                {
                    BlackStaticScore += BishopWeight;
                    NumberOfBlackPieces++;
                }
                else if (square == MoveGenerator.whiteRook)
                {
                    WhiteStaticScore += RookWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackRook)
                {
                    BlackStaticScore += RookWeight;
                    NumberOfBlackPieces++;
                }
                else if (square == MoveGenerator.whiteQueen)
                {
                    WhiteStaticScore += QueenWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackQueen)
                {
                    BlackStaticScore += QueenWeight;
                    NumberOfBlackPieces++;
                }
                else if (square == MoveGenerator.whiteKing)
                {
                    WhiteStaticScore += KingWeight;
                    NumberOfWhitePieces++;
                }
                else if (square == MoveGenerator.blackKing)
                {
                    BlackStaticScore += KingWeight;
                    NumberOfBlackPieces++;
                }
            }

            if (turn == 0)
            {
                return (WhiteStaticScore - BlackStaticScore);
            }
            else
            {
                return (BlackStaticScore - WhiteStaticScore);
            }
        }

        private static void ResetScores()
        {
            WhiteStaticScore = 0;
            BlackStaticScore = 0;
            NumberOfWhitePieces = 0;
            NumberOfBlackPieces = 0;
        }

        private static GamePhase GetGamePhase()
        {
            if (NumberOfWhitePieces + NumberOfBlackPieces <= 10)
            {
                return GamePhase.EndGame;
            }
            else if (NumberOfWhitePieces + NumberOfBlackPieces <= 20)
            {
                return GamePhase.MiddleGame;
            }
            else
            {
                return GamePhase.Opening;
            }
        }

    }
}


public enum GamePhase
{
    Opening,
    MiddleGame,
    EndGame
}
