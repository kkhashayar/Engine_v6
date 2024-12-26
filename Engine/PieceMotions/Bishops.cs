using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Bishops
{
    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        ulong bishopBitboard = 1UL << square; // Create a bitboard for the bishop's position

        ulong attacks = GetBishopAttacks(square, board);

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
                pieceType = turn == 0 ? 4 : -4,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        return moves;
    }

    private static ulong GetBishopAttacks(int square, int[] board)
    {
        List<int> WhiteSquares = new List<int> { 0, 2, 4, 6, 9, 11, 13, 15, 16, 18, 20, 22, 25, 27, 29, 31, 32, 34, 36, 38, 41, 43, 45, 47, 48, 50, 52, 54, 57, 59, 61, };
        List<int> BlackSquares = new List<int> { 1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23, 24, 26, 28, 30, 33, 35, 37, 39, 40, 42, 44, 46, 49, 51, 53, 55, 56, 58, 60, 62 };
        ulong attacks = 0;
        int[] directions = { 7, 9, -7, -9 }; // NorthEast, NorthWest, SouthEast, SouthWest

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && IsSameDiagonal(square, currentSquare))
            {
                // Add the current square to attacks
                attacks |= (1UL << currentSquare);

                // Stop if there's a piece in the current square
                if (board[currentSquare] != 0)
                    break;

                currentSquare += direction;
            }
        }

        return attacks;

     
    }

    

    private static bool IsSameDiagonal(int square1, int square2)
    {
        int rankDifference = Math.Abs((square1 / 8) - (square2 / 8));
        int fileDifference = Math.Abs((square1 % 8) - (square2 % 8));
        return rankDifference == fileDifference;
    }


}
