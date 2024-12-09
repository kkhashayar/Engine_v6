using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Kings
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

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
                        pieceType = 99,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            //////////// Short CASTLE //////////// 
            if (board[60] == 99 && Globals.WhiteShortCastle && board[61] == 0 && board[62] == 0
                && board[63] == 5 && Globals.WhiteKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 99,
                    StartSquare = 60,
                    EndSquare = 62,
                    ShortCastle = true
                });
            }

            //////////// LONG CASTLE //////////// 
            if (board[60] == 99 && Globals.WhiteLongCastle && board[59] == 0 && board[58] == 0
                && board[56] == 5 && Globals.WhiteQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 99,
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
                        pieceType = 109,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }

            if (board[4] == 109 && Globals.BlackShortCastle && board[5] == 0 && board[6] == 0
                && board[7] == 15 && Globals.BlackKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 109,
                    StartSquare = 4,
                    EndSquare = 6,
                    ShortCastle = true
                });
            }

            if (board[4] == 109 && Globals.BlackLongCastle && board[3] == 0 && board[2] == 0
                && board[0] == 15 && Globals.BlackQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 109,
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

        ulong attacks = GetKingAttacks(square);

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
                pieceType = turn == 0 ? 99 : 109,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        // Handle castling moves
        if (turn == 0) // White's turn
        {
            // Short castle
            if (board[60] == 99 && Globals.WhiteShortCastle && board[61] == 0 && board[62] == 0
                && board[63] == 5 && Globals.WhiteKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 99,
                    StartSquare = 60,
                    EndSquare = 62,
                    ShortCastle = true
                });
            }

            // Long castle
            if (board[60] == 99 && Globals.WhiteLongCastle && board[59] == 0 && board[58] == 0
                && board[56] == 5 && Globals.WhiteQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 99,
                    StartSquare = 60,
                    EndSquare = 58,
                    LongCastle = true
                });
            }
        }
        else if (turn == 1) // Black's turn
        {
            // Short castle
            if (board[4] == 109 && Globals.BlackShortCastle && board[5] == 0 && board[6] == 0
                && board[7] == 15 && Globals.BlackKingRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 109,
                    StartSquare = 4,
                    EndSquare = 6,
                    ShortCastle = true
                });
            }

            // Long castle
            if (board[4] == 109 && Globals.BlackLongCastle && board[3] == 0 && board[2] == 0
                && board[0] == 15 && Globals.BlackQueenRookMoved is false)
            {
                moves.Add(new MoveObject
                {
                    pieceType = 109,
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

    private static ulong GetKingAttacks(int square)
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
}

