
namespace Engine;

internal static class Rooks
{
    public static IEnumerable<MoveObject> GenerateWRookMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = WRookRules(GetRookRawMoves(square), board);
        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = MoveGenerator.whiteRook,
                StatrSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }



    private static List<int> WRookRules(List<int> maskForSquare, int[] board)
    {
        List<int> result = new();
        // Filters  
        foreach (int endsquare in maskForSquare)
        {
            if (!Piece.IsWhite(board[endsquare]))
                result.Add(endsquare);
        }

        return result;
    }


    private static List<int> GetRookRawMoves(int square) => MaskGenerator.RookMasks[square];

}
