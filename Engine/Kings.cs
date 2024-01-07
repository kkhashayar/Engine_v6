namespace Engine;

internal static class Kings
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
      
        List<int> masks = GetMasksForSquare(square);
        List<int> counterKingMasks = new(); 
        if(turn == 0)
        {
            counterKingMasks = GetMasksForSquare(MoveGenerator.BlackKingSquare);
        }
        else if(turn == 1)
        {
            counterKingMasks = GetMasksForSquare(MoveGenerator.WhiteKingSquare);
        }

        int piece = Piece.None;
        string pieceColor = "";
       
        if (turn == 0)      piece = MoveGenerator.whiteKing;
     
        else if (turn == 1) piece = MoveGenerator.blackKing;

        pieceColor = Piece.GetColor(piece);

        List<MoveObject> moves = new();


        foreach (int endSquare in masks)
        {
            if (!counterKingMasks.Contains(endSquare))
            {
                if (Piece.GetColor(board[endSquare]) != pieceColor || board[endSquare] == 0)
                {
                    MoveObject move = new MoveObject
                    {
                        pieceType = piece,
                        StartSquare = square,
                        EndSquare = endSquare
                    };
                    moves.Add(move);
                }
            } 
        }
        return moves;
    }
    
    public static List<int> GetMasksForSquare(int square)
    {
        return MaskGenerator.KingMasks[square];
    }

    public static List<int> GetAttackSquares(int square)
    {
        return MaskGenerator.KingMasks[square];
    }

}

