namespace Engine;

internal static class MoveHandler
{
    public static void MakeMove(ref Globals globals, MoveObject move)
    {
        
        globals.ChessBoard[move.EndSquare] = globals.ChessBoard[move.StartSquare];
        globals.ChessBoard[move.StartSquare] = 0;

        // Handle special moves: Castling, En Passant, Promotion
        // Update game state flags like castling rights, en passant possibilities

        UpdateCastlingRights(ref globals, move);

        // Other situations?!
    }

    public static void UndoMove(ref Globals globals, MoveObject move)
    {
        // Reverse the move
        globals.ChessBoard[move.StartSquare] = globals.ChessBoard[move.EndSquare];
        if (move.CapturedPiece != 0)
        {
            globals.ChessBoard[move.EndSquare] = move.CapturedPiece;
        }
        else
        {
            globals.ChessBoard[move.EndSquare] = 0;
        }

        // Reverse castling rights if necessary
        // Reverse any additional flags or game state changes

        // Special handling to reverse en passant, promotion, etc.
    }

    private static void UpdateCastlingRights(ref Globals globals, MoveObject move)
    {
        // Check if the move involves a rook or king and update the corresponding castling rights
        if (move.pieceType == Piece.King)
        {
            if (globals.Turn == 0) // White
            {
                Globals.WhiteShortCastle = Globals.WhiteLongCastle = false;
            }
            else // Black
            {
                Globals.BlackShortCastle = Globals.BlackLongCastle = false;
            }
        }
        else if (move.pieceType == Piece.Rook)
        {
            // Determine if the rook at the initial position moved and update castling rights accordingly
            if (move.StartSquare == 0 || move.StartSquare == 7) Globals.BlackShortCastle = false;
            if (move.StartSquare == 56 || move.StartSquare == 63) Globals.WhiteShortCastle = false;
        }
    }

}
