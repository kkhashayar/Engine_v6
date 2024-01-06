
namespace Engine;

internal static class Rooks
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();
        int piece = (turn == 0) ? MoveGenerator.whiteRook : MoveGenerator.blackRook;

        List<int> filteredMasksForSquare = Rules(GetMasksForSquare(square), board, square);

        foreach (int endSquare in filteredMasksForSquare)
        {
            moves.Add(new MoveObject
            {
                pieceType = piece,
                StartSquare = square,
                EndSquare = endSquare
            });
        }
        return moves;
    }

    public static List<int> Rules(List<int> maskForSquare, int[] board, int startSquare)
    {
        List<int> validMoves = new List<int>();
        foreach (int endSquare in maskForSquare)
        {
            if (MoveGenerator.IsPathClear(startSquare, endSquare, board))
            {
                if (board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != Piece.IsBlack(board[startSquare]))
                    validMoves.Add(endSquare);
            }
        }
        return validMoves;
    }

    private static List<int> GetMasksForSquare(int square)
    {
        return MaskGenerator.RookMasks[square];
    }


    public static List<int> GetAttackSquares(int square)
    {
        return MaskGenerator.RookMasks[square];
    }


}
