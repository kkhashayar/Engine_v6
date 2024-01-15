namespace Engine;

internal static class Queens
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        List<int> targetSquares = GetDiagonalAndCrossMasksForSquare(square);

        foreach (int targetSquare in targetSquares)
        {
            var targetSquareColor = Piece.GetColor(board[targetSquare]);
            bool isDiagonalMove = Math.Abs((targetSquare / 8) - (square / 8)) == Math.Abs((targetSquare % 8) - (square % 8));

            if ((isDiagonalMove && Globals.IsDiagonalPathClear(square, targetSquare, board)) ||
                (!isDiagonalMove && Globals.IsCrossSliderPathClear(square, targetSquare, board)))
            {
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
        }

        return moves;
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
