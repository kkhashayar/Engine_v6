using Engine.Core;

namespace Engine.BItBoards;

public static class WhitePieceMoveGenerator
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

        // Loop through the board and look for white pieces
        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board[squareIndex];

            if (piece == MoveGenerator.whiteKing)
            {
                ulong kingPseudoMoves = Kings.GetWhiteKingMoves(squareIndex);
                List<MoveObject> kingMoves = Helpers.ConvertBitboardToMoveObjects(kingPseudoMoves, squareIndex, MoveGenerator.whiteKing);

                foreach (MoveObject move in kingMoves)
                {
                    if (IsWhiteMoveLegal(move, board)) moves.Add(move);
                }
            }

            else if (piece == MoveGenerator.whitePawn)
            {
                ulong whitePawn = 1UL << squareIndex; // bitboard with pawns position
                ulong occupiedSquares = whitePieces | blackPieces;

                ulong pawnPseudoMoves = Pawns.GetWhitePawnMoves(whitePawn, occupiedSquares);
                List<MoveObject> pawnMoves = Helpers.ConvertBitboardToMoveObjects(pawnPseudoMoves, squareIndex, MoveGenerator.whitePawn);

                // Generate attack moves for the pawn (including en passant)
                ulong enPassantTarget = (enPassantSquareIndex != -1) ? (1UL << enPassantSquareIndex) : 0UL;
                ulong pawnAttackMoves = Pawns.GetWhitePawnAttacks(whitePawn, blackPieces, enPassantTarget);
                List<MoveObject> pawnAttacks = Helpers.ConvertBitboardToMoveObjects(pawnAttackMoves, squareIndex, MoveGenerator.whitePawn);

                foreach (var move in pawnMoves)
                {
                    if (IsWhiteMoveLegal(move, board)) moves.Add(move);
                }
                // Add legal attack moves
                foreach (var move in pawnAttacks)
                {
                    if (IsWhiteMoveLegal(move, board)) moves.Add(move);
                }
            }
        }

        return moves;
    }

    public static List<int> GenerateWhiteAttackSquares(int[] board)
    {
        List<int> attackSquares = new();

        for (int squareIndex = 0; squareIndex < 64; squareIndex++)
        {
            int piece = board[squareIndex];

            if (piece == MoveGenerator.whiteKing)
            {
                ulong kingMoves = Kings.GetWhiteKingMoves(squareIndex);
                attackSquares.AddRange(Helpers.BitboardToSquares(kingMoves));
            }
            else if (piece == MoveGenerator.whitePawn)
            {
                ulong whitePawn = 1UL << squareIndex; // Bitboard with pawn's position
                ulong enPassantTarget = (enPassantSquareIndex != -1) ? (1UL << enPassantSquareIndex) : 0UL;
                ulong pawnAttacks = Pawns.GetWhitePawnAttacks(whitePawn, blackPieces, enPassantTarget);
                attackSquares.AddRange(Helpers.BitboardToSquares(pawnAttacks));
            }
        }

        return attackSquares;
    }
    public static bool IsWhiteMoveLegal(MoveObject move, int[] board)
    {
        // If the destination square is occupied by another white piece, the move is illegal
        if (Piece.IsWhite(board[move.EndSquare]))
            return false;

        // Create a shadow board for simulating the move
        int[] shadowBoard = new int[64];
        Array.Copy(board, shadowBoard, 64);

        // Apply the move to the shadow board
        MoveHandler.MakeMove(shadowBoard, move);

        // Get the white king's position on the shadow board
        int whiteKingPosition = Globals.GetWhiteKingSquare(shadowBoard);

        // Generate black attack squares on the shadow board
        List<int> blackAttacks = BlackPieceMoveGenerator.GenerateBlackAttackSquares(shadowBoard);

        // If the black attacks contain the white king's position, the move is illegal
        if (blackAttacks.Contains(whiteKingPosition))
            return false;

        // If the king is not in check, the move is legal
        return true;
    }

}
