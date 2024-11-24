using Engine.Core;
using Engine.Tables;

namespace Engine;
internal static class Evaluators
{
    public static int GetByMaterial(int[] chessBoard, int turn)
    {
        int whitePieces = chessBoard.Where(p => Piece.IsWhite(p)).Count();
        int blackPieces = chessBoard.Where(p => Piece.IsBlack(p)).Count();
        int totalPiecesOnTheBoard = whitePieces + blackPieces;

        var gamePhase = "";
        if (totalPiecesOnTheBoard == 32) gamePhase = "Opening";
        else if (totalPiecesOnTheBoard < 32 && totalPiecesOnTheBoard > 10) gamePhase = "MiddleGame";
        else gamePhase = "EndGame";

        int whiteMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 0, true).Count;
        int blackMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 1, true).Count;
        int score = 0;
        for (int i = 0; i < 64; i++)
        {
            int piece = chessBoard[i];

            switch (piece)
            {
                case 1:
                    score += 1;
                    if (gamePhase == "Opening") score += Pawns.GetSquareWeight(i, true);
                    break;
                case 3:
                    score += 3;
                    if (gamePhase == "Opening") score += Knights.GetSquareWeight(i, true);
                    break;
                case 4:
                    score += 3;
                    if (gamePhase == "Opening") score += Bishops.GetSquareWeight(i, true);
                    break;
                case 5:
                    score += 5;
                    break;
                case 9:
                    score += 9;
                    break;
                case 99:
                    score += 999999;
                    if (gamePhase == "EndGame") score += Kings.GetEndGameWeight(i, true);
                    else if (gamePhase == "Openning" || gamePhase == "MiddleGame") score += Kings.GetMiddleGameWeight(i, true);
                    break;
                case 11:
                    score -= 1;
                    if (gamePhase == "Opening") score -= Pawns.GetSquareWeight(i, false);
                    break;
                case 13:
                    score -= 3;
                    if (gamePhase == "Opening") score -= Knights.GetSquareWeight(i, false);
                    break;
                case 14:
                    score -= 3;
                    if (gamePhase == "Opening") score -= Bishops.GetSquareWeight(i, false);
                    break;
                case 15:
                    score -= 5;
                    break;
                case 19:
                    score -= 9;
                    break;
                case 109:
                    score -= 999999;
                    if (gamePhase == "EndGame") score -= Kings.GetEndGameWeight(i, false);
                    else if (gamePhase == "Openning" || gamePhase == "MiddleGame") score -= Kings.GetMiddleGameWeight(i, true);
                    break;
                default:
                    break;
            }
        }
        //if (gamePhase == "Oppening" || gamePhase == "EndGame")
        //{
            if (turn == 0)
            {

                score += blackMoveCount switch
                {
                    0 => 400,
                    1 => 400,
                    2 => 400,
                    3 => 400,
                    4 => 400,
                    5 => 400,
                    6 => 400,
                    7 => 400,
                    8 => 400,
                    9 => 400,
                    11 => 170,
                    13 => 150,
                    15 => 130,
                    17 => 110,
                    19 => 100,
                    21 => 90,
                    23 => 80,
                    25 => 70,
                    27 => 60,
                    29 => 50,
                    31 => 40,
                    33 => 30,
                    35 => 20,
                    _ => 0
                };
            }
            else
            {
                score -= whiteMoveCount switch
                {
                    0 => 400,
                    1 => 400,
                    2 => 400,
                    3 => 400,
                    4 => 400,
                    5 => 400,
                    6 => 400,
                    7 => 400,
                    8 => 400,
                    9 => 400,
                    11 => 170,
                    13 => 150,
                    15 => 130,
                    17 => 110,
                    19 => 100,
                    21 => 90,
                    23 => 80,
                    25 => 70,
                    27 => 60,
                    29 => 50,
                    31 => 40,
                    33 => 30,
                    35 => 20,
                    _ => 0
                };
            }
        //}

        return score;
    }

    public static bool IsPositionDraw(int turn, int numberOfWhiteMoves, int numberOfBlackMoves)
    {
        if (turn == 0)
        {
            if (numberOfBlackMoves == 0) return true;
        }
        if (numberOfWhiteMoves == 0) return true;
        return false;
    }
}
