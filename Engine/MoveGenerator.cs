using System.Diagnostics.CodeAnalysis;
using System.Net.NetworkInformation;

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


    static Globals currentState = new Globals(); // LazyInitializer 
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
                // Filter out illegal white moves using black's pseudo-legal moves
                foreach (var move in whitePseudoMoves)
                {
                    if (IsMoveLegal(move, blackPseudoMoves, chessBoard))
                    {
                        moves.Add(move);
                    }
                }
            }
            else // Black's turn
            {
                // Filter out illegal black moves using white's pseudo-legal moves
                foreach (var move in blackPseudoMoves)
                {
                    if (IsMoveLegal(move, whitePseudoMoves, chessBoard))
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

    private static List<MoveObject> GeneratePseudoLegalMoves(int[] chessBoard, int color)
    {
        List<MoveObject> pseudoMoves = new List<MoveObject>();

        for (int square = 0; square < 64; square++)
        {
            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (color == 0)
            {
                if (piece == whiteKing)
                {
                    WhiteKingSquare = square;
                    pseudoMoves.AddRange(Kings.GenerateWKingMoves(square, chessBoard));
                }
                else if (piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateWRookMoves(square, chessBoard));
                }
                else if (piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateWKnightMoves(square, chessBoard));
                }

            }
            // Generate moves for black pieces
            else if (color == 1)
            {
                if (piece == blackKing)
                {
                    BlackKingSquare = square;
                    pseudoMoves.AddRange(Kings.GenerateBKingMoves(square, chessBoard));
                }
                else if (piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateBRookMoves(square, chessBoard));
                }
                else if (piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateBKnightMoves(square, chessBoard));
                }            
               
            }
        }

        return pseudoMoves;
    }


    //////////////////////////////////////   HELPERS 
    private static bool IsMoveLegal(MoveObject move, List<MoveObject> opponentMoves, int[] board)
    {
        // int[] boardCopy = (int[])board.Clone();

        Globals shadowState = Globals.Clone(currentState);
        MakeMove(move, shadowState.ChessBoard);

        int kingSquare = 0;
        kingSquare = GetCurrentColor(move, kingSquare);

        // Check if any opponent move can capture the king
        foreach (var oppMove in opponentMoves)
        {
            if (oppMove.EndSquare == kingSquare)
            {
                return false; // The move leaves or puts the king in check
            }
        }

        return true; // The move is legal
    }

    private static int GetCurrentColor(MoveObject move, int kingSquare)
    {
        var color = Piece.GetColor(move.pieceType);

        if (color == "White")
        {
            if (move.pieceType == whiteKing)
            {
                kingSquare = move.EndSquare;
            }

            else
            {
                kingSquare = WhiteKingSquare;
            }
        }
        else if (color == "Black")
        {
            if (move.pieceType == blackKing)
            {
                kingSquare = move.EndSquare;
            }

            else
            {
                kingSquare = BlackKingSquare;
            }
        }

        return kingSquare;
    }

    private static void MakeMove(MoveObject move, int[] board)
    {
        // Apply the move
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;
    }


    public static bool IsPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;
        bool pieceColor = Piece.IsBlack(board[startSquare]);

        while (currentSquare != endSquare)
        {
            if (board[currentSquare] != 0)
            {
                return false;
            }
            currentSquare += direction;
        }
        if (board[endSquare] != 0) // If the end square is occupied
        {
            bool targetPieceColor = Piece.IsBlack(board[endSquare]);
            if (pieceColor != targetPieceColor)
            {
                return true; // Can capture
            }
            else
            {
                return false; // Blocked by own piece
            }
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
