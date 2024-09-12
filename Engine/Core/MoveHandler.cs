using Engine.Core;

namespace Engine;

public static class MoveHandler
{
    public static void MakeMove(int[] board, MoveObject move)
    {
        if (move is not null)
        {
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;

            /////////////////////////////////// WHITE TURN ///////////////////////////////////
            if (Globals.Turn == 0)
            {
                // Pawn Promotion
                if (move.pieceType == MoveGenerator.whitePawn && move.IsPromotion)
                {
                    var piece = move.PromotionPiece;
                    board[move.EndSquare] = piece;
                }

                // If white's king-side rook moves, disable short castling
                if (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 63 && Globals.WhiteKingRookMoved is false)
                {
                    Globals.WhiteKingRookMoved = true;
                    Globals.WhiteShortCastle = false;
                }

                // If white's queen-side rook moves, disable long castling
                if (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 56 && Globals.WhiteQueenRookMoved is false)
                {
                    Globals.WhiteQueenRookMoved = true;
                    Globals.WhiteLongCastle = false;
                }

                // Handle White Short Castling
                if (move.pieceType == MoveGenerator.whiteKing && move.ShortCastle)
                {
                    board[63] = 0;
                    board[61] = MoveGenerator.whiteRook;
                    Globals.WhiteShortCastle = false;
                    Globals.WhiteLongCastle = false;
                    Globals.WhiteKingRookMoved = true;
                    Globals.WhiteQueenRookMoved = true; // Ensure both are false after castling
                }

                // Handle White Long Castling
                if (move.pieceType == MoveGenerator.whiteKing && move.LongCastle)
                {
                    board[56] = 0;
                    board[59] = MoveGenerator.whiteRook;
                    Globals.WhiteLongCastle = false;
                    Globals.WhiteShortCastle = false;
                    Globals.WhiteQueenRookMoved = true;
                    Globals.WhiteKingRookMoved = true; // Ensure both are false after castling
                }

                // If the white king moves, disable both castling rights
                if (move.pieceType == MoveGenerator.whiteKing)
                {
                    Globals.WhiteShortCastle = false;
                    Globals.WhiteLongCastle = false;
                }
            }

            /////////////////////////////////// BLACK TURN ///////////////////////////////////
            else
            {
                // Pawn Promotion
                if (move.pieceType == MoveGenerator.blackPawn && move.IsPromotion)
                {
                    var piece = move.PromotionPiece;
                    board[move.EndSquare] = piece;
                }

                // If black's king-side rook moves, disable short castling
                if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 7 && Globals.BlackKingRookMoved is false)
                {
                    Globals.BlackKingRookMoved = true;
                    Globals.BlackShortCastle = false;
                }

                // If black's queen-side rook moves, disable long castling
                if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 0 && Globals.BlackQueenRookMoved is false)
                {
                    Globals.BlackQueenRookMoved = true;
                    Globals.BlackLongCastle = false;
                }

                // Handle Black Short Castling
                if (move.pieceType == MoveGenerator.blackKing && move.ShortCastle)
                {
                    board[7] = 0;
                    board[5] = MoveGenerator.blackRook;
                    Globals.BlackShortCastle = false;
                    Globals.BlackLongCastle = false;
                    Globals.BlackKingRookMoved = true;
                    Globals.BlackQueenRookMoved = true;
                }

                // Handle Black Long Castling
                if (move.pieceType == MoveGenerator.blackKing && move.LongCastle)
                {
                    board[0] = 0;
                    board[3] = MoveGenerator.blackRook;
                    Globals.BlackLongCastle = false;
                    Globals.BlackShortCastle = false;
                    Globals.BlackKingRookMoved = true;
                    Globals.BlackQueenRookMoved = true;
                }

                // If the black king moves, disable both castling rights
                if (move.pieceType == MoveGenerator.blackKing)
                {
                    Globals.BlackShortCastle = false;
                    Globals.BlackLongCastle = false;
                }
            }
        }
    }

    public static void UndoMove(int[] board, MoveObject move, int pieceMoving, int targetSquare, int promotedTo)
    {
        board[move.StartSquare] = pieceMoving;
        board[move.EndSquare] = targetSquare;

        // Undo pawn promotion
        if ((move.pieceType == MoveGenerator.whitePawn || move.pieceType == MoveGenerator.blackPawn) && move.IsPromotion is true)
        {
            board[move.EndSquare] = 0;
            board[move.StartSquare] = move.pieceType;
        }

        // Undo white king castling
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

        // Undo black king castling
        if (move.pieceType == MoveGenerator.blackKing)
        {
            if (move.ShortCastle)
            {
                board[5] = 0;
                board[7] = MoveGenerator.blackRook;
            }

            if (move.LongCastle)
            {
                board[3] = 0;
                board[0] = MoveGenerator.blackRook;
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
