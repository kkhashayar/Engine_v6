using Engine.Core;
using Engine.PieceMotions;

namespace Engine;

public static class MoveGenerator
{
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
    public static readonly int None = Piece.None;



    static Globals globals = new Globals();

    //////////////////////////////////////   ENGINE CORE LOOP 

    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn, bool filter = false)
    {
        List<MoveObject> moves = new List<MoveObject>();

        List<MoveObject> whitePseudoMoves = GeneratePseudoLegalMoves(chessBoard, 0); // 0 for white
        List<MoveObject> blackPseudoMoves = GeneratePseudoLegalMoves(chessBoard, 1); // 1 for black

        if (filter is true)
        {
            if (turn == 0)
            {
                int blackKingPosition = Globals.GetBlackKingSquare(chessBoard);
                for (int i = 0; i < whitePseudoMoves.Count; i++)
                {
                    var move = whitePseudoMoves[i];
                    if (IsMoveLegal(move, chessBoard, turn))
                    //if (IsMoveLegalByBitboard(move, chessBoard, turn))
                    {

                        if (chessBoard[move.EndSquare] != 0)
                        {
                            move.IsCapture = true;

                        }
                        move.IsCheck = IsMoveCheck(move, chessBoard, turn);

                        if (move.pieceType == whitePawn)
                        {
                            var leftKillSquare = move.EndSquare - 7;
                            var rightKillSquare = move.EndSquare - 9;
                            if (Globals.IsValidSquare(leftKillSquare) && blackKingPosition == leftKillSquare)
                            {
                                move.IsCapture = true;
                            }

                            if (Globals.IsValidSquare(rightKillSquare) && blackKingPosition == rightKillSquare)
                            {
                                move.IsCapture = true;
                            }

                        }
                        moves.Add(move);
                    }
                }
            }
            else
            {
                int whiteKingPosition = Globals.GetWhiteKingSquare(chessBoard);
                for (int i = 0; i < blackPseudoMoves.Count; i++)
                {
                    var move = blackPseudoMoves[i];
                    if (IsMoveLegal(move, chessBoard, turn))
                    {

                        if (chessBoard[move.EndSquare] != 0)
                        {
                            move.IsCapture = true;
                        }
                        move.IsCheck = IsMoveCheck(move, chessBoard, turn);

                        if (move.pieceType == blackPawn)
                        {
                            var leftKillSquare = move.EndSquare + 7;
                            var rightKillSquare = move.EndSquare + 9;
                            if (Globals.IsValidSquare(leftKillSquare) && whiteKingPosition == leftKillSquare)
                            {
                                move.IsCapture = true;
                            }

                            if (Globals.IsValidSquare(rightKillSquare) && whiteKingPosition == rightKillSquare)
                            {
                                move.IsCapture = true;
                            }

                        }

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

    private static List<MoveObject> GeneratePseudoLegalMoves(int[] chessBoard, int turn)
    {
        List<MoveObject> pseudoMoves = new List<MoveObject>();

        for (int square = 0; square < 64; square++)
        {
            if (!Globals.IsValidSquare(square)) continue;

            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (turn == 0)
            {
                switch (piece)
                {
                    case 99:
                        //pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 3:
                        //pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 5:
                        //pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Rooks.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 4:
                        //pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Bishops.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 9:
                        //pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Queens.GenerateMovesForSquareByBitboard(square, turn, chessBoard));

                        break;

                    case 1:
                        pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                        // pseudoMoves.AddRange(Pawns.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                }
            }
            // Generate moves for black pieces
            else if (turn == 1)
            {
                switch (piece)
                {
                    case 109:
                        //pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 13:
                        //pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 15:
                        //pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Rooks.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 14:
                        //pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Bishops.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 19:
                        //pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                        pseudoMoves.AddRange(Queens.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;

                    case 11:
                        pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                        // pseudoMoves.AddRange(Pawns.GenerateMovesForSquareByBitboard(square, turn, chessBoard));

                        break;
                }
            }
        }


        return pseudoMoves;
    }


    public static List<MoveObject> GetKingAttacks(int[] board, int turn)
    {
        List<MoveObject> kingMoves = new List<MoveObject>();
        int KingPosition = turn == 0 ? Globals.GetWhiteKingSquare(board) : Globals.GetBlackKingSquare(board);

        var rowKingMoves = (Kings.GenerateMovesForSquare(KingPosition, turn, board));
        foreach (var move in rowKingMoves)
        {
            if (IsMoveLegal(move, board, turn)) { kingMoves.Add(move); }
        }
        return kingMoves;
    }

    // version 2 
    private static bool IsMoveLegal(MoveObject move, int[] board, int turn)
    {
        int[] shadowBoard = (int[])board.Clone();

        if (turn == 0)
        {
            if (move.LongCastle)
            {
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(shadowBoard, 1);
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 59 || bMove.EndSquare == 58)) return false;
            }
            else if (move.ShortCastle)
            {
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(shadowBoard, 1);
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 61 || bMove.EndSquare == 62)) return false;
            }

            MakeMove(move, shadowBoard);

            int whiteKingSquare = Globals.GetWhiteKingSquare(shadowBoard);
            var blackResponseMoves = GeneratePseudoLegalMoves(shadowBoard, 1);

            if (blackResponseMoves.Any(bMove => bMove.EndSquare == whiteKingSquare)) return false;


            return true;
        }

        // Black turn
        if (move.LongCastle)
        {
            var whiteResponseMovesCastle = GeneratePseudoLegalMoves(shadowBoard, 0);
            if (whiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 3 || wMove.EndSquare == 2 || wMove.EndSquare == 4)) return false;
        }
        else if (move.ShortCastle)
        {
            var whiteResponseMovesCastle = GeneratePseudoLegalMoves(shadowBoard, 0);
            if (whiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 5 || wMove.EndSquare == 6 || wMove.EndSquare == 4)) return false;
        }

        MakeMove(move, shadowBoard);

        int blackKingSquare = Globals.GetBlackKingSquare(shadowBoard);
        var whiteResponseMoves = GeneratePseudoLegalMoves(shadowBoard, 0);

        if (whiteResponseMoves.Any(wMove => wMove.EndSquare == blackKingSquare)) return false;

        move.IsCheck = IsMoveCheck(move, board, turn); // Set IsCheck property
        return true;
    }

    private static bool IsMoveCheck(MoveObject move, int[] board, int turn)
    {
        int[] shadowBoard = (int[])board.Clone();

        ApplyMove(shadowBoard, move);
        MoveHandler.RestoreStateFromSnapshot();

        int opponentKingPosition = -1;

        if (turn == 0) opponentKingPosition = Globals.GetBlackKingSquare(board);
        else if (turn == 1) opponentKingPosition = Globals.GetWhiteKingSquare(board);



        if (move.EndSquare == opponentKingPosition)
        {
            move.IsCheck = true;
            return true;
        }

        var pieceMoving = move.pieceType;


        if (turn == 0) opponentKingPosition = Globals.GetBlackKingSquare(shadowBoard);
        else if (turn == 1) opponentKingPosition = Globals.GetWhiteKingSquare(shadowBoard);

        var attacks = GetAllAttacksForPiece(move, shadowBoard, turn);

        if (attacks.Contains(opponentKingPosition))
        {
            move.IsCheck = true;
            return true;
        }


        return false;
    }


    private static List<int> GetAllAttacksForPiece(MoveObject move, int[] board, int turn)
    {
        int piece = move.pieceType;
        int square = move.EndSquare;

        List<int> attacks = new List<int>();

        if (piece == whiteQueen)
        {
            attacks = Queens.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == whiteRook)
        {
            attacks = Rooks.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == whiteBishop)
        {
            attacks = Bishops.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == whiteKnight)
        {
            attacks = Knights.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }

        else if (piece == blackQueen)
        {
            attacks = Queens.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == blackRook)
        {
            attacks = Rooks.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == blackBishop)
        {
            attacks = Bishops.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        else if (piece == blackKnight)
        {
            attacks = Knights.GenerateMovesForSquare(square, turn, board).Select(move => move.EndSquare).ToList();
        }
        return attacks;
    }


    // TODO: General check
    private static void MakeMove(MoveObject move, int[] board)
    {
        if (!Globals.IsValidSquare(move.StartSquare) || !Globals.IsValidSquare(move.EndSquare)) return;
        if (move.LongCastle || move.ShortCastle)
        {

        }

        else if (move.IsPromotion)
        {

        }

        else if (move.IsEnPassant)
        {

        }

        else
        {
            board[move.EndSquare] = move.pieceType;
            board[move.StartSquare] = 0;
        }
    }


    private static int[] ApplyMove(int[] board, MoveObject move)
    {
        int[] shadowBoard = (int[])board.Clone();
        MoveHandler.RegisterStaticStates();
        MoveHandler.MakeMove(shadowBoard, move);
        return shadowBoard;
    }

    private static bool IsMoveLegalByBitboard(MoveObject move, int[] board, int turn)
    {
        // Create a copy of the board to simulate the move
        int[] newBoard = (int[])board.Clone();

        // Apply the move to the copied board and update Globals accordingly
        ApplyMoveToBoard(newBoard, move, turn);

        // Create a new bitboard from the updated board
        ulong newBitboard = CreateBitboard(newBoard);

        // King position check
        int kingSquare = GetKingSquareFromBitboard(newBitboard, turn == 0, newBoard);
        if (kingSquare == -1)
        {
            Console.WriteLine("Error: Could not find king after move. Invalid move state.");
            return false; // King not found means the move left king in an illegal state.
        }

        // Generate opponent attacks to verify if the king is attacked
        ulong opponentAttacks = GenerateAttacksByBitboard(newBitboard, turn == 0 ? 1 : 0, newBoard);

        // If the king's square is under attack, move is illegal
        if ((opponentAttacks & (1UL << kingSquare)) != 0)
        {
            Console.WriteLine("Move left the king in check. Marking as illegal.");
            return false;
        }

        return true; // If none of the checks failed, the move is legal.
    }

    private static void ApplyMoveToBoard(int[] board, MoveObject move, int turn)
    {
        // Move the piece
        board[move.EndSquare] = board[move.StartSquare];
        board[move.StartSquare] = 0;

        // Handle captures
        if (move.IsCapture && !move.IsEnPassant)
        {
            board[move.EndSquare] = board[move.StartSquare];
        }
        else if (move.IsEnPassant)
        {
            if (turn == 0) // White captures en passant
            {
                board[move.EndSquare + 8] = 0; // Remove the black pawn
            }
            else // Black captures en passant
            {
                board[move.EndSquare - 8] = 0; // Remove the white pawn
            }
        }

        // Handle promotions
        if (move.IsPromotion)
        {
            board[move.EndSquare] = move.PromotionPiece;
        }

        // Handle castling: move the rook as well
        if (move.ShortCastle)
        {
            if (turn == 0) // White short castle
            {
                board[61] = board[63]; // Move rook from h1 to f1
                board[63] = 0;
            }
            else // Black short castle
            {
                board[5] = board[7]; // Move rook from h8 to f8
                board[7] = 0;
            }
        }
        else if (move.LongCastle)
        {
            if (turn == 0) // White long castle
            {
                board[58] = board[56]; // Move rook from a1 to d1
                board[56] = 0;
            }
            else // Black long castle
            {
                board[2] = board[0]; // Move rook from a8 to d8
                board[0] = 0;
            }
        }

        // Update castling rights
        UpdateCastlingRights(board, move, turn);

        // Update en passant square
        UpdateEnPassantSquare(board, move, turn);
    }

    private static void UpdateCastlingRights(int[] board, MoveObject move, int turn)
    {
        // If a king moves, lose all castling rights for that side
        if (move.pieceType == MoveGenerator.whiteKing)
        {
            Globals.WhiteShortCastle = false;
            Globals.WhiteLongCastle = false;
        }
        else if (move.pieceType == MoveGenerator.blackKing)
        {
            Globals.BlackShortCastle = false;
            Globals.BlackLongCastle = false;
        }

        // If a rook moves or is captured, lose the corresponding castling right
        if (move.StartSquare == 63 || move.EndSquare == 63) // White kingside rook
        {
            Globals.WhiteShortCastle = false;
        }
        if (move.StartSquare == 56 || move.EndSquare == 56) // White queenside rook
        {
            Globals.WhiteLongCastle = false;
        }
        if (move.StartSquare == 7 || move.EndSquare == 7) // Black kingside rook
        {
            Globals.BlackShortCastle = false;
        }
        if (move.StartSquare == 0 || move.EndSquare == 0) // Black queenside rook
        {
            Globals.BlackLongCastle = false;
        }

        // If a rook is captured, ensure the castling rights are updated
        if (move.IsCapture)
        {
            if (move.EndSquare == 63) // Captured white kingside rook
            {
                Globals.WhiteShortCastle = false;
            }
            if (move.EndSquare == 56) // Captured white queenside rook
            {
                Globals.WhiteLongCastle = false;
            }
            if (move.EndSquare == 7) // Captured black kingside rook
            {
                Globals.BlackShortCastle = false;
            }
            if (move.EndSquare == 0) // Captured black queenside rook
            {
                Globals.BlackLongCastle = false;
            }
        }
    }

    private static void UpdateEnPassantSquare(int[] board, MoveObject move, int turn)
    {
        // Reset en passant square by default
        Globals.LastEndSquare = -1;

        // If a pawn moves two squares forward, set the en passant square
        if (move.pieceType == MoveGenerator.whitePawn && move.StartSquare - move.EndSquare == 16)
        {
            Globals.LastEndSquare = move.StartSquare - 8;
        }
        else if (move.pieceType == MoveGenerator.blackPawn && move.EndSquare - move.StartSquare == 16)
        {
            Globals.LastEndSquare = move.StartSquare + 8;
        }
    }


    // Helper method: Create a bitboard representation from the board array
    private static ulong CreateBitboard(int[] board)
    {
        ulong bitboard = 0UL;

        for (int square = 0; square < 64; square++)
        {
            if (board[square] != 0)
            {
                bitboard |= (1UL << square);
            }
        }

        return bitboard;
    }

    // Helper method: Generate all potential attacks for a given turn using bitboards
    private static ulong GenerateAttacksByBitboard(ulong bitboard, int turn, int[] board)
    {
        ulong attacks = 0UL;

        for (int square = 0; square < 64; square++)
        {
            if (((bitboard >> square) & 1UL) != 0) // If there's a piece at this square
            {
                int piece = board[square];
                if (piece != MoveGenerator.None && ((turn == 0 && piece >= MoveGenerator.blackKing) || (turn == 1 && piece < MoveGenerator.blackKing)))
                {
                    Console.WriteLine($"Generating attacks for piece: {piece} at square: {square}");
                    // Directly implement attack generation based on the piece type
                    switch (piece)
                    {
                        case 99: // White King
                        case 109: // Black King
                            attacks |= GenerateKingAttacks(square);
                            break;

                        case 9: // White Queen
                        case 19: // Black Queen
                            attacks |= GenerateQueenAttacks(square, bitboard);
                            break;

                        case 5: // White Rook
                        case 15: // Black Rook
                            attacks |= GenerateRookAttacks(square, bitboard);
                            break;

                        case 4: // White Bishop
                        case 14: // Black Bishop
                            attacks |= GenerateBishopAttacks(square, bitboard);
                            break;

                        case 3: // White Knight
                        case 13: // Black Knight
                            attacks |= GenerateKnightAttacks(square);
                            break;

                        case 1: // White Pawn
                        case 11: // Black Pawn
                            attacks |= GeneratePawnAttacks(square, turn);
                            break;
                    }
                }
            }
        }

        Console.WriteLine($"Total attacks generated for turn {turn}: {Convert.ToString((long)attacks, 2).PadLeft(64, '0')}");
        return attacks;
    }


    // Helper method: Simulate the move on the bitboard to test legality
    private static ulong ApplyMoveByBitboard(ulong bitboard, MoveObject move)
    {
        ulong newBitboard = bitboard;
        newBitboard &= ~(1UL << move.StartSquare); // Clear the starting square
        newBitboard |= (1UL << move.EndSquare); // Set the end square

        return newBitboard;
    }

    // Helper method: Get the position of a king from the bitboard
    private static int GetKingSquareFromBitboard(ulong bitboard, bool isWhite, int[] board)
    {
        int piece = isWhite ? MoveGenerator.whiteKing : MoveGenerator.blackKing;

        for (int square = 0; square < 64; square++)
        {
            if (((bitboard >> square) & 1UL) != 0 && board[square] == piece)
            {
                return square;
            }
        }

        return -1; // King not found
    }

    // Generate attacks for specific pieces directly within the switch-case

    private static ulong GenerateKingAttacks(int square)
    {
        ulong attacks = 0UL;
        int[] directions = { 9, 8, 7, 1, -9, -7, -8, -1 };

        foreach (int direction in directions)
        {
            int targetSquare = square + direction;
            if (Globals.IsValidSquare(targetSquare) && IsWithinOneStep(square, targetSquare))
            {
                attacks |= (1UL << targetSquare);
            }
        }

        return attacks;
    }

    private static bool IsWithinOneStep(int originalSquare, int targetSquare)
    {
        int originalRank = originalSquare / 8;
        int originalFile = originalSquare % 8;
        int targetRank = targetSquare / 8;
        int targetFile = targetSquare % 8;

        return Math.Abs(targetRank - originalRank) <= 1 && Math.Abs(targetFile - originalFile) <= 1;
    }


    private static ulong GenerateQueenAttacks(int square, ulong bitboard)
    {
        return GenerateRookAttacks(square, bitboard) | GenerateBishopAttacks(square, bitboard);
    }

    private static ulong GenerateRookAttacks(int square, ulong bitboard)
    {
        ulong attacks = 0UL;
        int[] directions = { 8, -8, 1, -1 }; // Vertical and horizontal directions

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare))
            {
                attacks |= (1UL << currentSquare);
                if ((bitboard & (1UL << currentSquare)) != 0) break; // Stop if there's a piece
                currentSquare += direction;
            }
        }

        return attacks;
    }

    private static ulong GenerateBishopAttacks(int square, ulong bitboard)
    {
        ulong attacks = 0UL;
        int[] directions = { 9, 7, -7, -9 }; // Diagonal directions

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare))
            {
                attacks |= (1UL << currentSquare);
                if ((bitboard & (1UL << currentSquare)) != 0) break; // Stop if there's a piece
                currentSquare += direction;
            }
        }

        return attacks;
    }

    private static ulong GenerateKnightAttacks(int square)
    {
        ulong attacks = 0UL;
        (int, int)[] knightMoves = { (2, 1), (1, 2), (-1, 2), (-2, 1), (-2, -1), (-1, -2), (1, -2), (2, -1) };
        int originalRow = square / 8;
        int originalCol = square % 8;

        foreach (var (rowChange, colChange) in knightMoves)
        {
            int newRow = originalRow + rowChange;
            int newCol = originalCol + colChange;

            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                int targetSquare = newRow * 8 + newCol;
                if (Globals.IsValidSquare(targetSquare))
                {
                    attacks |= (1UL << targetSquare);
                }
            }
        }

        return attacks;
    }

    private static ulong GeneratePawnAttacks(int square, int turn)
    {
        ulong attacks = 0UL;
        int direction = (turn == 0) ? -8 : 8;
        int[] captureDirections = (turn == 0) ? new int[] { -9, -7 } : new int[] { 7, 9 };

        foreach (int captureDirection in captureDirections)
        {
            int targetSquare = square + captureDirection;
            if (Globals.IsValidSquare(targetSquare))
            {
                attacks |= (1UL << targetSquare);
            }
        }

        return attacks;
    }

}