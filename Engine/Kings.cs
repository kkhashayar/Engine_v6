namespace Engine;

internal static class Kings
{
    public static IEnumerable<MoveObject> GenerateMovesForSquare(int square, int color, int[] board)
    {
        int piece = Piece.None; 
        List<int> masks = GetMaskForSquare(square);

        if(color == 0) piece = MoveGenerator.whiteKing;
        else if(color == 1) piece = MoveGenerator.blackKing;

        List<MoveObject> moves = new(); 

        foreach (int endSquare in masks)
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
    
    public static List<int> GetMaskForSquare(int square)
    {
        return MaskGenerator.KingMasks[square];
    }

    public static List<int> GetAttackMasksForSquare(int square)
    {
        return MaskGenerator.KingMasks[square];
    }

}

