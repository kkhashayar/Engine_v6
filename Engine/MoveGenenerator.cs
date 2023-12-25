using System.Diagnostics.CodeAnalysis;

namespace Engine;

public static class MoveGenenerator
{
    public static int WhiteKingSquare { get; set; }
    public static int BlackKingSquare { get; set; }

    public static readonly int whiteKing = Piece.King;
    public static readonly int whiteQueen = Piece.Queen;
    public static readonly int whiteRook = Piece.Rook;
    public static readonly int whiteKnight = Piece.Knight;
    public static readonly int whiteBishop = Piece.Bishop;
    public static readonly int whitePawn = Piece.Pawn;

    public static readonly int blackKing = Piece.King + Piece.BlackPieceOffset;
    public static readonly int blackQueen = Piece.Queen + Piece.BlackPieceOffset;
    public static readonly int blackRook = Piece.Rook + Piece.BlackPieceOffset;
    public static readonly int blackKnight = Piece.Knight + Piece.BlackPieceOffset;
    public static readonly int blackBishop = Piece.Bishop + Piece.BlackPieceOffset;
    public static readonly int blackPawn = Piece.Pawn + Piece.BlackPieceOffset;

    
    #region MAIN LOOP 
    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn)// this is returning raw moves
    {
        List<MoveObject> moves = new();

        if (turn == 3)
        {
            for (int square = 0; square < 64; square++)
            {
                // WHITE
                if (chessBoard[square] == whiteKing)
                {
                    WhiteKingSquare = square;
                    BlackKingSquare = square;
                    moves.AddRange(GenerateWKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == whiteKnight)
                {
                    moves.AddRange(GenerateWKnightMoves(square, chessBoard));
                }

                else if (chessBoard[square] == whiteRook)
                {
                    moves.AddRange(GenerateWRookMoves(square, chessBoard));
                }

                // BLACK
                else if (chessBoard[square] == blackKing) 
                {
                    WhiteKingSquare = square;
                    BlackKingSquare = square;
                    moves.AddRange(GenerateBKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == blackKnight)
                {
                    moves.AddRange(GenerateBKnightMoves(square, chessBoard));
                }

            }
        }

        else if (turn == 1)
        {
            for (int square = 0; square < 64; square++)
            {
                
                if (chessBoard[square] == blackKing) 
                {
                    BlackKingSquare = square;
                    WhiteKingSquare = square;
                    moves.AddRange(GenerateBKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == blackKnight)
                {
                    moves.AddRange(GenerateBKingMoves(square, chessBoard));
                }

            }
        }

        else if (turn == 0)
        {
            for (int square = 0; square < 64; square++)
            {
                if (chessBoard[square] == whiteKing)
                {
                    WhiteKingSquare = square;
                    BlackKingSquare = square;
                    moves.AddRange(GenerateWKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == whiteKnight)
                {
                    moves.AddRange(GenerateWKnightMoves(square, chessBoard));
                }

                else if (chessBoard[square] == whiteRook)
                {
                    moves.AddRange(GenerateWRookMoves(square, chessBoard));
                }
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

    public static IEnumerable<MoveObject> GenerateWKnightMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = WKnightRules(GetKnightRawMoves(square), board);

        foreach(int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = whiteKnight,
                StatrSquare = square,
                EndSquare = endSquare
            };
            yield return move;
        }
    }

    public static IEnumerable<MoveObject> GenerateWRookMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = WRookRules(GetRookRawMoves(square), board);
        foreach(int endSquare in filteredMasksForSquare)
        {
            MoveObject move = new MoveObject
            {
                pieceType = whiteRook,
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


    public static IEnumerable<MoveObject> GenerateBKnightMoves(int square, int[] board)
    {
        List<int> filteredMasksForSquare = BKnightRules(GetKingRawMoves(square), board);

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


    #region PIECE RULES AND CONDITIONS

    ////////////////////////////////     KINGS 
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
            if (board[endSquare] == 0 && !whiteKingInfluence.Contains(endSquare))
                result.Add(endSquare);

        }
        return result;
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
    //////////////////////////////// KNIGHT 

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
    
    public static List<int>BKnightRules(List<int> maskForSquare, int[] board)
    {
        List<int> result = new();
        foreach (int endSquare in maskForSquare)
        {
            if (!Piece.IsBlack(board[endSquare]))
                result.Add(endSquare);
        }
        return result;
    }

    #endregion



    #region RETRIEVING MASKS FOR PIECE ON GIVEN SQUARE  
    private static List<int> GetKingRawMoves(int square) => MaskGenerator.KingMasks[square];
    private static List<int> GetKnightRawMoves(int square) => MaskGenerator.KnightMasks[square];
    private static List<int> GetRookRawMoves(int square) => MaskGenerator.RookMasks[square];


    #endregion







}
