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

        //if (turn == 0)
        //{

        //    if(blackMoveCount == 1) score += 1900;
        //    else if(blackMoveCount == 3) score += 1800;
        //    else if(blackMoveCount == 5) score += 1700; 
        //    else if(blackMoveCount == 7) score += 1600;  
        //    else if(blackMoveCount == 9) score += 1500;  
        //    else if(blackMoveCount == 11) score += 1400;
        //    else if (blackMoveCount == 13) score += 1300;
        //    else if (blackMoveCount == 15) score += 1200;
        //    else if (blackMoveCount == 17) score += 1100;
        //    else if (blackMoveCount == 19) score += 150;
        //    else if (blackMoveCount == 21) score += 125;
        //    else if (blackMoveCount == 23) score += 110;
        //    else if (blackMoveCount == 25) score += 15;
        //    else if (blackMoveCount == 27) score += 12;
        //    else if (blackMoveCount == 29) score += 11;
        //}
        //else
        //{

        //    if (whiteMoveCount == 1) score -= 1900;
        //    else if (whiteMoveCount == 3) score -= 1800;
        //    else if (whiteMoveCount == 5) score -= 1700;
        //    else if (whiteMoveCount == 7) score -= 1600;
        //    else if (whiteMoveCount == 9) score -= 1500;
        //    else if (whiteMoveCount == 11) score -= 1400;
        //    else if (whiteMoveCount == 13) score -= 1300;
        //    else if (whiteMoveCount == 15) score -= 1200;
        //    else if (whiteMoveCount == 17) score -= 1100;
        //    else if (whiteMoveCount == 19) score -= 150;
        //    else if (whiteMoveCount == 21) score -= 125;
        //    else if (whiteMoveCount == 23) score -= 110;
        //    else if (whiteMoveCount == 25) score -= 15;
        //    else if (whiteMoveCount == 27) score -= 12;
        //    else if (whiteMoveCount == 29) score -= 1;
        //}

        //int GetScoreAdjustment(int moveCount, bool isBlackTurn)
        //{
        //    int adjustment = moveCount switch
        //    {
        //        0 => 3000,
        //        1 => 2000,
        //        2 => 1945,
        //        3 => 1940,
        //        4 => 1935,
        //        5 => 1930,
        //        6 => 1925,
        //        7 => 1920,
        //        8 => 1915,
        //        9 => 1910,
        //        13 => 1900,
        //        15 => 1800,
        //        17 => 1700,
        //        19 => 1600,
        //        21 => 1500,
        //        23 => 1400,
        //        25 => 1300,
        //        27 => 1200,
        //        29 => 1100,
        //        31 => 150,
        //        33 => 125,
        //        35 => 110,
        //        37 => 15,
        //        39 => 12,
        //        41 => 11,
        //        _ => 0
        //    };

        //    return isBlackTurn ? adjustment : -adjustment;
        //}

        //// Usage
        //score += GetScoreAdjustment(turn == 0 ? blackMoveCount : whiteMoveCount, turn == 0);


        return score;
    }
}
