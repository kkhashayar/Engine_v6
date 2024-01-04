namespace Engine;

internal static class Knights
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new();
        int piece = Piece.None;

        List<int> filteredMasksForSquare = Rules(GetKnightRawMoves(square), turn, board);


        if (turn == 0) piece = MoveGenerator.whiteKnight;
        else if (turn == 1) piece = MoveGenerator.blackKnight;

        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = piece,
                StartSquare = square,
                EndSquare = endSquare
            };
            moves.Add(move);
        }
        return moves;
    }

    public static List<int> Rules(List<int> maskForSquare, int turn, int[] board)
    {
        string pieceColor = "";
        List<int> result = new();

        if (turn == 0)
        {
            pieceColor = "White";
            foreach (int endSquare in maskForSquare)
            {

                if (!Piece.IsWhite(board[endSquare]))
                    result.Add(endSquare);
            }
            return result;
        }

        pieceColor = "Black";
        foreach (int endSquare in maskForSquare)
        {

            if (!Piece.IsBlack(board[endSquare]))
                result.Add(endSquare);
        }
        return result;
    }

    private static List<int> GetKnightRawMoves(int square) 
    {
      return MaskGenerator.KnightMasks[square];
    } 

    private static List<int> GetAttackSquares(int square)
    {
        return MaskGenerator.KingMasks[square];
    }
}
