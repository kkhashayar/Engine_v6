using Engine.Tables;

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
                    // if(turn == 0) score += Pawns.GetSquareWeight(i, true);    
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
                    score += 999999;
                    break;
                case 11:
                    score -= 1;
                    //if(turn == 1) score -= Pawns.GetSquareWeight(i, false);   
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
                    score -= 999999;
                    break;
                default:
                    break;
            }
        }

        // Mobility-based adjustment
        //int moveCount = (turn == 0) ? blackMoveCount : whiteMoveCount;
        //int MobilityWeight = 10; // Adjust this value as needed
        //int adjustment = moveCount * MobilityWeight;

        //score += (turn == 0) ? adjustment : -adjustment;

        if (turn == 0)
        {
            score += blackMoveCount switch
            {
                0 => 3000,
                1 => 2900,
                2 => 2890,
                3 => 2880,
                4 => 2870,
                5 => 2860,
                6 => 2850,
                7 => 2840,
                8 => 2830,
                9 => 2800,
                11 => 1700,
                13 => 1600,
                15 => 1500,
                17 => 1400,
                19 => 1300,
                21 => 1200,
                23 => 1100,
                25 => 150,
                27 => 125,
                29 => 110,
                31 => 15,
                33 => 12,
                35 => 11,
                _ => 0
            };
        }
        else
        {
            score -= whiteMoveCount switch
            {
                0 => 3000,
                1 => 2900,
                2 => 2890,
                3 => 2880,
                4 => 2870,
                5 => 2860,
                6 => 2850,
                7 => 2840,
                8 => 2830,
                9 => 2800,
                11 => 1700,
                13 => 1600,
                15 => 1500,
                17 => 1400,
                19 => 1300,
                21 => 1200,
                23 => 1100,
                25 => 150,
                27 => 125,
                29 => 110,
                31 => 15,
                33 => 12,
                35 => 1,
                _ => 0
            };
        }

        return score;
    }
}
