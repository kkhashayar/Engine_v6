namespace Engine;

public static class Evaluators
{
    private static readonly int PawnValue = 100;
    private static readonly int KnightValue = 320;
    private static readonly int BishopValue = 330;
    private static readonly int RookValue = 500;
    private static readonly int QueenValue = 900;
    private static readonly int KingValue = 20000;

    private static int[] whitePawnFiles = new int[8];
    private static int[] blackPawnFiles = new int[8];
    public static int EvaluatePosition(int[] board, int turn)
    {
        int whiteScore = 0;
        int blackScore = 0;

        for (int i = 0; i < board.Length; ++i)
        {
            int piece = board[i];
            int file = i % 8;

            if (piece == MoveGenerator.whitePawn)
            {
                whiteScore += PawnValue;
                
            }
            else if (piece == MoveGenerator.blackPawn)
            {
                blackScore += PawnValue;
                
            }
            else if (piece == MoveGenerator.whiteKnight)
            {
                whiteScore += KnightValue;
            }
            else if (piece == MoveGenerator.blackKnight)
            {
                blackScore += KnightValue;
            }
            else if (piece == MoveGenerator.whiteBishop)
            {
                whiteScore += BishopValue;
            }
            else if (piece == MoveGenerator.blackBishop)
            {
                blackScore += BishopValue;
            }
            else if (piece == MoveGenerator.whiteRook)
            {
                whiteScore += RookValue;
            }
            else if (piece == MoveGenerator.blackRook)
            {
                blackScore += RookValue;
            }
            else if (piece == MoveGenerator.whiteQueen)
            {
                whiteScore += QueenValue;
            }
            else if (piece == MoveGenerator.blackQueen)
            {
                blackScore += QueenValue;
            }
            else if (piece == MoveGenerator.whiteKing)
            {
                whiteScore += KingValue;
            }
            else if (piece == MoveGenerator.blackKing)
            {
                blackScore += KingValue;
            }
        }

        return (turn == 0) ? (whiteScore - blackScore) : (blackScore - whiteScore);
    }
}

