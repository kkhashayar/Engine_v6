using Engine.Core;

namespace Engine.PieceMotions;

internal static class Kings
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = new();  
        
        
        int whiteKingPosition = Globals.GetWhiteKingSquare(board);
        int blacKingPosition = Globals.GetBlackKingSquare(board);

        List<int> whiteTargetSquares = GetKingMovesFromTable(whiteKingPosition);
        List<int> blackTargetSquares = GetKingMovesFromTable(blacKingPosition);
       
        // Filtering opposite kings 
        if(turn == 0)
        {
            whiteTargetSquares = whiteTargetSquares.Except(blackTargetSquares).ToList();
            targetSquares = whiteTargetSquares;
        }

        else if(turn == 1)
        {
            blackTargetSquares = blackTargetSquares.Except(whiteTargetSquares).ToList();
            targetSquares = blackTargetSquares;
        }



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
                        pieceType = MoveGenerator.whiteKing,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            //////////// Short CASTLE //////////// 
            if (board[60] == MoveGenerator.whiteKing && Globals.WhiteShortCastle && board[61] == 0 && board[62] == 0
                && board[63] == MoveGenerator.whiteRook && Globals.WhiteKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.whiteKing,
                    StartSquare = 60,
                    EndSquare = 62,
                    ShortCastle = true
                });
            }

            //////////// LONG CASTLE //////////// 
            if (board[60] == MoveGenerator.whiteKing && Globals.WhiteLongCastle && board[59] == 0 && board[58] == 0
                && board[56] == MoveGenerator.whiteRook && Globals.WhiteQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.whiteKing,
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
                        pieceType = MoveGenerator.blackKing,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            if (board[4] == MoveGenerator.blackKing && Globals.BlackShortCastle && board[5] == 0 && board[6] == 0
                && board[7] == MoveGenerator.blackRook && Globals.BlackKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.blackKing,
                    StartSquare = 4,
                    EndSquare = 6,
                    ShortCastle = true
                });
            }


            if (board[4] == MoveGenerator.blackKing && Globals.BlackLongCastle && board[3] == 0 && board[2] == 0
                && board[0] == MoveGenerator.blackRook && Globals.BlackQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.blackKing,
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

        int[] KingDirections = new int[8] { 9, 8, 7, 1, -9, -7, -8, -1 };

        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in KingDirections)
        {
            int desSquare = square + direction;

            // Calculate rank and file after the move
            int newRank = desSquare / 8;
            int newFile = desSquare % 8;

            // Check if move is within one rank/file step from the original position
            if (Math.Abs(newRank - originalRank) <= 1 && Math.Abs(newFile - originalFile) <= 1)
            {
                if (Globals.IsValidSquare(desSquare))
                {
                    squares.Add(desSquare);
                }
            }
        }

        return squares;
    }

    public static List<int> GetKingMovesFromTable(int squareIndex)
    {
        return KingMovesTable[squareIndex].ToList();
    }

    private static readonly int[][] KingMovesTable = new int[64][]
    {
        new int[] { 1, 8, 9 },           // a8 (index 0) - can move to b8, a7, b7
        new int[] { 0, 2, 8, 9, 10 },    // b8 (index 1) - can move to a8, c8, a7, b7, c7
        new int[] { 1, 3, 9, 10, 11 },   // c8 (index 2) - can move to b8, d8, b7, c7, d7
        new int[] { 2, 4, 10, 11, 12 },  // d8 (index 3) - can move to c8, e8, c7, d7, e7
        new int[] { 3, 5, 11, 12, 13 },  // e8 (index 4) - can move to d8, f8, d7, e7, f7
        new int[] { 4, 6, 12, 13, 14 },  // f8 (index 5) - can move to e8, g8, e7, f7, g7
        new int[] { 5, 7, 13, 14, 15 },  // g8 (index 6) - can move to f8, h8, f7, g7, h7
        new int[] { 6, 14, 15 },         // h8 (index 7) - can move to g8, g7, h7

        new int[] { 0, 1, 9, 16, 17 },   // a7 (index 8) - can move to a8, b8, b7, a6, b6
        new int[] { 0, 1, 2, 8, 10, 16, 17, 18 }, // b7 (index 9) - can move to a8, b8, c8, a7, c7, a6, b6, c6
        new int[] { 1, 2, 3, 9, 11, 17, 18, 19 }, // c7 (index 10) - can move to b8, c8, d8, b7, d7, b6, c6, d6
        new int[] { 2, 3, 4, 10, 12, 18, 19, 20 }, // d7 (index 11) - can move to c8, d8, e8, c7, e7, c6, d6, e6
        new int[] { 3, 4, 5, 11, 13, 19, 20, 21 }, // e7 (index 12) - can move to d8, e8, f8, d7, f7, d6, e6, f6
        new int[] { 4, 5, 6, 12, 14, 20, 21, 22 }, // f7 (index 13) - can move to e8, f8, g8, e7, g7, e6, f6, g6
        new int[] { 5, 6, 7, 13, 15, 21, 22, 23 }, // g7 (index 14) - can move to f8, g8, h8, f7, h7, f6, g6, h6
        new int[] { 6, 7, 14, 22, 23 },  // h7 (index 15) - can move to g8, g7, h7, g6, h6

        new int[] { 8, 9, 17, 24, 25 },  // a6 (index 16) - can move to a7, b7, b6, a5, b5
        new int[] { 8, 9, 10, 16, 18, 24, 25, 26 }, // b6 (index 17) - can move to a7, b7, c7, a6, c6, a5, b5, c5
        new int[] { 9, 10, 11, 17, 19, 25, 26, 27 }, // c6 (index 18) - can move to b7, c7, d7, b6, d6, b5, c5, d5
        new int[] { 10, 11, 12, 18, 20, 26, 27, 28 }, // d6 (index 19) - can move to c7, d7, e7, c6, e6, c5, d5, e5
        new int[] { 11, 12, 13, 19, 21, 27, 28, 29 }, // e6 (index 20) - can move to d7, e7, f7, d6, f6, d5, e5, f5
        new int[] { 12, 13, 14, 20, 22, 28, 29, 30 }, // f6 (index 21) - can move to e7, f7, g7, e6, g6, e5, f5, g5
        new int[] { 13, 14, 15, 21, 23, 29, 30, 31 }, // g6 (index 22) - can move to f7, g7, h7, f6, h6, f5, g5, h5
        new int[] { 14, 15, 22, 30, 31 }, // h6 (index 23) - can move to g7, g6, h6, g5, h5

        new int[] { 16, 17, 25, 32, 33 }, // a5 (index 24) - can move to a6, b6, b5, a4, b4
        new int[] { 16, 17, 18, 24, 26, 32, 33, 34 }, // b5 (index 25) - can move to a6, b6, c6, a5, c5, a4, b4, c4
        new int[] { 17, 18, 19, 25, 27, 33, 34, 35 }, // c5 (index 26) - can move to b6, c6, d6, b5, d5, b4, c4, d4
        new int[] { 18, 19, 20, 26, 28, 34, 35, 36 }, // d5 (index 27) - can move to c6, d6, e6, c5, e5, c4, d4, e4
        new int[] { 19, 20, 21, 27, 29, 35, 36, 37 }, // e5 (index 28) - can move to d6, e6, f6, d5, f5, d4, e4, f4
        new int[] { 20, 21, 22, 28, 30, 36, 37, 38 }, // f5 (index 29) - can move to e6, f6, g6, e5, g5, e4, f4, g4
        new int[] { 21, 22, 23, 29, 31, 37, 38, 39 }, // g5 (index 30) - can move to f6, g6, h6, f5, h5, f4, g4, h4
        new int[] { 22, 23, 30, 38, 39 }, // h5 (index 31) - can move to g6, g5, h5, g4, h4

        new int[] { 24, 25, 33, 40, 41 }, // a4 (index 32) - can move to a5, b5, b4, a3, b3
        new int[] { 24, 25, 26, 32, 34, 40, 41, 42 }, // b4 (index 33) - can move to a5, b5, c5, a4, c4, a3, b3, c3
        new int[] { 25, 26, 27, 33, 35, 41, 42, 43 }, // c4 (index 34) - can move to b5, c5, d5, b4, d4, b3, c3, d3
        new int[] { 26, 27, 28, 34, 36, 42, 43, 44 }, // d4 (index 35) - can move to c5, d5, e5, c4, e4, c3, d3, e3
        new int[] { 27, 28, 29, 35, 37, 43, 44, 45 }, // e4 (index 36) - can move to d5, e5, f5, d4, f4, d3, e3, f3
        new int[] { 28, 29, 30, 36, 38, 44, 45, 46 }, // f4 (index 37) - can move to e5, f5, g5, e4, g4, e3, f3, g3
        new int[] { 29, 30, 31, 37, 39, 45, 46, 47 }, // g4 (index 38) - can move to f5, g5, h5, f4, h4, f3, g3, h3
        new int[] { 30, 31, 38, 46, 47 }, // h4 (index 39) - can move to g5, g4, h4, g3, h3

        new int[] { 32, 33, 41, 48, 49 }, // a3 (index 40) - can move to a4, b4, b3, a2, b2
        new int[] { 32, 33, 34, 40, 42, 48, 49, 50 }, // b3 (index 41) - can move to a4, b4, c4, a3, c3, a2, b2, c2
        new int[] { 33, 34, 35, 41, 43, 49, 50, 51 }, // c3 (index 42) - can move to b4, c4, d4, b3, d3, b2, c2, d2
        new int[] { 34, 35, 36, 42, 44, 50, 51, 52 }, // d3 (index 43) - can move to c4, d4, e4, c3, e3, c2, d2, e2
        new int[] { 35, 36, 37, 43, 45, 51, 52, 53 }, // e3 (index 44) - can move to d4, e4, f4, d3, f3, d2, e2, f2
        new int[] { 36, 37, 38, 44, 46, 52, 53, 54 }, // f3 (index 45) - can move to e4, f4, g4, e3, g3, e2, f2, g2
        new int[] { 37, 38, 39, 45, 47, 53, 54, 55 }, // g3 (index 46) - can move to f4, g4, h4, f3, h3, f2, g2, h2
        new int[] { 38, 39, 46, 54, 55 }, // h3 (index 47) - can move to g4, g3, h3, g2, h2

        new int[] { 40, 41, 49, 56, 57 }, // a2 (index 48) - can move to a3, b3, b2, a1, b1
        new int[] { 40, 41, 42, 48, 50, 56, 57, 58 }, // b2 (index 49) - can move to a3, b3, c3, a2, c2, a1, b1, c1
        new int[] { 41, 42, 43, 49, 51, 57, 58, 59 }, // c2 (index 50) - can move to b3, c3, d3, b2, d2, b1, c1, d1
        new int[] { 42, 43, 44, 50, 52, 58, 59, 60 }, // d2 (index 51) - can move to c3, d3, e3, c2, e2, c1, d1, e1
        new int[] { 43, 44, 45, 51, 53, 59, 60, 61 }, // e2 (index 52) - can move to d3, e3, f3, d2, f2, d1, e1, f1
        new int[] { 44, 45, 46, 52, 54, 60, 61, 62 }, // f2 (index 53) - can move to e3, f3, g3, e2, g2, e1, f1, g1
        new int[] { 45, 46, 47, 53, 55, 61, 62, 63 }, // g2 (index 54) - can move to f3, g3, h3, f2, h2, f1, g1, h1
        new int[] { 46, 47, 54, 62, 63 }, // h2 (index 55) - can move to g3, g2, h2, g1, h1

        new int[] { 48, 49, 57 }, // a1 (index 56) - can move to a2, b2, b1
        new int[] { 48, 49, 50, 56, 58 }, // b1 (index 57) - can move to a2, b2, c2, a1, c1
        new int[] { 49, 50, 51, 57, 59 }, // c1 (index 58) - can move to b2, c2, d2, b1, d1
        new int[] { 50, 51, 52, 58, 60 }, // d1 (index 59) - can move to c2, d2, e2, c1, e1
        new int[] { 51, 52, 53, 59, 61 }, // e1 (index 60) - can move to d2, e2, f2, d1, f1
        new int[] { 52, 53, 54, 60, 62 }, // f1 (index 61) - can move to e2, f2, g2, e1, g1
        new int[] { 53, 54, 55, 61, 63 }, // g1 (index 62) - can move to f2, g2, h2, f1, h1
        new int[] { 54, 55, 62 } // h1 (index 63) - can move to g2, g1, h1
    };

}

