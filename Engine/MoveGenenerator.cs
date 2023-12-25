using System.Diagnostics.CodeAnalysis;

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
    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn = 3)// this is returning raw moves
    {
        List<MoveObject> moves = new(); 
        
        if(turn == 3)
        {
            for (int square = 0; square < 64; square++)
            {
                if (chessBoard[square] == whiteKing) moves.AddRange(GenerateWKingMoves(square, chessBoard));
                else if (chessBoard[square] == blackKing) moves.AddRange(GenerateBKingMoves(square, chessBoard));

            }
        }

        else if(turn == 1)
        {
            for (int square = 0; square < 64; square++)
            {
                
                if (chessBoard[square] == blackKing) moves.AddRange(GenerateBKingMoves(square, chessBoard));

            }
        }

        else if(turn == 0)
        {
            for (int square = 0; square < 64; square++)
            {
                if (chessBoard[square] == whiteKing) moves.AddRange(GenerateWKingMoves(square, chessBoard));
            }
        }
        
        return moves;
    }
    #endregion


    #region GENERATING RAW MOVES FOR A PIECE ON GIVEN SQUARE
    public static IEnumerable<MoveObject> GenerateWKingMoves(int square, int[] board)
    {
        
        List<int> filteredMasksForSquare = WKingRules(GetKingRawMoves(square), board); 
        
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
        List<int> filteredMasksForSquare = BKingRules(GetKingRawMoves(square), board);
        
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
    public static List<int> GetKingRawMoves(int square) => MaskGenerator.KingMasks[square];
    public static List<int> GetKnightRawMoves(int square) => MaskGenerator.KnightMasks[square];
    #endregion




    #region PIECE RULES AND CONDITIONS
    
    public static List<int> WKingRules(List<int> maskForSquare, int[] board)
    {
       
        List<int> blackKingInfluence = GetKingRawMoves(Array.IndexOf(board, blackKing));
        List<int> result = new List<int>();
        foreach (int endSquare in maskForSquare)
        {
            if (board[endSquare] == 0 && !blackKingInfluence.Contains(endSquare)) result.Add(endSquare);
        }
        return result;
    }

    public static List<int> BKingRules(List<int> maskForSquare, int[] board)
    {
        List<int> whiteKingInfluence = GetKingRawMoves(Array.IndexOf(board, whiteKing));
        List<int> result = new List<int>();
        foreach (int endSquare in maskForSquare)
        {
            if (board[endSquare] == 0 && !whiteKingInfluence.Contains(endSquare)) result.Add(endSquare);

        }
        return result;
    }

    #endregion
}
