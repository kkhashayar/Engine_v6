using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Engine;

public static class MoveGenerator
{
    public static int WhiteKingSquare { get; set; }
    public static int BlackKingSquare { get; set; }

    public static List<int>? BlackAttackSquares { get; set; }
    public static List<int>? WhiteAttackSquares { get; set; }

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

    public static bool WhiteKingIsCheck { get; set; } = false;
    public static bool BlackKingIsCheck { get; set; } = false;


    static Globals currentState = new Globals(); // LazyInitializer need to think about it 
    //////////////////////////////////////   ENGINE CORE LOOP 

    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn, bool filter = false)
    {
        List<MoveObject> moves = new List<MoveObject>();

        // Generate all pseudo-legal moves for white and black.
        List<MoveObject> whitePseudoMoves = GeneratePseudoLegalMoves(chessBoard, 0); // 0 for white
        List<MoveObject> blackPseudoMoves = GeneratePseudoLegalMoves(chessBoard, 1); // 1 for black

        if (filter is true)
        {
            if (turn == 0) // White's turn
            {
                foreach (var move in whitePseudoMoves)
                {
                    if (IsMoveLegal(move, blackPseudoMoves, chessBoard, turn))
                    {
                        moves.Add(move);
                    }
                }
            }
            else // Black's turn
            {
                foreach (var move in blackPseudoMoves)
                {
                    if (IsMoveLegal(move, whitePseudoMoves, chessBoard, turn))
                    {
                        moves.Add(move);
                    }
                }
            }
        }
        else
        {
            // If not filtering, just return all moves for the current turn
            if (turn == 0)
            {
                moves = whitePseudoMoves;
            }
            else
            {
                moves = blackPseudoMoves;
            }
        }
        return moves;
    }

    private static List<MoveObject> GeneratePseudoLegalMoves(int[] chessBoard, int turn)
    {
        List<MoveObject> pseudoMoves = new List<MoveObject>();
        for (int square = 0; square < 64; square++)
        {
            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (turn == 0)
            {
                if (piece == whiteKing)
                {
                    WhiteKingSquare = square;
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }

            }
            // Generate moves for black pieces
            else if (turn == 1)
            {
                if (piece == blackKing)
                {
                    BlackKingSquare = square;
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }
            }
        }
        return pseudoMoves;
    }


    private static bool IsMoveLegal(MoveObject move, List<MoveObject> opponentMoves, int[] board, int turn)
    {
        int[] shadowBoard = (int[])board.Clone();
        MakeMove(move, shadowBoard); // Apply the move to the shadow board

        // Find current king's position after the move
        int kingSquare = GetKingSquare(move, shadowBoard);

        // Check if any opponent move can capture the king, implying check or checkmate
        foreach (var oppMove in opponentMoves)
        {
            if (oppMove.EndSquare == kingSquare)
            {
                return false; // The move leaves or puts the king in check
            }
        }

        return true; // The move is legal
    }


    private static int GetKingSquare(MoveObject move, int[] board)
    {
        // If the moved piece is a king, its new position is the end square.
        if (move.pieceType == whiteKing || move.pieceType == blackKing)
        {
            return move.EndSquare;
        }

        // For other pieces, find the king's position from the board
        int kingValue = move.pieceType > 0 ? whiteKing : blackKing;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == kingValue)
            {
                return i;
            }
        }
        return -1; // Error state, king not found
    }


    private static void MakeMove(MoveObject move, int[] shadowBoard)
    {
        move.CapturedPiece = shadowBoard[move.EndSquare];
        shadowBoard[move.EndSquare] = move.pieceType; // Place the moving piece in the end square
        shadowBoard[move.StartSquare] = 0; // Clear the start square
    }



    public static bool IsPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;
        bool pieceColor = Piece.IsBlack(board[startSquare]);

        while (currentSquare != endSquare)
        {
            if (board[currentSquare] != 0) return false;
            currentSquare += direction;
        }

        return board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != pieceColor;
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


    private static int GetKingPosition(int color, int[] board)
    {
        int king = color == 0 ? 99 : 199;
        return Array.FindIndex(board, b => b == king);
    }

}
