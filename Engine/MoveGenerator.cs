using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Tracing;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Engine;

public static class MoveGenerator
{
    

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

    public static List<int>? BlackDefendedSquares { get; set; }
    public static List<int>? WhiteDefenedSquares { get; set; }



    static Globals currentState = new Globals();

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
                    if (IsMoveLegal(move, chessBoard, turn))
                    {
                        moves.Add(move);
                    }
                }
            }
            else // Black's turn
            {
                foreach (var move in blackPseudoMoves)
                {
                    if (IsMoveLegal(move, chessBoard, turn))
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

        WhiteDefenedSquares = new();
        BlackDefendedSquares = new();

        for (int square = 0; square < 64; square++)
        {
            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (turn == 0)
            {
                if (piece == whiteKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                    if (Kings.DefendingSquares is not null) WhiteDefenedSquares.AddRange(Kings.DefendingSquares);
                }
                
                else if(piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                    if (Knights.DefendingSquares is not null) WhiteDefenedSquares.AddRange(Knights.DefendingSquares);
                }

                else if(piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));

                }
                

            }
            // Generate moves for black pieces
            else if (turn == 1)
            {
                if (piece == blackKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                    if (Kings.DefendingSquares is not null) BlackDefendedSquares.AddRange(Kings.DefendingSquares);
                }
                
                else if(piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                    if (Knights.DefendingSquares is not null) BlackDefendedSquares.AddRange(Knights.DefendingSquares);
                }

                else if(piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }
               
            }
        }
        return pseudoMoves;
    }


    private static bool IsMoveLegal(MoveObject move, int[] board, int turn)
    {
        

        // Early exit in case of capture deffended piece 
        if(turn == 0)
        {
            var capturedPieceForWhite = board[move.EndSquare];
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;

            int whiteKingSquare = GetWhiteKingSquare(board);
            // In case of capturing, can be useful for early exit before generating opposite moves
            if (BlackDefendedSquares is not null && BlackDefendedSquares.Any(s => s == whiteKingSquare)) return false;
            
            int blackResponseTurn = turn;
            blackResponseTurn ^= 1; 
            var blackResponseMoves = GeneratePseudoLegalMoves(board, blackResponseTurn);

            board[move.EndSquare] = capturedPieceForWhite;
            board[move.StartSquare] = move.pieceType;

            if (blackResponseMoves.Any(bMove => bMove.EndSquare == whiteKingSquare)) return false; 

            return true;
        }

        var capturedPieceForBlack = board[move.EndSquare];
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;
        int blackKingSquare = GetBlackKingSquare(board);
        // In case of capturing, can be useful for early exit before generating opposite moves
        if (WhiteDefenedSquares is not null && WhiteDefenedSquares.Any(s => s == blackKingSquare)) return false;

        int whiteResponseTurn = turn;
        whiteResponseTurn ^= 1;
        var WhiteResponseMoves = GeneratePseudoLegalMoves(board, whiteResponseTurn);

        board[move.EndSquare] = capturedPieceForBlack;
        board[move.StartSquare] = move.pieceType;

        if (WhiteResponseMoves.Any(bMove => bMove.EndSquare == blackKingSquare)) return false;

        return true; // The move is legal
    }

    private static int GetBlackKingSquare(int[] board)
    {
        for (int i = 0; i < 64; i++)
        {
            if (board[i] == 109)
            {
                return i;
            }
        }
        return -1;
    }
    private static int GetWhiteKingSquare(int[] board)
    {
        for (int i = 0; i < 64; i++)
        {
            if (board[i] == 99)
            {
                return i;
            }
        }
        return -1; 
    }


    
}
