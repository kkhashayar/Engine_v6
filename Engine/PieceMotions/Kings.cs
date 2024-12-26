using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Kings
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares =   GetMasksForSquare(square);

        List<MoveObject> moves = new();
        /////////////////////////////////////////// WHITE KING ///////////////////////////////////////////
        if (turn == 0)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);

                if (targetsquareColor == "White") continue;
                else
                {
                    moves.Add(new MoveObject
                    {
                        pieceType = 1000,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            //////////// Short CASTLE //////////// 
            if (board[60] == 1000 && Globals.WhiteShortCastle && board[61] == 0 && board[62] == 0
                && board[63] == 5 && Globals.WhiteKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 1000,
                    StartSquare = 60,
                    EndSquare = 62,
                    ShortCastle = true
                });
            }

            //////////// LONG CASTLE //////////// 
            if (board[60] == 1000 && Globals.WhiteLongCastle && board[59] == 0 && board[58] == 0
                && board[56] == 5 && Globals.WhiteQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 1000,
                    StartSquare = 60,
                    EndSquare = 58,
                    LongCastle = true
                });
            }
            return moves;
        }

        /////////////////////////////////////////// BLACK KING ///////////////////////////////////////////
        else if (turn == 1)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (targetsquareColor == "Black") continue;
                else
                {
                    moves.Add(new MoveObject
                    {
                        pieceType = -1000,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            if (board[4] == -1000 && Globals.BlackShortCastle && board[5] == 0 && board[6] == 0
                && board[7] == -5 && Globals.BlackKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = -1000,
                    StartSquare = 4,
                    EndSquare = 6,
                    ShortCastle = true
                });
            }

            if (board[4] == -1000 && Globals.BlackLongCastle && board[3] == 0 && board[2] == 0
                && board[0] == -5 && Globals.BlackQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = -1000,
                    StartSquare = 4,
                    EndSquare = 2,
                    LongCastle = true
                });
            }
        }
        return moves;
    }

    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        ulong kingBitboard = 1UL << square; // Create a bitboard for the king's position

        //ulong attacks = GetKingAttacks(square);

        ulong attacks = Masks.King[square];

        while (attacks != 0)
        {
            int targetSquare = BitOperations.TrailingZeroCount(attacks);
            attacks &= attacks - 1; // Clear the least significant bit

            // Skip if the target square is not valid
            if (!Globals.IsValidSquare(targetSquare))
                continue;

            var targetSquareColor = Piece.GetColor(board[targetSquare]);
            // Skip if the target square has a piece of the same color
            if ((turn == 0 && targetSquareColor == "White") || (turn == 1 && targetSquareColor == "Black"))
                continue;

            moves.Add(new MoveObject
            {
                pieceType = turn == 0 ? 1000 : -1000,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        // Handle castling moves
        if (turn == 0) // White's turn
        {
            // Short castle
            if (board[60] == 1000 && Globals.WhiteShortCastle && board[61] == 0 && board[62] == 0
                && board[63] == 5 && Globals.WhiteKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 1000,
                    StartSquare = 60,
                    EndSquare = 62,
                    ShortCastle = true
                });
            }

            // Long castle
            if (board[60] == 1000 && Globals.WhiteLongCastle && board[59] == 0 && board[58] == 0
                && board[56] == 5 && Globals.WhiteQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 1000,
                    StartSquare = 60,
                    EndSquare = 58,
                    LongCastle = true
                });
            }
        }
        else if (turn == 1) // Black's turn
        {
            // Short castle
            if (board[4] == -1000 && Globals.BlackShortCastle && board[5] == 0 && board[6] == 0
                && board[7] == -5 && Globals.BlackKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = -1000,
                    StartSquare = 4,
                    EndSquare = 6,
                    ShortCastle = true
                });
            }

            // Long castle
            if (board[4] == -1000 && Globals.BlackLongCastle && board[3] == 0 && board[2] == 0
                && board[0] == -5 && Globals.BlackQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = -1000,
                    StartSquare = 4,
                    EndSquare = 2,
                    LongCastle = true
                });
            }
        }

        return moves;
    }

    static List<int> GetMasksForSquare(int square)
    {
        List<int> squares = new();

        int[] kingdirections = new int[8] { 9, 8, 7, 1, -9, -7, -8, -1 };

        int originalrank = square / 8;
        int originalfile = square % 8;

        foreach (int direction in kingdirections)
        {
            int dessquare = square + direction;

            // calculate rank and file after the move
            int newrank = dessquare / 8;
            int newfile = dessquare % 8;

            // check if move is within one rank/file step from the original position
            if (Math.Abs(newrank - originalrank) <= 1 && Math.Abs(newfile - originalfile) <= 1)
            {
                if (Globals.IsValidSquare(dessquare))
                {
                    squares.Add(dessquare);
                }
            }
        }

        return squares;
    }

    public static ulong GetKingAttacks(int square)
    {
        ulong attacks = 0;
        int[] KingDirections = new int[8] { 9, 8, 7, 1, -9, -7, -8, -1 };

        foreach (int direction in KingDirections)
        {
            int targetSquare = square + direction;
            if (Globals.IsValidSquare(targetSquare) && IsWithinOneStep(square, targetSquare))
            {
                attacks |= (1UL << targetSquare);
            }
        }

        return attacks;
    }

    private static bool IsWithinOneStep(int originalSquare, int targetSquare)
    {
        int originalRank = originalSquare / 8;
        int originalFile = originalSquare % 8;
        int targetRank = targetSquare / 8;
        int targetFile = targetSquare % 8;

        return Math.Abs(targetRank - originalRank) <= 1 && Math.Abs(targetFile - originalFile) <= 1;
    }

    public static readonly int[][] KingMoves = new int[][]
{
    // Rank 8 (Squares 0–7)
    new int[]{1,8,9},           //0  (a8)
    new int[]{0,2,8,9,10},      //1  (b8)
    new int[]{1,3,9,10,11},     //2  (c8)
    new int[]{2,4,10,11,12},    //3  (d8)
    new int[]{3,5,11,12,13},    //4  (e8)
    new int[]{4,6,12,13,14},    //5  (f8)
    new int[]{5,7,13,14,15},    //6  (g8)
    new int[]{6,14,15},         //7  (h8)

    // Rank 7 (Squares 8–15)
    new int[]{0,1,9,16,17},        //8  (a7)
    new int[]{0,1,2,8,10,16,17,18},//9  (b7)
    new int[]{1,2,3,9,11,17,18,19},//10 (c7)
    new int[]{2,3,4,10,12,18,19,20},//11(d7)
    new int[]{3,4,5,11,13,19,20,21},//12(e7)
    new int[]{4,5,6,12,14,20,21,22},//13(f7)
    new int[]{5,6,7,13,15,21,22,23},//14(g7)
    new int[]{6,7,14,22,23},       //15(h7)

    // Rank 6 (Squares 16–23)
    new int[]{8,9,17,24,25},          //16(a6)
    new int[]{8,9,10,16,18,24,25,26}, //17(b6)
    new int[]{9,10,11,17,19,25,26,27},//18(c6)
    new int[]{10,11,12,18,20,26,27,28},//19(d6)
    new int[]{11,12,13,19,21,27,28,29},//20(e6)
    new int[]{12,13,14,20,22,28,29,30},//21(f6)
    new int[]{13,14,15,21,23,29,30,31},//22(g6)
    new int[]{14,15,22,30,31},        //23(h6)

    // Rank 5 (Squares 24–31)
    new int[]{16,17,25,32,33},          //24(a5)
    new int[]{16,17,18,24,26,32,33,34}, //25(b5)
    new int[]{17,18,19,25,27,33,34,35}, //26(c5)
    new int[]{18,19,20,26,28,34,35,36}, //27(d5)
    new int[]{19,20,21,27,29,35,36,37}, //28(e5)
    new int[]{20,21,22,28,30,36,37,38}, //29(f5)
    new int[]{21,22,23,29,31,37,38,39}, //30(g5)
    new int[]{22,23,30,38,39},          //31(h5)

    // Rank 4 (Squares 32–39)
    new int[]{24,25,33,40,41},          //32(a4)
    new int[]{24,25,26,32,34,40,41,42}, //33(b4)
    new int[]{25,26,27,33,35,41,42,43}, //34(c4)
    new int[]{26,27,28,34,36,42,43,44}, //35(d4)
    new int[]{27,28,29,35,37,43,44,45}, //36(e4)
    new int[]{28,29,30,36,38,44,45,46}, //37(f4)
    new int[]{29,30,31,37,39,45,46,47}, //38(g4)
    new int[]{30,31,38,46,47},          //39(h4)

    // Rank 3 (Squares 40–47)
    new int[]{32,33,41,48,49},          //40(a3)
    new int[]{32,33,34,40,42,48,49,50}, //41(b3)
    new int[]{33,34,35,41,43,49,50,51}, //42(c3)
    new int[]{34,35,36,42,44,50,51,52}, //43(d3)
    new int[]{35,36,37,43,45,51,52,53}, //44(e3)
    new int[]{36,37,38,44,46,52,53,54}, //45(f3)
    new int[]{37,38,39,45,47,53,54,55}, //46(g3)
    new int[]{38,39,46,54,55},          //47(h3)

    // Rank 2 (Squares 48–55)
    new int[]{40,41,49,56,57},          //48(a2)
    new int[]{40,41,42,48,50,56,57,58}, //49(b2)
    new int[]{41,42,43,49,51,57,58,59}, //50(c2)
    new int[]{42,43,44,50,52,58,59,60}, //51(d2)
    new int[]{43,44,45,51,53,59,60,61}, //52(e2)
    new int[]{44,45,46,52,54,60,61,62}, //53(f2)
    new int[]{45,46,47,53,55,61,62,63}, //54(g2)
    new int[]{46,47,54,62,63},          //55(h2)

    // Rank 1 (Squares 56–63)
    new int[]{48,49,57},        //56(a1)
    new int[]{48,49,50,56,58},  //57(b1)
    new int[]{49,50,51,57,59},  //58(c1)
    new int[]{50,51,52,58,60},  //59(d1)
    new int[]{51,52,53,59,61},  //60(e1)
    new int[]{52,53,54,60,62},  //61(f1)
    new int[]{53,54,55,61,63},  //62(g1)
    new int[]{54,55,62}         //63(h1)
};

}

