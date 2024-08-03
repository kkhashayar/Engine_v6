using Engine.Core;

namespace Engine;

public static class Evaluators
{
    private static readonly int PawnValue = 10;
    private static readonly int KnightValue = 320;
    private static readonly int BishopValue = 330;
    private static readonly int RookValue = 500;
    private static readonly int QueenValue = 900;
    private static readonly int KingValue = 20000;

    public static int EvaluatePosition(int[] board, int turn)
    {
        int whiteScore = 0;
        int blackScore = 0;

        for (int i = 0; i < board.Length; ++i)
        {
            int piece = board[i];
            if (piece == MoveGenerator.whitePawn)
            {
                whiteScore += PawnValue;
                if (turn == 0) whiteScore += Tables.Pawns.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackPawn)
            {
                blackScore += PawnValue;
                if (turn == 1) blackScore -= Tables.Pawns.GetBlackSquareWeight(i);
            }
            else if (piece == MoveGenerator.whiteKnight)
            {
                whiteScore += KnightValue;
                //if (turn == 0) whiteScore += Tables.Knights.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackKnight)
            {
                blackScore += KnightValue;
                //if (turn == 1) blackScore -= Tables.Knights.GetBlackSquareWeight(i);
            }
            else if (piece == MoveGenerator.whiteBishop)
            {
                whiteScore += BishopValue;
                //if (turn == 0) whiteScore += Tables.Bishops.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackBishop)
            {
                blackScore += BishopValue;
                //if (turn == 1) blackScore -= Tables.Bishops.GetBlackSquareWeight(i);
            }
            else if (piece == MoveGenerator.whiteRook)
            {
                whiteScore += RookValue;
                //if (turn == 0) whiteScore += Tables.Rooks.GetWhiteSquareWeight(i);
            }
            else if (piece == MoveGenerator.blackRook)
            {
                blackScore += RookValue;
                //if (turn == 1) blackScore -= Tables.Rooks.GetBlackSquareWeight(i);
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
                if (turn == 0) whiteScore += Tables.Kings.GetBlackSquareWeight(board, i);
            }
            else if (piece == MoveGenerator.blackKing)
            {
                blackScore += KingValue;
                if (turn == 1) blackScore -= Tables.Kings.GetBlackSquareWeight(board, i);
            }
        }


        return (turn == 0) ? (whiteScore - blackScore)
                           : (blackScore - whiteScore);
    }
}
