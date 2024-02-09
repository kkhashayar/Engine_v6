namespace Engine;

internal static class MoveHandler
{
    public static void MakeMove(ref Globals globals, MoveObject move, int[]board)
    {
        if(move.LongCastle || move.ShortCastle)
        {
            HandleCastlingMove(ref globals, move, board);
            
        }
        else if (move.IsEnPassant)
        {
            HandleEnpassantMove(ref globals, move, board);
            
        }
        else if (move.IsPromotion)
        {
            HandlePromotionMove(ref globals, move, board);  
            
        }

        else
        {
            move.CapturedPiece = board[move.EndSquare];
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;
        }
         
    }

    public static void UndoMove(ref Globals globals, MoveObject move, int[]board)
    {
        if (move.LongCastle || move.ShortCastle)
        {
            HandleCastlingUndo(ref globals, move, board);
            
        }
        else if (move.IsEnPassant)
        {
            HandleEnPassantUndo(ref globals, move, board);
            
        }
        else if (move.IsPromotion)
        {
            HandlePromotionUndo(ref globals, move, board);
            
        }
        else
        {
            board[move.EndSquare] = move.CapturedPiece;
            board[move.StartSquare] = move.pieceType;
        }
        
    }


    private static void HandleCastlingMove(ref Globals globals, MoveObject move, int[] board)
    {
        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if(move.LongCastle)
            {
                move.CastleStatus = Globals.WhiteShortCastle; 

                board[56] = 0; board[58] = MoveGenerator.whiteKing; board[59] = MoveGenerator.whiteRook;
                Globals.WhiteShortCastle = false; Globals.WhiteLongCastle = false;
            }
            else if (move.ShortCastle)
            {
                move.CastleStatus = Globals.WhiteLongCastle;

                board[63] = 0; board[62] = MoveGenerator.whiteKing; board[61] = MoveGenerator.whiteRook;
                Globals.WhiteShortCastle = false; Globals.WhiteLongCastle= false;   
            }
        }

        else if (move.pieceType == MoveGenerator.blackKing)
        {
            if (move.LongCastle)
            {
                board[0] = 0; board[2] = MoveGenerator.blackKing; board[3] = MoveGenerator.blackRook;
                Globals.BlackLongCastle = false; Globals.BlackShortCastle = false; 
            }
            else if (move.ShortCastle)
            {
                board[7] = 0; board[6] = MoveGenerator.blackKing; board[5] = MoveGenerator.blackRook;
                Globals.BlackLongCastle = false; Globals.BlackShortCastle = false;
            }
        }
    }

    private static void HandleCastlingUndo(ref Globals globals, MoveObject move, int[] board)
    {
        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if (move.LongCastle)
            {
                board[59] = 0; board[56] = MoveGenerator.whiteRook; board[58] = 0; board[60] = MoveGenerator.whiteKing;
                Globals.WhiteLongCastle = true; Globals.WhiteShortCastle = move.CastleStatus;
                return;
            }
            else if (move.ShortCastle)
            {
                board[61] = 0; board[63] = MoveGenerator.whiteRook; board[62] = 0; board[60] = MoveGenerator.whiteKing;
                Globals.WhiteShortCastle = true; Globals.WhiteLongCastle = move.CastleStatus;
                return;
            }
        }

        else if(move.pieceType == MoveGenerator.blackKing)
        {
            if (move.LongCastle)
            {

            }
            else if (move.ShortCastle)
            {

            }
        }
    }

    private static void HandleEnpassantMove(ref Globals globals, MoveObject move, int[] board)
    {

    }

    private static void HandleEnPassantUndo(ref Globals globals, MoveObject move, int[] board)
    {

    }

    private static void HandlePromotionMove(ref Globals globals, MoveObject move, int[] board)
    {

    }

    private static void HandlePromotionUndo(ref Globals globals, MoveObject move, int[] board)
    {

    }
}
