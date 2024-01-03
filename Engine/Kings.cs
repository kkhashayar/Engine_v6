namespace Engine;

internal static class Kings
{
    public static IEnumerable<MoveObject> GenerateWKingMoves(int square, int[] board)
    {

        List<int> filteredMasksForSquare = WKingRules(GetMaskForSquare(square), board);

        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = MoveGenerator.whiteKing,
                StartSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    public static List<int> WKingRules(List<int> maskForSquare, int[] board)
    {
        List<int> blackKingInfluence = GetMaskForSquare(Array.IndexOf(board, MoveGenerator.blackKing));
        List<int> result = new List<int>();
        foreach (int endSquare in maskForSquare)
        {
            if ((board[endSquare] == 0 || Piece.IsBlack(board[endSquare])) && !blackKingInfluence.Contains(endSquare))
                result.Add(endSquare);
        }
        return result;
    }



    public static IEnumerable<MoveObject> GenerateBKingMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = BKingRules(GetMaskForSquare(square), board);

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

    public static List<int> BKingRules(List<int> maskForSquare, int[] board)
    {
        List<int> whiteKingInfluence = GetMaskForSquare(Array.IndexOf(board, MoveGenerator.whiteKing));
        List<int> result = new List<int>();
        foreach (int endSquare in maskForSquare)
        {
            if ((board[endSquare] == 0 || Piece.IsWhite(board[endSquare])) && !whiteKingInfluence.Contains(endSquare))
                result.Add(endSquare);

        }
        return result;
    }


    public static List<int> GetMaskForSquare(int square) => MaskGenerator.KingMasks[square];
}
