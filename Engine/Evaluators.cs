
//using System.Diagnostics.CodeAnalysis;

//namespace Engine;

//internal static class Evaluators
//{
//    public static int numbersOfWhitePieces { get; set; }
//    public static int numbersOfBlackPieces { get; set; }
//    public static int whiteStaticScore { get; set; }
//    public static int blackStaticScore { get; set; }

//    public static decimal GetByMaterial(int[] board, int turn)
//    {

//        numbersOfWhitePieces = 0;
//        numbersOfBlackPieces = 0;
//        whiteStaticScore = 0;
//        blackStaticScore = 0;


//        decimal score = 0;
//        for (int i = 0; i < 64; i++)
//        {
//            int piece = board[i];
//            if (piece == 0) continue;
//            if (piece == MoveGenerator.whitePawn)
//            {
//                whiteStaticScore += 1;
//                numbersOfWhitePieces++;
//            }
//            else if (piece == MoveGenerator.whiteKnight)
//            {
//                whiteStaticScore += 3;
//                numbersOfWhitePieces++;
//            }
//            else if (piece == MoveGenerator.whiteBishop)
//            {
//                whiteStaticScore += 4;
//                numbersOfWhitePieces++;
//            }
//            else if (piece == MoveGenerator.whiteRook)
//            {
//                whiteStaticScore += 5;
//                numbersOfWhitePieces++;
//            }
//            else if (piece == MoveGenerator.whiteQueen)
//            {
//                whiteStaticScore += 9;
//                numbersOfWhitePieces++;
//            }
//            else if (piece == MoveGenerator.whiteKing)
//            {
//                whiteStaticScore += 0;
//            }
//            else if (piece == MoveGenerator.blackPawn)
//            {
//                blackStaticScore += 1;
//                numbersOfBlackPieces++;
//            }
//            else if (piece == MoveGenerator.blackKnight)
//            {
//                blackStaticScore += 3;
//                numbersOfBlackPieces++;
//            }
//            else if (piece == MoveGenerator.blackBishop)
//            {
//                blackStaticScore += 4;
//                numbersOfBlackPieces++;
//            }
//            else if (piece == MoveGenerator.blackRook)
//            {
//                blackStaticScore += 5;
//                numbersOfBlackPieces++;
//            }
//            else if (piece == MoveGenerator.blackQueen)
//            {
//                blackStaticScore += 9;
//                numbersOfBlackPieces++;
//            }
//            else if (piece == MoveGenerator.blackKing)
//            {
//                blackStaticScore += 0;
//            }
//        }
//       if(turn == 0) return whiteStaticScore - blackStaticScore;
//       return blackStaticScore - whiteStaticScore;
//    }


//    private static decimal GetPieceValue(int piece)
//    {

//        if (piece == MoveGenerator.whitePawn || piece == MoveGenerator.blackPawn) return 1;
//        if (piece == MoveGenerator.whiteKnight || piece == MoveGenerator.blackKnight) return 3;
//        if (piece == MoveGenerator.whiteBishop || piece == MoveGenerator.blackBishop) return 3.2m;
//        if (piece == MoveGenerator.whiteRook || piece == MoveGenerator.blackRook) return 5;
//        if (piece == MoveGenerator.whiteQueen || piece == MoveGenerator.blackQueen) return 9;
//        if (piece == MoveGenerator.whiteKing || piece == MoveGenerator.blackKing) return 999999;
//        return 0;   

//    }
//}


using System.Diagnostics.CodeAnalysis;

namespace Engine
{
    internal static class Evaluators
    {
        public static int numbersOfWhitePieces { get; set; }
        public static int numbersOfBlackPieces { get; set; }
        public static decimal whiteStaticScore { get; set; }
        public static decimal blackStaticScore { get; set; }

        private const decimal KingWt = 0; // Typically not used in material evaluation
        private const decimal QueenWt = 9;
        private const decimal RookWt = 5;
        private const decimal KnightWt = 3;
        private const decimal BishopWt = 3.2m;
        private const decimal PawnWt = 1;
        private const decimal MobilityWt = 0.1m; // Adjust this weight as necessary

        public static decimal GetByMaterial(int[] board, int turn)
        {
            numbersOfWhitePieces = 0;
            numbersOfBlackPieces = 0;
            whiteStaticScore = 0;
            blackStaticScore = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = board[i];
                if (piece == 0) continue;

                whiteStaticScore += GetPieceValue(piece, true);
                blackStaticScore += GetPieceValue(piece, false);
            }

            decimal materialScore = whiteStaticScore - blackStaticScore;

            decimal whiteMobility = GetMobility(board, 0);
            decimal blackMobility = GetMobility(board, 1);
            decimal mobilityScore = MobilityWt * (whiteMobility - blackMobility);

            int who2Move;
            if (turn == 0)
            {
                who2Move = 1;  // White to move
            }
            else
            {
                who2Move = -1; // Black to move
            }
            return (materialScore + mobilityScore) * who2Move;

        }

        private static decimal GetPieceValue(int piece, bool isWhite)
        {
            if (isWhite)
            {
                if (piece == MoveGenerator.whitePawn) return PawnWt;
                if (piece == MoveGenerator.whiteKnight) return KnightWt;
                if (piece == MoveGenerator.whiteBishop) return BishopWt;
                if (piece == MoveGenerator.whiteRook) return RookWt;
                if (piece == MoveGenerator.whiteQueen) return QueenWt;
                if (piece == MoveGenerator.whiteKing) return KingWt;
            }
            else
            {
                if (piece == MoveGenerator.blackPawn) return PawnWt;
                if (piece == MoveGenerator.blackKnight) return KnightWt;
                if (piece == MoveGenerator.blackBishop) return BishopWt;
                if (piece == MoveGenerator.blackRook) return RookWt;
                if (piece == MoveGenerator.blackQueen) return QueenWt;
                if (piece == MoveGenerator.blackKing) return KingWt;
            }
            return 0;
        }

        private static int GetMobility(int[] board, int turn)
        {
           return MoveGenerator.GenerateAllMoves(board, turn, true).Count;
           
        }
    }
}

