namespace Engine;

internal static class Kings
{
    public static List<int>? DefendingSquares { get; set; }
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

        List<MoveObject> moves = new();


        if (turn == 0)
        {
            List<int> DefendingSquares = new();
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);

                if (targetsquareColor == "White") DefendingSquares.Add(targetSquare);
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
            return moves;
        }
        else if (turn == 1)
        {
            List<int> DefendingSquares = new();
            foreach (int targetSquare in targetSquares)
            {

                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (targetsquareColor == "Black") DefendingSquares.Add(targetSquare);
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
        }


        return moves;
    }

    // 
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


}

