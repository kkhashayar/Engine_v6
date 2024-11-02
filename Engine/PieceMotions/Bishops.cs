using Engine.Core;
using System.Numerics;

namespace Engine.PieceMotions;

internal static class Bishops
{
    // Lets try to compress the code even the fact I don't like it 
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);
        List<MoveObject> moves = new List<MoveObject>();

        foreach (int targetSquare in targetSquares)
        {
            var targetSquareColor = Piece.GetColor(board[targetSquare]);
            if (Globals.IsDiagonalPathClear(square, targetSquare, board))
            {
                // Skip if the target square has a piece of the same color
                if (turn == 0 && targetSquareColor == "White" || turn == 1 && targetSquareColor == "Black")
                    continue;

                moves.Add(new MoveObject
                {
                    pieceType = turn == 0 ? MoveGenerator.whiteBishop : MoveGenerator.blackBishop,
                    StartSquare = square,
                    EndSquare = targetSquare
                });
            }
        }

        return moves;
    }

    public static List<int> GetMasksForSquare(int square)
    {
        List<int> squares = new List<int>();
        int[] directions = { 7, 9, -7, -9 }; //  NorthEast, NorthWest, SouthEast, SouthWest
        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && !Globals.IsDiagBreaksMask(currentSquare, direction, originalRank, originalFile))
            {
                squares.Add(currentSquare);
                currentSquare += direction;
            }
        }

        return squares;
    }

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
                pieceType = turn == 0 ? MoveGenerator.whiteBishop : MoveGenerator.blackBishop,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        return moves;
    }

    private static ulong GetBishopAttacks(int square, int[] board)
    {
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
