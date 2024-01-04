
namespace Engine;

internal static class Rooks
{
    public static List<MoveObject> GenerateMovesForSquare(int square,int turn ,int[] board)
    {
        List<MoveObject> moves = new(); 
        int piece = Piece.None;

        string pieceColor = ""; 

        if (turn == 0)     piece = MoveGenerator.whiteRook;
        else if(turn == 1) piece = MoveGenerator.blackRook;
       
        pieceColor = Piece.GetColor(piece); 
        List<int> filteredMasksForSquare = Rules(GetmasksForSquare(square), board, turn, square); 

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

    public static List<int> Rules(List<int> maskForSquare, int[] board, int turn, int startSquare)
    {
        string pieceColor = "";
        if(turn == 0)
        {
            pieceColor = "White";
        }
        else if (turn == 1)
        {
            pieceColor = "Black";
        }
        List<int> result = new();
        foreach (int endSquare in maskForSquare)
        {
            //!Piece.IsWhite(board[endSquare]) &&
            if (MoveGenerator.IsPathClear(startSquare, endSquare, board))
                result.Add(endSquare);
        }
        return result;

    }

   
    private static List<int> GetmasksForSquare(int square)
    {
      return MaskGenerator.RookMasks[square];
    }

    public static List<int> GetAttackSquares(int square)
    {
        return MaskGenerator.RookMasks[square];
    }


}
