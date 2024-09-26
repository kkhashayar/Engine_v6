using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Bitboards;

public static class BlackPieceMoveGenerator
{
    public static int enPassantSquareIndex = -1;
    public static ulong whitePieces = 0UL;
    public static ulong blackPieces = 0UL;

    public static List<MoveObject> GetMoves(int[] board)
    {
        List<MoveObject> moves = new();

        // Generate bitboards for white and black pieces
        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board[squareIndex];

            if (Piece.IsWhite(piece)) whitePieces |= 1UL << squareIndex;

            if (Piece.IsBlack(piece)) blackPieces |= 1UL << squareIndex;
        }

        // Loop through the board and look for black pieces
        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board[squareIndex];

            if (piece == MoveGenerator.blackKing)
            {
                ulong kingPseudoMoves = Kings.GetBlackKingMoves(squareIndex);
                List<MoveObject> kingMoves = Helpers.ConvertBitboardToMoveObjects(kingPseudoMoves, squareIndex, MoveGenerator.blackKing);

                foreach (MoveObject move in kingMoves)
                {
                    if (IsBlackMoveLegal(move, board)) moves.Add(move);
                }
            }

            else if (piece == MoveGenerator.blackPawn)
            {
                ulong blackPawn = 1UL << squareIndex; // Bitboard with pawns position
                ulong occupiedSquares = whitePieces | blackPieces;

                // Generate quiet moves for the pawn
                ulong pawnPseudoMoves = Pawns.GetBlackPawnMoves(blackPawn, occupiedSquares);
                List<MoveObject> pawnMoves = Helpers.ConvertBitboardToMoveObjects(pawnPseudoMoves, squareIndex, MoveGenerator.blackPawn);

                // Generate attack moves for the pawn (including en passant)
                ulong enPassantTarget = (enPassantSquareIndex != -1) ? (1UL << enPassantSquareIndex) : 0UL;
                ulong pawnAttackMoves = Pawns.GetBlackPawnAttacks(blackPawn, whitePieces, enPassantTarget);
                List<MoveObject> pawnAttacks = Helpers.ConvertBitboardToMoveObjects(pawnAttackMoves, squareIndex, MoveGenerator.blackPawn);

                foreach (var move in pawnMoves)
                {
                    if (IsBlackMoveLegal(move, board)) moves.Add(move);
                }

                foreach (var move in pawnAttacks)
                {
                    if (IsBlackMoveLegal(move, board)) moves.Add(move);
                }
            }
        }

        return moves;
    }

    public static List<int> GenerateBlackAttackSquares(int[] board)
    {
        List<int> attackSquares = new();

        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board[squareIndex];

            if (piece == MoveGenerator.blackKing)  // Black King
            {
                ulong kingMoves = Kings.GetBlackKingMoves(squareIndex);
                attackSquares.AddRange(Helpers.BitboardToSquares(kingMoves));
            }
            else if (piece == MoveGenerator.blackPawn)
            {
                ulong blackPawn = 1UL << squareIndex; // Bitboard with pawn's position
                ulong enPassantTarget = (enPassantSquareIndex != -1) ? (1UL << enPassantSquareIndex) : 0UL;
                ulong pawnAttacks = Pawns.GetBlackPawnAttacks(blackPawn, whitePieces, enPassantTarget);
                attackSquares.AddRange(Helpers.BitboardToSquares(pawnAttacks));
            }
        }

        return attackSquares;
    }

    public static bool IsBlackMoveLegal(MoveObject move, int[] board)
    {
        // If the destination square is occupied by another black piece, the move is illegal
        if (Piece.IsBlack(board[move.EndSquare]))
            return false;

        // Create a shadow board for simulating the move
        int[] shadowBoard = new int[64];
        Array.Copy(board, shadowBoard, 64);

        // Apply the move to the shadow board
        MoveHandler.MakeMove(shadowBoard, move);

        // Get the black king's position on the shadow board
        int blackKingPosition = Globals.GetBlackKingSquare(shadowBoard);

        // Generate white attack squares on the shadow board
        List<int> whiteAttacks = WhitePieceMoveGenerator.GenerateWhiteAttackSquares(shadowBoard);

        // If the white attacks contain the black king's position, the move is illegal
        if (whiteAttacks.Contains(blackKingPosition))
            return false;

        // If the king is not in check, the move is legal
        return true;
    }
}

