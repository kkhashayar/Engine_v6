namespace Engine;

internal static class Knights
{
    public static List<int>? DefendingSquares { get; set; }
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

        List<MoveObject> moves = new();
       

        if(turn == 0)
        {
            DefendingSquares = new();
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (targetsquareColor == "White") DefendingSquares.Add(targetSquare);
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
        else if(turn == 1)
        {
            DefendingSquares = new();
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (targetsquareColor == "Black") DefendingSquares.Add(targetSquare);
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
