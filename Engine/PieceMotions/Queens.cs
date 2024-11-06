using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Queens
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        List<int> targetSquares = GetDiagonalAndCrossMasksForSquare(square);

        foreach (int targetSquare in targetSquares)
        {
            var targetSquareColor = Piece.GetColor(board[targetSquare]);
            bool isDiagonalMove = Math.Abs(targetSquare / 8 - square / 8) == Math.Abs(targetSquare % 8 - square % 8);

            if (isDiagonalMove && Globals.IsDiagonalPathClear(square, targetSquare, board) ||
                !isDiagonalMove && Globals.IsCrossSliderPathClear(square, targetSquare, board))
            {
                // Skip if the target square has a piece of the same color
                if (turn == 0 && targetSquareColor == "White" || turn == 1 && targetSquareColor == "Black")
                    continue;

                moves.Add(new MoveObject
                {
                    pieceType = turn == 0 ? MoveGenerator.whiteQueen : MoveGenerator.blackQueen,
                    StartSquare = square,
                    EndSquare = targetSquare
                });
            }
        }

        return moves;
    }

    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        ulong queenBitboard = 1UL << square; // Create a bitboard for the queen's position

        ulong attacks = GetQueenAttacks(square, board);

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
                pieceType = turn == 0 ? MoveGenerator.whiteQueen : MoveGenerator.blackQueen,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        return moves;
    }

    private static ulong GetQueenAttacks(int square, int[] board)
    {
        ulong attacks = 0;
        int[] directions = { 7, 9, -7, -9, 1, -1, 8, -8 }; // Diagonal and cross directions

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && IsValidDirection(square, currentSquare, direction))
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

    private static List<int> GetDiagonalAndCrossMasksForSquare(int square)
    {
        List<int> squares = new List<int>();
        int[] directions = { 7, 9, -7, -9, 1, -1, 8, -8 }; // Diagonal and cross directions
        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) &&
                   !BreaksMask(currentSquare, direction, originalRank, originalFile))
            {
                squares.Add(currentSquare);
                currentSquare += direction;
            }
        }

        return squares;
    }

    private static bool IsValidDirection(int originalSquare, int targetSquare, int direction)
    {
        int originalRank = originalSquare / 8;
        int originalFile = originalSquare % 8;
        int targetRank = targetSquare / 8;
        int targetFile = targetSquare % 8;

        // Ensure that the move is consistent along the direction
        if (Math.Abs(direction) == 7 || Math.Abs(direction) == 9)
        {
            return Math.Abs(targetRank - originalRank) == Math.Abs(targetFile - originalFile);
        }
        else
        {
            return originalRank == targetRank || originalFile == targetFile;
        }
    }

    private static bool BreaksMask(int square, int direction, int originalRank, int originalFile)
    {
        int newRank = square / 8;
        int newFile = square % 8;

        // Check for diagonal movement
        if (Math.Abs(direction) == 7 || Math.Abs(direction) == 9)
        {
            if (Globals.IsDiagBreaksMask(square, direction, originalRank, originalFile))
                return true;
        }
        // Check for vertical or horizontal movement
        else
        {
            if (Globals.IsVerHorBreaksMask(square, direction, originalRank, originalFile))
                return true;
        }

        return false;
    }
}
