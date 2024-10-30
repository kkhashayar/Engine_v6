using System;

namespace Engine;

internal static class Evaluators
{
    public static decimal GetByMaterial(int[] chessBoard, int turn)
    {
        int whiteMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 0, false).Count;
        int blackMoveCount = MoveGenerator.GenerateAllMoves(chessBoard, 1, false).Count;
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
                    score += 1000;
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
                    score -= 1000;
                    break;
                default:
                    break;
            }
        }

        if (turn == 0)
        {
            if(blackMoveCount == 0) score += 1000;  
            else if(blackMoveCount == 1) score += 900;
            else if(blackMoveCount == 2) score += 800;
            else if(blackMoveCount == 3) score += 700; 
            else if(blackMoveCount == 4) score += 600;  
            else if(blackMoveCount == 5) score += 500;  
            else if(blackMoveCount == 6) score += 400;
            else if (blackMoveCount == 7) score += 300;
            else if (blackMoveCount == 8) score += 200;
            else if (blackMoveCount == 9) score += 100;
        }
        else
        {
            if(whiteMoveCount == 0) score -= 1000;
            else if (whiteMoveCount == 1) score -= 900;
            else if (whiteMoveCount == 2) score -= 800;
            else if (whiteMoveCount == 3) score -= 700;
            else if (whiteMoveCount == 4) score -= 600;
            else if (whiteMoveCount == 5) score -= 500;
            else if (whiteMoveCount == 6) score -= 400;
            else if (whiteMoveCount == 7) score -= 300;
            else if (whiteMoveCount == 8) score -= 200;
            else if (whiteMoveCount == 9) score -= 100;
        }

        return score;
    }
}
