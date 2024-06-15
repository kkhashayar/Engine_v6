

//namespace Engine;
//internal static class Evaluators
//{
//    // Piece weights
//    private static readonly int PawnWeight = 10;
//    private static readonly int KnightWeight = 30;
//    private static readonly int BishopWeight = 35;
//    private static readonly int RookWeight = 50;
//    private static readonly int QueenWeight = 90;
//    private static readonly int KingWeight = 10000;

//    private static int NumberOfWhitePieces { get; set; }
//    private static int NumberOfBlackPieces { get; set; }
//    private static int WhiteStaticScore { get; set; }
//    private static int BlackStaticScore { get; set; }
//    private static int WhitePositionValue { get; set; }
//    private static int BlackPositionValue { get; set; }
//    private static int WhiteKingActivityScore { get; set; }
//    private static int BlackKingActivityScore { get; set; }
//    private static readonly int KingActivityWeight = 5;
//    private static readonly int MaxKingProximityWeight = 10;

//    public static int EvaluatePosition(int[] board, int turn)
//    {
//        Globals.GamePhase = GetGamePhase();

//        ResetScores();


//        for (int i = 0; i < board.Length; ++i)
//        {
//            int square = board[i];
//            if (square == 0) continue;

//            if (square == MoveGenerator.whitePawn)
//            {
//                WhiteStaticScore += PawnWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackPawn)
//            {
//                BlackStaticScore += PawnWeight;
//                NumberOfBlackPieces++;
//            }
//            if (square == MoveGenerator.whiteKnight)
//            {
//                WhiteStaticScore += KnightWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackKnight)
//            {
//                BlackStaticScore += KnightWeight;
//                NumberOfBlackPieces++;
//            }
//            if (square == MoveGenerator.whiteBishop)
//            {
//                WhiteStaticScore += BishopWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackBishop)
//            {
//                BlackStaticScore += BishopWeight;
//                NumberOfBlackPieces++;
//            }
//            if (square == MoveGenerator.whiteRook)
//            {
//                WhiteStaticScore += RookWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackRook)
//            {
//                BlackStaticScore += RookWeight;
//                NumberOfBlackPieces++;
//            }
//            if (square == MoveGenerator.whiteQueen)
//            {
//                WhiteStaticScore += QueenWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackQueen)
//            {
//                BlackStaticScore += QueenWeight;
//                NumberOfBlackPieces++;
//            }
//            if (square == MoveGenerator.whiteKing)
//            {
//                WhiteStaticScore += KingWeight;
//                NumberOfWhitePieces++;
//            }
//            if (square == MoveGenerator.blackKing)
//            {
//                BlackStaticScore += KingWeight;
//                NumberOfBlackPieces++;
//            }
//        }
//        GetPieceTableValues(board);
//        if (Globals.GamePhase == GamePhase.EndGame)
//        {
//            EvaluateKingActivity(board);
//        }

//        if (turn == 0)
//        {
//            return (WhiteStaticScore - BlackStaticScore)
//                + (NumberOfWhitePieces - NumberOfBlackPieces)
//                + (WhitePositionValue - BlackPositionValue)
//                + (WhiteKingActivityScore - BlackKingActivityScore);
//        }
//        else
//        {
//            return (BlackStaticScore - WhiteStaticScore)
//                + (NumberOfBlackPieces - NumberOfWhitePieces)
//                + (BlackPositionValue - WhitePositionValue)
//                + (BlackKingActivityScore - WhiteKingActivityScore);
//        }
//    }

//    private static void GetPieceTableValues(int[] board)
//    {
//        for (int square = 0; square < board.Length; square++)
//        {
//            if (square == 0) continue;
//            if (board[square] == MoveGenerator.whitePawn) WhitePositionValue += Tables.Pawns.GetWhiteSquareWeight(square);
//            if (board[square] == MoveGenerator.blackPawn) BlackPositionValue += Tables.Pawns.GetBlackSquareWeight(square);
//            if (board[square] == MoveGenerator.whiteKnight) WhitePositionValue += Tables.Knights.GetWhiteSquareWeight(square);
//            if (board[square] == MoveGenerator.blackKnight) BlackPositionValue += Tables.Knights.GetBlackSquareWeight(square);
//            if (board[square] == MoveGenerator.whiteBishop) WhitePositionValue += Tables.Bishops.GetWhiteSquareWeight(square);
//            if (board[square] == MoveGenerator.blackBishop) BlackPositionValue += Tables.Bishops.GetBlackSquareWeight(square);
//            if (board[square] == MoveGenerator.whiteRook) WhitePositionValue += Tables.Rooks.GetWhiteSquareWeight(square);
//            if (board[square] == MoveGenerator.blackRook) BlackPositionValue += Tables.Rooks.GetBlackSquareWeight(square);
//            //if (square == MoveGenerator.whiteQueen) WhitePositionValue += Tables.Queens.GetWhiteSquareWeight(board[square]);
//            //if (square == MoveGenerator.blackQueen) BlackPositionValue += Tables.Queens.GetBlackSquareWeight(board[square]);
//            if (board[square] == MoveGenerator.whiteKing) WhitePositionValue += Tables.Kings.GetWhiteSquareWeight(square);
//            if (board[square] == MoveGenerator.blackKing) BlackPositionValue += Tables.Kings.GetBlackSquareWeight(square);
//        }
//    }

//    private static void ResetScores()
//    {
//        WhiteStaticScore = 0;
//        BlackStaticScore = 0;
//        NumberOfWhitePieces = 0;
//        NumberOfBlackPieces = 0;
//        WhitePositionValue = 0;
//        BlackPositionValue = 0;
//        WhiteKingActivityScore = 0;
//        BlackKingActivityScore = 0;
//    }

