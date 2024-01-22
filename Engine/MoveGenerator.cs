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
                }
                
                else if(piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if(piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if(piece == whiteBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if(piece == whiteQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if(piece == whitePawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                }

            }
            // Generate moves for black pieces
            else if (turn == 1)
            {
                if (piece == blackKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));    
                }
                
                else if(piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if(piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));   
                }

                else if (piece == blackBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackPawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                    
                 
                }
            }
        }
        return pseudoMoves;
    }


    private static bool IsMoveLegal(MoveObject move, int[] board, int turn)
    {
        if (turn == 0) // TODO: REFACTOR REPEATED CODE 
        {
            // Castlings 
            if (move.LongCastle)
            {
                int blackResponseTurnonCastle = turn;
                blackResponseTurnonCastle ^= 1;
                // generate black sseudo moves
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(board, blackResponseTurnonCastle);
                // checks reserved squares in long castle position 
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 59 || bMove.EndSquare == 58)) return false;
            }
            else if (move.ShortCastle)
            {
                int blackResponseTurnonCastle = turn;
                blackResponseTurnonCastle ^= 1;
                // generate black sseudo moves
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(board, blackResponseTurnonCastle);
                // checks reserved squares in long castle position
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 61 || bMove.EndSquare == 62)) return false;
            }

            // normal moves 
            // Make a move
            var capturedPieceForWhite = board[move.EndSquare];
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;

            // Get a king position on new board
            int whiteKingSquare = GetWhiteKingSquare(board);
            
            // flip the turn
            int blackResponseTurn = turn;
            blackResponseTurn ^= 1;
            // generate black sseudo moves
            var blackResponseMoves = GeneratePseudoLegalMoves(board, blackResponseTurn);

            // Take back the move
            board[move.EndSquare] = capturedPieceForWhite;
            if(move.pieceType == MoveGenerator.whitePawn && move.IsPromotion) move.pieceType = MoveGenerator.whitePawn;
            board[move.StartSquare] = move.pieceType;

            // Check if any black piece can hit the king 
            if (blackResponseMoves.Any(bMove => bMove.EndSquare == whiteKingSquare)) return false; 

            return true; // Move is legal 



        }
        // Black Castlings
        if (move.LongCastle)
        {
            int whiteResponseTurnCastle = turn;
            whiteResponseTurnCastle ^= 1;
            var WhiteResponseMovesCastle = GeneratePseudoLegalMoves(board, whiteResponseTurnCastle);
            // checks reserved squares in long castle position 
            if (WhiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 3 || wMove.EndSquare == 4)) return false;
        }
        else if (move.ShortCastle)
        {
            int whiteResponseTurnCastle = turn;
            whiteResponseTurnCastle ^= 1;
            var WhiteResponseMovesCastle = GeneratePseudoLegalMoves(board, whiteResponseTurnCastle);
            // checks reserved squares in long castle position
            if (WhiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 5 || wMove.EndSquare == 6)) return false;
        }

        // Same algorithm goes for black.
        var capturedPieceForBlack = board[move.EndSquare];
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;
        int blackKingSquare = GetBlackKingSquare(board);

        int whiteResponseTurn = turn;
        whiteResponseTurn ^= 1;
        var WhiteResponseMoves = GeneratePseudoLegalMoves(board, whiteResponseTurn);

        board[move.EndSquare] = capturedPieceForBlack;
        if (move.pieceType == MoveGenerator.blackPawn && move.IsPromotion) move.pieceType = MoveGenerator.blackPawn;
        board[move.StartSquare] = move.pieceType;

        if (WhiteResponseMoves.Any(wMove => wMove.EndSquare == blackKingSquare)) return false;

        return true;
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
