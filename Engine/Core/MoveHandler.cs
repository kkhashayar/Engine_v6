using Engine.Core;

namespace Engine;

public static class MoveHandler
{
    public static void MakeMove(int[] board, MoveObject move)
    {
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;


        /////////////////////////////////// WHITE
        if (Globals.Turn == 0)             
        {
            /////////////////////////////////// Pawn Promotion  
            if (move.pieceType == MoveGenerator.whitePawn && move.IsPromotion)
            {
                var piece = move.PromotionPiece;    
                board[move.EndSquare] = piece;
            }

            if (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 63 && Globals.WhiteKingRookMoved is false)
            {
                Globals.WhiteKingRookMoved = true;
            }

            // ROOK ON LONG CASLTE 
            if (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 56 && Globals.WhiteQueenRookMoved is false)
            {
                Globals.WhiteQueenRookMoved = true;
            }
            /////////////////////////////////// Castling   

            if (move.pieceType == MoveGenerator.whiteKing && move.LongCastle)
            {
                board[56] = 0; board[59] = MoveGenerator.whiteRook;
                Globals.WhiteLongCastle = false;
                Globals.WhiteShortCastle = false;
                Globals.WhiteQueenRookMoved = true;
            }

            if (move.pieceType == MoveGenerator.whiteKing && move.ShortCastle)
            {
                board[63] = 0; board[61] = MoveGenerator.whiteRook;
                Globals.WhiteLongCastle = false;
                Globals.WhiteShortCastle = false;
                Globals.WhiteKingRookMoved = true;
            }
        }
        ///////////////////////////////// BLACK 
        else
        {

            /////////////////////////////////// Pawn Promotion
            if (move.pieceType == MoveGenerator.blackPawn && move.IsPromotion)
            {
                var piece = move.PromotionPiece;
                board[move.EndSquare] = piece;
            }

            if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 0 && Globals.BlackQueenRookMoved is false)
            {
                Globals.BlackQueenRookMoved = true;
            }


            if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 7 && Globals.BlackKingRookMoved is false)
            {
                Globals.BlackKingRookMoved = true;
            }


            if (move.pieceType == MoveGenerator.blackKing && move.LongCastle)
            {
                board[0] = 0; board[3] = MoveGenerator.blackRook;
                Globals.BlackLongCastle = false;
                Globals.BlackShortCastle = false;
                Globals.BlackQueenRookMoved = true;

            }
            if (move.pieceType == MoveGenerator.blackKing && move.ShortCastle)
            {
                board[7] = 0; board[5] = MoveGenerator.blackRook;
                Globals.BlackShortCastle = false;
                Globals.BlackLongCastle = false;
                Globals.BlackKingRookMoved = true;
            }
        }

    }

    public static void UndoMove(int[] board, MoveObject move, int pieceMoving, int targetSquare, int promotedTo)
    {
        board[move.StartSquare] = pieceMoving;
        board[move.EndSquare] = targetSquare;

        if ((move.pieceType == MoveGenerator.whitePawn || move.pieceType == MoveGenerator.blackPawn) && move.IsPromotion is true)
        {
            board[move.EndSquare] = 0;
            board[move.StartSquare] = move.pieceType;
        }

        if (move.pieceType == MoveGenerator.whiteKing)
        {
            if (move.ShortCastle)
            {
                board[61] = 0;
                board[63] = MoveGenerator.whiteRook;

            }

            if (move.LongCastle)
            {
                board[59] = 0;
                board[56] = MoveGenerator.whiteRook;

            }

        }


        if (move.pieceType == MoveGenerator.blackKing)
        {
            if (move.LongCastle)
            {
                board[3] = 0;
                board[0] = MoveGenerator.blackRook;


            }

            if (move.ShortCastle)
            {
                board[5] = 0;
                board[7] = MoveGenerator.blackRook;

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
