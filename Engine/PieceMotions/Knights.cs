
using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Knights
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);
        List<MoveObject> moves = new();

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
                        pieceType = MoveGenerator.whiteKnight,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }
            return moves;
        }
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
                        pieceType = MoveGenerator.blackKnight,
                        StartSquare = square,
                        EndSquare = targetSquare
                    });
                }
            }
        }
        return moves;
    }

    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        ulong knightBitboard = 1UL << square; // Create a bitboard for the knight's position

        ulong attacks = GetKnightAttacks(square);

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
                pieceType = turn == 0 ? MoveGenerator.whiteKnight : MoveGenerator.blackKnight,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        return moves;
    }

    private static ulong GetKnightAttacks(int square)
    {
        ulong attacks = 0;
        int originalRow = square / 8;
        int originalCol = square % 8;

        // 8 possible moves for a knight at a given square
        (int, int)[] KnightMoves = {(2, 1), (1, 2), (-1, 2), (-2, 1),
                                    (-2, -1), (-1, -2), (1, -2), (2, -1)};

        foreach (var (rowChange, colChange) in KnightMoves)
        {
            int newRow = originalRow + rowChange;  // New row after making the move
            int newCol = originalCol + colChange;  // New column after making the move

            // Check if the new position is still on the board
            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                int targetSquare = newRow * 8 + newCol;
                if (Globals.IsValidSquare(targetSquare))
                {
                    attacks |= (1UL << targetSquare);
                }
            }
        }

        return attacks;
    }

    public static List<int> GetMasksForSquare(int square)
    {
        // 8 possible moves for a knight at given square
        (int, int)[] KnightMoves = {(2, 1), (1, 2), (-1, 2), (-2, 1),
                                    (-2, -1), (-1, -2), (1, -2), (2, -1)};

        List<int> moves = new List<int>();
        int originalRow = square / 8;  // Calculating the row of the square
        int originalCol = square % 8;  // Calculating the file of the square

        foreach (var (rowChange, colChange) in KnightMoves)
        {
            int newRow = originalRow + rowChange;  // New row after making the move
            int newCol = originalCol + colChange;  // New column after making the move

            // Check if the new position is still on the board
            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                int targetSquare = newRow * 8 + newCol;
                if (Globals.IsValidSquare(targetSquare)) moves.Add(targetSquare);
            }
        }

        return moves;
    }
}
