namespace Engine;

internal static class Knights
{
    public static IEnumerable<MoveObject> GenerateWKnightMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = WKnightRules(GetKnightRawMoves(square), board);

        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = MoveGenerator.whiteKnight,
                StartSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    public static List<int> WKnightRules(List<int> maskForSquare, int[] board)
    {

        List<int> result = new();
        foreach (int endSquare in maskForSquare)
        {
            if (!Piece.IsWhite(board[endSquare]))
                result.Add(endSquare);
        }
        return result;
    }

    public static IEnumerable<MoveObject> GenerateBKnightMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = BKnightRules(Kings.GetMaskForSquare(square), board);

        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = MoveGenerator.blackKing,
                StartSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    public static List<int> BKnightRules(List<int> maskForSquare, int[] board)
    {
        List<int> result = new();
        foreach (int endSquare in maskForSquare)
        {
            if (!Piece.IsBlack(board[endSquare]))
                result.Add(endSquare);
        }
        return result;
    }

    private static List<int> GetKnightRawMoves(int square) => MaskGenerator.KnightMasks[square];
}
