using Engine.Core;

namespace Engine;

public static class MoveHandler
{
    public static void MakeMove(int[] board, MoveObject move, int turn)
    {
        if(move is not null)
        {
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;


            /////////////////////////////////// WHITE
            if (turn == 0)
            {
                /////////////////////////////////// Pawn Promotion  
                if (move.pieceType == 1 && move.IsPromotion)
                {
                    board[move.EndSquare] = move.PromotionPiece;
                    
                }

                if (move.pieceType == 5 && move.StartSquare == 63 && Globals.WhiteKingRookMoved is false)
                {
                    Globals.WhiteKingRookMoved = true;
                }

                // ROOK ON LONG CASLTE 
                if (move.pieceType == 5 && move.StartSquare == 56 && Globals.WhiteQueenRookMoved is false)
                {
                    Globals.WhiteQueenRookMoved = true;
                }
                /////////////////////////////////// Castling   

                if (move.pieceType == 99 && move.LongCastle)
                {
                    board[56] = 0; board[59] = 5;
                    
                }

                else if (move.pieceType == 99 && move.ShortCastle)
                {
                    board[63] = 0; board[61] = 5;
                    
                }
            }
            ///////////////////////////////// BLACK 
            else
            {

                /////////////////////////////////// Pawn Promotion
                if (move.pieceType == 11 && move.IsPromotion)
                {
                    board[move.EndSquare] = move.PromotionPiece;
                    
                }

                if (move.pieceType == 15 && move.StartSquare == 0 && Globals.BlackQueenRookMoved is false)
                {
                    Globals.BlackQueenRookMoved = true;
                }


                if (move.pieceType == 15 && move.StartSquare == 7 && Globals.BlackKingRookMoved is false)
                {
                    Globals.BlackKingRookMoved = true;
                }


                if (move.pieceType == 109 && move.LongCastle)
                {
                    board[0] = 0; board[3] = 15;
                    
                }
                else if (move.pieceType == 109 && move.ShortCastle)
                {
                    board[7] = 0; board[5] = 15;
                    
                }
            }
        }
        

    }

    public static void UndoMove(int[] board, MoveObject move, int pieceMoving, int targetSquare, int promotedTo)
    {
        board[move.StartSquare] = pieceMoving;
        board[move.EndSquare] = targetSquare;

        if ((move.pieceType == 1 || move.pieceType == 11) && move.IsPromotion is true)
        {
            board[move.EndSquare] = 0;
            board[move.StartSquare] = move.pieceType;
        }

        else if (move.pieceType == 9)
        {
            if (move.ShortCastle)
            {
                board[61] = 0;
                board[62] = 0;
                board[63] = 5;

            }

            else if (move.LongCastle)
            {
                board[59] = 0;
                board[58] = 0;
                board[57] = 0;
                board[56] = 5;

            }

        }


        else if (move.pieceType == 109)
        {
            if (move.LongCastle)
            {
                board[1] = 0;
                board[2] = 0;
                board[3] = 0;
                board[0] = 15;


            }

            else if (move.ShortCastle)
            {
                board[6] = 0;
                board[5] = 0;
                board[7] = 15;

            }
        }
    }
    public static void RegisterStaticStates()
    {

        StateSnapshotBase.WhiteShortCastle = Globals.WhiteShortCastle;
        StateSnapshotBase.WhiteLongCastle = Globals.WhiteLongCastle;

        StateSnapshotBase.WhiteKingRookMoved = Globals.WhiteKingRookMoved;
        StateSnapshotBase.WhiteQueenRookMoved = Globals.WhiteQueenRookMoved;

        StateSnapshotBase.BlackShortCastle = Globals.BlackShortCastle;
        StateSnapshotBase.BlackLongCastle = Globals.BlackLongCastle;

        StateSnapshotBase.BlackKingRookMoved = Globals.BlackKingRookMoved;
        StateSnapshotBase.BlackQueenRookMoved = Globals.BlackQueenRookMoved;

        StateSnapshotBase.CheckmateWhite = Globals.CheckmateWhite;
        StateSnapshotBase.CheckmateBlack = Globals.CheckmateBlack;

        StateSnapshotBase.CheckWhite = Globals.CheckWhite;
        StateSnapshotBase.CheckBlack = Globals.CheckBlack;

        StateSnapshotBase.Stalemate = Globals.Stalemate;

        StateSnapshotBase.LastMoveWasPawn = Globals.LastMoveWasPawn;

        StateSnapshotBase.LastEndSquare = Globals.LastEndSquare;

        StateSnapshotBase.Turn = Globals.Turn;
    }

    public static void RestoreStateFromSnapshot()
    {
        Globals.WhiteShortCastle = StateSnapshotBase.WhiteShortCastle;
        Globals.WhiteLongCastle = StateSnapshotBase.WhiteLongCastle;

        Globals.WhiteKingRookMoved = StateSnapshotBase.WhiteKingRookMoved;
        Globals.WhiteQueenRookMoved = StateSnapshotBase.WhiteQueenRookMoved;

        Globals.BlackShortCastle = StateSnapshotBase.BlackShortCastle;
        Globals.BlackLongCastle = StateSnapshotBase.BlackLongCastle;

        Globals.BlackKingRookMoved = StateSnapshotBase.BlackKingRookMoved;
        Globals.BlackQueenRookMoved = StateSnapshotBase.BlackQueenRookMoved;

        Globals.CheckmateWhite = StateSnapshotBase.CheckmateWhite;
        Globals.CheckmateBlack = StateSnapshotBase.CheckmateBlack;

        Globals.CheckWhite = StateSnapshotBase.CheckWhite;
        Globals.CheckBlack = StateSnapshotBase.CheckBlack;

        Globals.Stalemate = StateSnapshotBase.Stalemate;

        Globals.LastMoveWasPawn = StateSnapshotBase.LastMoveWasPawn;

        Globals.LastEndSquare = StateSnapshotBase.LastEndSquare;

        Globals.Turn = StateSnapshotBase.Turn;
    }
}

/*
       00, 01,  02,  03,  04,  05,  06,  07,
       08, 09,  10,  11,  12,  13,  14,  15,
       16, 17,  18,  19,  20,  21,  22,  23,
       24, 25,  26,  27,  28,  29,  30,  31,
       32, 33,  34,  35,  36,  37,  38,  39,
       40, 41,  42,  43,  44,  45,  46,  47,
       48, 49,  50,  51,  52,  53,  54,  55,
       56, 57,  58,  59,  60,  61,  62,  63
 */