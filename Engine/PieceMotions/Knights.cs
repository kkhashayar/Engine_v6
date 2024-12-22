
using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Knights
{

    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        ulong knightBitboard = 1UL << square; // Create a bitboard for the knight's position

        //ulong attacks = GetKnightAttacks(square);

        ulong attacks = Masks.Knight[square];

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
                pieceType = turn == 0 ? 3 : -3,
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

}