//    private static void EvaluateKingActivity(int[] board)
//    {
//        int whiteKingSquare = Globals.GetWhiteKingSquare(board);
//        int blackKingSquare = Globals.GetBlackKingSquare(board);
//        WhiteKingActivityScore = CalculateKingActivityScore(board, 0, whiteKingSquare, blackKingSquare);
//        BlackKingActivityScore = CalculateKingActivityScore(board, 1, blackKingSquare, whiteKingSquare);
//    }

//    private static int CalculateKingActivityScore(int[] board, int turn, int kingSquare, int opponentKingSquare)
//    {
//        var kingMoves = MoveGenerator.GetKingAttacks(board, turn);
//        int activityScore = 0;

//        foreach (var move in kingMoves)
//        {
//            int distanceToOpponentKing = GetManhattanDistance(move.EndSquare, opponentKingSquare);
//            int proximityWeight = MaxKingProximityWeight - distanceToOpponentKing;
//            activityScore += KingActivityWeight + proximityWeight;
//        }

//        return activityScore;
//    }

//    private static int GetManhattanDistance(int square1, int square2)
//    {
//        int x1 = square1 % 8;
//        int y1 = square1 / 8;
//        int x2 = square2 % 8;
//        int y2 = square2 / 8;
//        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
//    }

//    public static GamePhase GetGamePhase()
//    {
//        if (NumberOfWhitePieces + NumberOfBlackPieces <= 10)
//        {
//            return GamePhase.EndGame;
//        }
//        else if (NumberOfWhitePieces + NumberOfBlackPieces <= 20)
//        {
//            return GamePhase.MiddleGame;
//        }
//        else
//        {
//            return GamePhase.Opening;
//        }
//    }
//}




namespace Engine;

internal static class Evaluators
{
    private static readonly int PawnWeight = 10;
    private static readonly int KnightWeight = 30;
    private static readonly int BishopWeight = 35;
    private static readonly int RookWeight = 50;
    private static readonly int QueenWeight = 90;
    private static readonly int KingWeight = 10000;

    private static int[] whitePawnFiles = new int[8];
    private static int[] blackPawnFiles = new int[8];

    public static int EvaluatePosition(int[] board, int turn)
    {
        int whiteScore = 0;
        int blackScore = 0;

        Array.Clear(whitePawnFiles, 0, whitePawnFiles.Length);
        Array.Clear(blackPawnFiles, 0, blackPawnFiles.Length);

        for (int i = 0; i < board.Length; ++i)
        {
            int piece = board[i];
            int file = i % 8;

            if (piece == MoveGenerator.whitePawn)
            {
                whiteScore += PawnWeight;
                if (turn == 0) whiteScore += Tables.Pawns.GetWhiteSquareWeight(i);
                whitePawnFiles[file]++;
            }
            else if (piece == MoveGenerator.blackPawn)
            {
                blackScore += PawnWeight;
                if (turn == 1) blackScore -= Tables.Pawns.GetBlackSquareWeight(i);
                blackPawnFiles[file]++;
            }
            else if (piece == MoveGenerator.whiteKnight)
            {
                whiteScore += KnightWeight;
                if (turn == 0) whiteScore += Tables.Knights.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackKnight)
            {
                blackScore += KnightWeight;
                if (turn == 1) blackScore -= Tables.Knights.GetBlackSquareWeight(i);
            }
            else if (piece == MoveGenerator.whiteBishop)
            {
                whiteScore += BishopWeight;
                if (turn == 0) whiteScore += Tables.Bishops.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackBishop)
            {
                blackScore += BishopWeight;
                if(turn == 1) blackScore -= Tables.Bishops.GetBlackSquareWeight(i);    
            }
            else if (piece == MoveGenerator.whiteRook)
            {
                whiteScore += RookWeight;
            }
            else if (piece == MoveGenerator.blackRook)
            {
                blackScore += RookWeight;
            }
            else if (piece == MoveGenerator.whiteQueen)
            {
                whiteScore += QueenWeight;
            }
            else if (piece == MoveGenerator.blackQueen)
            {
                blackScore += QueenWeight;
            }
            else if (piece == MoveGenerator.whiteKing)
            {
                whiteScore += KingWeight;
            }
            else if (piece == MoveGenerator.blackKing)
            {
                blackScore += KingWeight;
            }
        }

        // Add pawn structure evaluation after piece evaluation
        whiteScore += EvaluatePawnStructure(whitePawnFiles);
        blackScore += EvaluatePawnStructure(blackPawnFiles);

        // Return the score adjusted for whose turn it is
        return (turn == 0) ? (whiteScore - blackScore) : (blackScore - whiteScore);
    }

    private static int EvaluatePawnStructure(int[] pawnFiles)
    {
        int score = 0;
        for (int file = 0; file < 8; file++)
        {
            int count = pawnFiles[file];
            if (count > 1)
            {
                // Deduct score for each additional pawn in the same file (doubled pawns)
                score -= (count - 1) * 5;
            }

            // Check for isolated pawns
            bool isIsolated = (file == 0 || pawnFiles[file - 1] == 0) && (file == 7 || pawnFiles[file + 1] == 0);
            if (isIsolated && count > 0)
            {
                score -= 10;
            }

            // Check for passed pawns
            bool isPassed = true;  // Assume it's passed until proven otherwise
            for (int adjacent = file - 1; adjacent <= file + 1; adjacent++)
            {
                if (adjacent >= 0 && adjacent < 8 && pawnFiles[adjacent] > 0)
                {
                    isPassed = false;
                    break;
                }
            }
            if (isPassed && count > 0)
            {
                score += 20 * count;  // Add bonus for each passed pawn
            }
        }
        return score;
    }
}

