namespace Engine;

internal static class MoveHandler
{
    public static void MakeMove(MoveObject move, int[]board)
    {
        if(move.LongCastle || move.ShortCastle)
        {
            HandleCastlingMove(move, board);
            
        }
        else if (move.IsEnPassant)
        {
            HandleEnpassantMove(move, board);
            
        }
        else if (move.IsPromotion)
        {
            HandlePromotionMove(move, board);  
            
        }

        else
        {
            move.CapturedPiece = board[move.EndSquare];
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;
        }
         
    }

    public static void UndoMove(MoveObject move, int[]board)
    {
        if (move.LongCastle || move.ShortCastle)
        {
            HandleCastlingUndo(move, board);
            
        }
        else if (move.IsEnPassant)
        {
            HandleEnPassantUndo(move, board);
            
        }
        else if (move.IsPromotion)
        {
            HandlePromotionUndo(move, board);
            
        }
        else
        {
            board[move.EndSquare] = move.CapturedPiece;
            board[move.StartSquare] = move.pieceType;
        }
        
    }


    private static void HandleCastlingMove(MoveObject move, int[] board)
    {
        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if(move.LongCastle)
            {
                move.CastleStatus = Globals.WhiteShortCastle;
                move.CapturedPiece = 0;
                board[56]=0;  board[60]=0; board[58]=MoveGenerator.whiteKing; board[59] = MoveGenerator.whiteRook;
                Globals.WhiteShortCastle = false; Globals.WhiteLongCastle = false;
            }
            else if (move.ShortCastle)
            {
                move.CastleStatus = Globals.WhiteLongCastle;
                move.CapturedPiece = 0;
                board[63]=0; board[60]=0;  board[62]=MoveGenerator.whiteKing; board[61] = MoveGenerator.whiteRook;
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

    private static void HandleCastlingUndo(MoveObject move, int[] board)
    {
        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if (move.LongCastle)
            {
                board[59] = 0; board[58] = 0; board[56] = MoveGenerator.whiteRook; board[60] = MoveGenerator.whiteKing;
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

    private static void HandleEnpassantMove(MoveObject move, int[] board)
    {

    }

    private static void HandleEnPassantUndo(MoveObject move, int[] board)
    {

    }

    private static void HandlePromotionMove(MoveObject move, int[] board)
    {

    }

    private static void HandlePromotionUndo(MoveObject move, int[] board)
    {

    }
}
