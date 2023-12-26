using System.Diagnostics.CodeAnalysis;

namespace Engine;

public static class MoveGenerator
{
    public static int WhiteKingSquare { get; set; }
    public static int BlackKingSquare { get; set; }

    public static List<int> BlackAttackSquares { get; set; }
    public static List<int> WhiteAttackSquares { get; set; }

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

    
   
    //////////////////////////////////////   ENGINE CORE LOOP 
  
    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn)// this is returning raw moves
    {
        List<MoveObject> moves = new();
        WhiteAttackSquares = new List<int>();
        BlackAttackSquares = new List<int>();

        if (turn == 3)
        {
            for (int square = 0; square < 64; square++)
            {
                // WHITE
                if (chessBoard[square] == whiteKing)
                {
                    WhiteKingSquare = square;
                    BlackKingSquare = square;
                    moves.AddRange(Kings.GenerateWKingMoves(square, chessBoard));

                }
                else if (chessBoard[square] == whiteKnight)
                {
                    moves.AddRange(Knights.GenerateWKnightMoves(square, chessBoard));
                    WhiteAttackSquares.Add(square);
                    
                }

                else if (chessBoard[square] == whiteRook)
                {
                    moves.AddRange(Rooks.GenerateWRookMoves(square, chessBoard));
                    WhiteAttackSquares.Add(square);
                }

                // BLACK
                else if (chessBoard[square] == blackKing) 
                {
                    WhiteKingSquare = square;
                    BlackKingSquare = square;
                    moves.AddRange(Kings.GenerateBKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == blackKnight)
                {
                    moves.AddRange(Knights.GenerateBKnightMoves(square, chessBoard));
                    BlackAttackSquares.Add(square);  
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
                    moves.AddRange(Kings.GenerateBKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == blackKnight)
                {
                    moves.AddRange(Kings.GenerateBKingMoves(square, chessBoard));
                    WhiteAttackSquares.Add(square);
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
                    moves.AddRange(Kings.GenerateWKingMoves(square, chessBoard));
                }
                else if (chessBoard[square] == whiteKnight)
                {
                    moves.AddRange(Knights.GenerateWKnightMoves(square, chessBoard));
                    WhiteAttackSquares.Add(square);  
                }

                else if (chessBoard[square] == whiteRook)
                {
                    moves.AddRange(Rooks.GenerateWRookMoves(square, chessBoard));
                    WhiteAttackSquares.Add(square);
                }
            }
        }

        return moves;
    }


    //////////////////////////////////////   HELPERS 
    private static bool IsWhiteKingInCheck(int[] board, int kingposition)
    {

        return false; 
    }


    public static bool IsPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;


        while (currentSquare != endSquare)
        {
            currentSquare += direction;
            var targetPieceColor = Piece.GetColor(board[currentSquare]);

            if (board[currentSquare] != 0)
                return false;

        }
        
        return true;
    }


    public static int GetDirection(int startSquare, int endSquare)
    {
        if (endSquare > startSquare) // Moving up or right
        {
            if (endSquare % 8 == startSquare % 8) // Vertical move
                return 8; 
            else // Horizontal move
                return 1;
        }
        else // Moving down or left
        {
            if (endSquare % 8 == startSquare % 8) // Vertical move
                return -8; 
            else // Horizontal move
                return -1;
        }
    }
}
