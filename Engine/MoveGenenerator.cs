namespace Engine;

public static class MoveGenenerator
{
    public static readonly int whiteKing   = new Piece().King;
    public static readonly int whiteQueen  = new Piece().Queen;
    public static readonly int whiteRook   = new Piece().Rook;
    public static readonly int whiteKnight = new Piece().Knight;
    public static readonly int whiteBishop = new Piece().Bishop;
    public static readonly int whitePawn   = new Piece().Pawn;
    
    public static readonly int blackKing   = new Piece().King   + new Piece().BlackPieceOffset;
    public static readonly int blackQueen  = new Piece().Queen  + new Piece().BlackPieceOffset;
    public static readonly int blackRook   = new Piece().Rook   + new Piece().BlackPieceOffset;
    public static readonly int blackKnight = new Piece().Knight + new Piece().BlackPieceOffset;
    public static readonly int blackBishop = new Piece().Bishop + new Piece().BlackPieceOffset;
    public static readonly int blackPawn   = new Piece().Pawn   + new Piece().BlackPieceOffset;

    

    #region MAIN LOOP 
    public static List<MoveObject> GenerateAllMoves(int[] chessBoard)
    {
        List<MoveObject> moves = new(); 
        
        for (int square = 0;  square < chessBoard.Length; square++)
        {
            if (chessBoard[square] == whiteKing)  moves.AddRange(GenerateWKingMoves(square, chessBoard));
            else if (chessBoard[square] == blackKing) moves.AddRange(GenerateBKingMoves(square, chessBoard));

        }
        return moves;
    }
    #endregion


    #region GENERATING RAW MOVES FOR A PIECE ON GIVEN SQUARE
    public static IEnumerable<MoveObject> GenerateWKingMoves(int square, int[] board)
    {
       
        List<int> filteredMasksForSquare = WKingRules(GetKingMoves(square), board); 
        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = whiteKing,
                StatrSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    public static IEnumerable<MoveObject> GenerateBKingMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = BKingRules(GetKingMoves(square), board);
        foreach (int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = blackKing,
                StatrSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    #endregion


    #region RETRIEVING MASKS FOR PIECE ON GIVEN SQUARE  
    public static List<int> GetKingMoves(int square) => MaskGenerator.KingMasks[square];
    public static List<int> GetKnightMoves(int square) => MaskGenerator.KnightMasks[square];
    #endregion




    #region PIECE RULES AND CONDITIONS
    
    public static List<int> WKingRules(List<int> maskForSquare, int[] board)
    {
        foreach (int endSquare in maskForSquare)
        {
            if (board[endSquare] == whiteBishop || board[endSquare] == whiteRook ||
                board[endSquare] == whiteKnight || board[endSquare] == whiteQueen ||
                board[endSquare] == whitePawn || board[endSquare] == blackKing) maskForSquare.Remove(endSquare);

        }
        return maskForSquare;
    }

    public static List<int> BKingRules(List<int> maskForSquare, int[] board)
    {
        foreach (int endSquare in maskForSquare)
        {
            if (board[endSquare] == blackBishop || board[endSquare] == blackRook ||
                board[endSquare] == blackKnight || board[endSquare] == blackQueen ||
                board[endSquare] == blackPawn || board[endSquare] == whiteKing) maskForSquare.Remove(endSquare);

        }
        return maskForSquare;
    }

    #endregion
}
