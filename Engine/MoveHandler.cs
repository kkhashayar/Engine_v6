namespace Engine;

internal static class MoveHandler
{
    public static int whiteShortCastleRookPosition { get; set; } = 56;
    public static int whiteLongCastleRookPosition { get; set; } = 63;
    public static int blackShortCastleRookPosition { get; set; } = 7;
    public static int blackLongCastleRookPosition { get; set; } = 0;

    public static void MakeMove(MoveObject move, int[] board)
    {
        if (move.LongCastle || move.ShortCastle)
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

            if ((move.pieceType == MoveGenerator.whiteRook && move.StartSquare == whiteLongCastleRookPosition && Globals.WhiteLongCastle) ||
            (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == whiteShortCastleRookPosition && Globals.WhiteShortCastle) ||
            (move.pieceType == MoveGenerator.blackRook && move.StartSquare == blackLongCastleRookPosition && Globals.BlackLongCastle) ||
            (move.pieceType == MoveGenerator.blackRook && move.StartSquare == blackShortCastleRookPosition && Globals.BlackShortCastle))
            {
                UpdateCastleRightsOnRookMoves(move);
            }
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

            // undo any changes in castle rights based on rook moves 
            if(move.pieceType == MoveGenerator.whiteRook 
                &&(move.StartSquare == whiteShortCastleRookPosition || move.StartSquare == whiteLongCastleRookPosition))
            {
                UpdateCastleRightsOnRookUndo(move);
            }

            if (move.pieceType == MoveGenerator.blackRook
                && (move.StartSquare == blackShortCastleRookPosition || move.StartSquare == blackLongCastleRookPosition))
            {
                UpdateCastleRightsOnRookUndo(move);
            }

        }
        
    }


    private static void HandleCastlingMove(MoveObject move, int[] board)
    {
        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if(move.LongCastle)
            {
                move.CastleStatus = Globals.WhiteShortCastle;
                
                board[56] = 0;  board[60] = 0; board[58] = MoveGenerator.whiteKing; board[59] = MoveGenerator.whiteRook;
                Globals.WhiteShortCastle = false; Globals.WhiteLongCastle = false;
            }
            else if (move.ShortCastle)
            {
                move.CastleStatus = Globals.WhiteLongCastle;
                
                board[63] = 0; board[60] = 0;  board[62] = MoveGenerator.whiteKing; board[61] = MoveGenerator.whiteRook;
                Globals.WhiteShortCastle = false; Globals.WhiteLongCastle = false;   
            }
        }

        else if (move.pieceType == MoveGenerator.blackKing)
        {
            if (move.LongCastle)
            {
                move.CastleStatus = Globals.BlackShortCastle;

                board[0] = 0; board[2] = MoveGenerator.blackKing; board[3] = MoveGenerator.blackRook;
                Globals.BlackLongCastle = false; Globals.BlackShortCastle = false; 
            }
            else if (move.ShortCastle)
            {
                move.CastleStatus = Globals.BlackLongCastle;

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
                board[3] = 0; board[2] = 0; board[0] = MoveGenerator.blackRook; board[4] = MoveGenerator.blackKing;
                Globals.BlackLongCastle = true; Globals.BlackShortCastle = move.CastleStatus;
                return;
            }
            else if (move.ShortCastle)
            {
                board[6] = 0; board[5] = 0; board[7] = MoveGenerator.blackRook; board[4] = MoveGenerator.blackKing;
                Globals.BlackShortCastle = true; Globals.BlackLongCastle = move.CastleStatus;
                return;
            }
        }
    }

    private static void UpdateCastleRightsOnRookUndo(MoveObject move)
    {
        if(move.pieceType == MoveGenerator.whiteRook)
        {
            if(move.StartSquare == whiteLongCastleRookPosition)
            {
                Globals.WhiteLongCastle = move.CastleStatus;
            }
            else if(move.StartSquare == whiteShortCastleRookPosition)
            {
                Globals.WhiteShortCastle = move.CastleStatus;
            }
        }
        else if(move.pieceType == MoveGenerator.blackRook)
        {
            if (move.StartSquare == blackLongCastleRookPosition)
            {

            }
            else if (move.StartSquare == blackShortCastleRookPosition)
            {
                Globals.BlackShortCastle = move.CastleStatus;
            }
        }
    }
    private static void UpdateCastleRightsOnRookMoves(MoveObject move)
    {
        if(move.pieceType == MoveGenerator.whiteRook)
        {
            if(move.StartSquare == whiteLongCastleRookPosition)
            {
                move.CastleStatus = Globals.WhiteLongCastle;
                Globals.WhiteLongCastle = false;    
            }
            else if(move.StartSquare == whiteShortCastleRookPosition)
            {
                move.CastleStatus = Globals.WhiteShortCastle;
                Globals.WhiteShortCastle = false;
            }
        }


        else if (move.pieceType == MoveGenerator.blackRook)
        {
            if (move.StartSquare == blackLongCastleRookPosition)
            {
                move.CastleStatus = Globals.BlackLongCastle;
                Globals.BlackLongCastle = false;
            }
            else if (move.StartSquare == blackShortCastleRookPosition)
            {
                move.CastleStatus = Globals.BlackShortCastle;
                Globals.BlackShortCastle = false;
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
