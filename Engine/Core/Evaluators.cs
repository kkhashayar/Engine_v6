using System;
using System.Reflection.Metadata.Ecma335;

namespace Engine;

internal static class Evaluators
{
    public static decimal GetByMaterial(int[] chessBoard, int turn)
    {
        int whiteMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 0, true).Count;
        int blackMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 1, true).Count;
        decimal score = 0;
        for (int i = 0; i < 64; i++)
        {
            int piece = chessBoard[i];

            switch (piece)
            {
                case 1:
                    score += 1;
                    break;
                case 3:
                    score += 3;
                    break;
                case 4:
                    score += 3.2m;
                    break;
                case 5:
                    score += 5;
                    break;
                case 9:
                    score += 9;
                    break;
                case 99:
                    score += 0.5m;
                    break;
                case 11:
                    score -= 1;
                    break;
                case 13:
                    score -= 3;
                    break;
                case 14:
                    score -= 3.2m;
                    break;
                case 15:
                    score -= 5;
                    break;
                case 19:
                    score -= 9;
                    break;
                case 109:
                    score -= 0.5m;
                    break;
                default:
                    break;
            }
        }

        if (turn == 0)
        {
            if (blackMoveCount == 0) score += 3000;
            if (blackMoveCount == 1) score += 1900;
            else if (blackMoveCount == 9) score += 1800;
            else if (blackMoveCount == 11) score += 1700;
            else if (blackMoveCount == 13) score += 1600;
            else if (blackMoveCount == 15) score += 1500;
            else if (blackMoveCount == 17) score += 1400;
            else if (blackMoveCount == 19) score += 1300;
            else if (blackMoveCount == 21) score += 1200;
            else if (blackMoveCount == 23) score += 1100;
            else if (blackMoveCount == 25) score += 150;
            else if (blackMoveCount == 27) score += 125;
            else if (blackMoveCount == 29) score += 110;
            else if (blackMoveCount == 31) score += 15;
            else if (blackMoveCount == 33) score += 12;
            else if (blackMoveCount == 35) score += 11;

        }
        else
        {
            if (whiteMoveCount == 0) score -= 3000;
            if (whiteMoveCount == 1) score -= 1900;
            else if (whiteMoveCount == 9) score -= 1800;
            else if (whiteMoveCount == 11) score -= 1700;
            else if (whiteMoveCount == 13) score -= 1600;
            else if (whiteMoveCount == 15) score -= 1500;
            else if (whiteMoveCount == 17) score -= 1400;
            else if (whiteMoveCount == 19) score -= 1300;
            else if (whiteMoveCount == 21) score -= 1200;
            else if (whiteMoveCount == 23) score -= 1100;
            else if (whiteMoveCount == 25) score -= 150;
            else if (whiteMoveCount == 27) score -= 125;
            else if (whiteMoveCount == 29) score -= 110;
            else if (whiteMoveCount == 31) score -= 15;
            else if (whiteMoveCount == 33) score -= 12;
            else if (whiteMoveCount == 35) score -= 1;
        }

        return score;
    }
}
