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
                if (turn == 0) whiteScore += Tables.Rooks.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackRook)
            {
                blackScore += RookWeight;
                if (turn == 1) blackScore -= Tables.Rooks.GetBlackSquareWeight(i);
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
                if (turn == 0) whiteScore += Tables.Kings.GetWhiteSquareWeight(board, i);
            }
            else if (piece == MoveGenerator.blackKing)
            {
                blackScore += KingWeight;
                if (turn == 1) blackScore -= Tables.Kings.GetBlackSquareWeight(board, i);
            }
        }

        // Add pawn structure
        whiteScore += EvaluatePawnStructure(whitePawnFiles);
        blackScore += EvaluatePawnStructure(blackPawnFiles);

        // Return the score adjusted for whose turn it is
        //return (turn == 0) ? (whiteScore - blackScore) : (blackScore - whiteScore);

        if (turn == 0) return (whiteScore - blackScore); 
        return (blackScore - whiteScore);
       
    }


    // Should be extended 
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

