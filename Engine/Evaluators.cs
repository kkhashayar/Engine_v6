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
        private static readonly int MobilityWeight = 1;

        public static int NumberOfWhitePieces { get; private set; }
        public static int NumberOfBlackPieces { get; private set; }
        public static int WhiteStaticScore { get; private set; }
        public static int BlackStaticScore { get; private set; }

        public static int GetByMaterial(int[] board, int turn)
        {
            ResetScores();

            foreach (var square in board)
            {
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

        public static int GetPieceValue(int[] board, int turn, MoveObject? move = default)
        {
            foreach (var square in board)
            {
                if (turn == 0)
                {
                    if (square == 0) continue;
                    else if (square == MoveGenerator.whitePawn) return Tables.Pawns.GetWhiteSquareWeight(move?.EndSquare ?? default);
                    else if (square == MoveGenerator.whiteKnight) return Tables.Knights.GetBlackSquareWeight(move?.EndSquare ?? default);
                    else if (square == MoveGenerator.whiteBishop) return Tables.Bishops.GetWhiteSquareWeight(move?.EndSquare ?? default);
                    else if (square == MoveGenerator.whiteRook) return Tables.Rooks.GetWhiteSquareWeight(move?.EndSquare ?? default);
                    else if (square == MoveGenerator.whiteKing) return Tables.Kings.GetWhiteSquareWeight(move?.EndSquare ?? default);
                }


                if (square == 0) continue;
                else if (square == MoveGenerator.blackPawn) return Tables.Pawns.GetBlackSquareWeight(move?.EndSquare ?? default);
                else if (square == MoveGenerator.blackKnight) return Tables.Knights.GetBlackSquareWeight(move?.EndSquare ?? default);
                else if (square == MoveGenerator.blackBishop) return Tables.Bishops.GetBlackSquareWeight(move?.EndSquare ?? default);
                else if (square == MoveGenerator.blackRook) return Tables.Rooks.GetBlackSquareWeight(move?.EndSquare ?? default);
                else if (square == MoveGenerator.blackKing) return Tables.Kings.GetBlackSquareWeight(move?.EndSquare ?? default);

            }

            return 0;
        }

    }
}
