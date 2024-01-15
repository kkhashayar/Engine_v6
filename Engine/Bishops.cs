namespace Engine;

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
                if ((turn == 0 && targetSquareColor == "White") || (turn == 1 && targetSquareColor == "Black"))
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
}
