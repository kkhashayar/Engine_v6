
namespace Engine;

internal static class Rooks
{
    public static IEnumerable<MoveObject> GenerateWRookMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = WRookRules(GetRookRawMoves(square), board, square); // Notice the addition of square to the method call
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



    private static List<int> WRookRules(List<int> maskForSquare, int[] board, int startSquare)
    {
        // Might be better to send the whole list 
        List<int> result = new();
        foreach (int endSquare in maskForSquare)
        {
            //!Piece.IsWhite(board[endSquare]) &&
            if ( MoveGenerator.IsPathClear(startSquare, endSquare, board))
                result.Add(endSquare);
        }
        return result;
    }


    private static List<int> GetRookRawMoves(int square) => MaskGenerator.RookMasks[square];

}
