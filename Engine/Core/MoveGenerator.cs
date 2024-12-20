using Engine.Core;
using Engine.PieceMotions;

namespace Engine;

public static class MoveGenerator
{
    static Result result = new();
    
    //////////////////////////////////////   ENGINE CORE LOOP 
    public static Result GenerateAllMoves(int[] chessBoard, int turn, bool filter = false)
    {
        /// Useful information for evaluation
        int whitePieces = chessBoard.Where(p => Piece.IsWhite(p)).Count();
        int blackPieces = chessBoard.Where(p => Piece.IsBlack(p)).Count();
        int totalPiecesOnTheBoard = whitePieces + blackPieces;

        if (totalPiecesOnTheBoard == 32)
        {
            result.GamePhase = Enums.GamePhase.Opening;
            result.CalculationTime = Globals.OpeningTime;
        }
        else if (totalPiecesOnTheBoard < 32 && totalPiecesOnTheBoard > 10)
        {
            result.GamePhase = Enums.GamePhase.MiddleGame;
            result.CalculationTime = Globals.MiddleGameTime;
        }
        else 
        {
            result.GamePhase = Enums.GamePhase.EndGame;
            result.CalculationTime = Globals.EndGameTime;
        } 

        result.Moves = new List<MoveObject>();  

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
                    
                    {

                        if (chessBoard[move.EndSquare] != 0)
                        {
                            move.IsCapture = true;

                        }
                        //move.IsCheck = IsMoveCheck(move, chessBoard, turn);

                        if (move.pieceType == 1)
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
                        result.Moves.Add(move);
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
                        //move.IsCheck = IsMoveCheck(move, chessBoard, turn);

                        if (move.pieceType == 11)
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

                        result.Moves.Add(move);
                    }
                }
            }
        }
        else
        {
            // If not filtering, just return all moves for the current turn
            if (turn == 0) result.Moves = whitePseudoMoves;
            else result.Moves = blackPseudoMoves;
        }
        return result;
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
                    case 99: pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));   // using new hard coded masks
                        break;
                    case 3:  pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard)); // using new hard coded masks
                        break;
                    case 5:  pseudoMoves.AddRange(Rooks.GenerateMovesForSquareByBitboard(square, turn, chessBoard));   
                        break;
                    case 4:  pseudoMoves.AddRange(Bishops.GenerateMovesForSquareByBitboard(square, turn, chessBoard)); 
                        break;
                    case 9:  pseudoMoves.AddRange(Queens.GenerateMovesForSquareByBitboard(square, turn, chessBoard));  
                        break;
                    case 1:  pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                        break;
                }
            }
            // Generate moves for black pieces

            else if (turn == 1)
            {
                switch (piece)
                {
                    case 109: pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));   // using new hard coded masks
                        break;
                    case 13:  pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard)); // using new hard coded masks 
                        break;
                    case 15:  pseudoMoves.AddRange(Rooks.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                    case 14:  pseudoMoves.AddRange(Bishops.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                    case 19:  pseudoMoves.AddRange(Queens.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                    case 11:
                        pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                        break;
                }
            }
        }
        return pseudoMoves;
    }

    // Legacy version 
    private static bool IsMoveLegal(MoveObject move, int[] board, int turn)
    {
        int[] shadowBoard = (int[])board.Clone();

        if (turn == 0)
        {
            if (move.LongCastle)
            {
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(shadowBoard, 1);
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 59 || bMove.EndSquare == 58 || bMove.EndSquare == 60)) return false;
            }
            else if (move.ShortCastle)
            {
                var blackResponseMovesonCastle = GeneratePseudoLegalMoves(shadowBoard, 1);
                if (blackResponseMovesonCastle.Any(bMove => bMove.EndSquare == 61 || bMove.EndSquare == 62 || bMove.EndSquare == 60)) return false;
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



        // In fact we don't even need this check for check :D 
        //move.IsCheck = IsMoveCheck(move, board, turn); // Set IsCheck property
        return true;
    }

    

    internal static bool IsLegalBitboardVer(int[] chessBoard, MoveObject move, int turn)
    {
        ulong wPawns = 0, wKnights = 0, wBishops = 0, wRooks = 0, wQueens = 0, wKing = 0;
        ulong bPawns = 0, bKnights = 0, bBishops = 0, bRooks = 0, bQueens = 0, bKing = 0;

        ulong allWhitePieces = 0, allBlackPieces = 0, allpieces = 0;

        // Creates Bitboard version of the given chess board. 
        for (int square = 0; square < 64; square++)
        {
            int piece = chessBoard[square];
            if (piece == 0) continue; // Fixed: Skip empty squares, not occupied ones.

            if (Piece.IsWhite(piece))
            {
                switch (piece)
                {
                    case 1: wPawns |= 1UL << square; break;
                    case 3: wKnights |= 1UL << square; break;
                    case 4: wBishops |= 1UL << square; break;
                    case 5: wRooks |= 1UL << square; break;
                    case 9: wQueens |= 1UL << square; break;
                    case 99: wKing |= 1UL << square; break;
                }
            }
            else if (Piece.IsBlack(piece))
            {
                switch (piece)
                {
                    case 11: bPawns |= 1UL << square; break;
                    case 13: bKnights |= 1UL << square; break;
                    case 14: bBishops |= 1UL << square; break;
                    case 15: bRooks |= 1UL << square; break;
                    case 19: bQueens |= 1UL << square; break;
                    case 109: bKing |= 1UL << square; break;
                }
            }
        }

        // Current state of positions for all pieces
        allWhitePieces = wPawns | wKnights | wBishops | wRooks | wQueens | wKing;
        allBlackPieces = bPawns | bKnights | bBishops | bRooks | bQueens | bKing;
        allpieces = allWhitePieces | allBlackPieces;

        // Identify the moving piece bitboard
        ulong pieceBitboard = 0;
        if (Piece.IsWhite(move.pieceType))
        {
            switch (move.pieceType)
            {
                case 1: pieceBitboard = wPawns; break;
                case 3: pieceBitboard = wKnights; break;
                case 4: pieceBitboard = wBishops; break;
                case 5: pieceBitboard = wRooks; break;
                case 9: pieceBitboard = wQueens; break;
                case 99: pieceBitboard = wKing; break;
            }
        }
        else if (Piece.IsBlack(move.pieceType))
        {
            switch (move.pieceType)
            {
                case 11: pieceBitboard = bPawns; break;
                case 13: pieceBitboard = bKnights; break;
                case 14: pieceBitboard = bBishops; break;
                case 15: pieceBitboard = bRooks; break;
                case 19: pieceBitboard = bQueens; break;
                case 109: pieceBitboard = bKing; break;
            }
        }

        // Clear start square
        pieceBitboard &= ~(1UL << move.StartSquare);
        // Set end square
        pieceBitboard |= 1UL << move.EndSquare;

        // Reassign the updated piece bitboard back
        if (Piece.IsWhite(move.pieceType))
        {
            switch (move.pieceType)
            {
                case 1: wPawns = pieceBitboard; break;
                case 3: wKnights = pieceBitboard; break;
                case 4: wBishops = pieceBitboard; break;
                case 5: wRooks = pieceBitboard; break;
                case 9: wQueens = pieceBitboard; break;
                case 99: wKing = pieceBitboard; break;
            }
            allWhitePieces = wPawns | wKnights | wBishops | wRooks | wQueens | wKing;
        }
        else
        {
            switch (move.pieceType)
            {
                case 11: bPawns = pieceBitboard; break;
                case 13: bKnights = pieceBitboard; break;
                case 14: bBishops = pieceBitboard; break;
                case 15: bRooks = pieceBitboard; break;
                case 19: bQueens = pieceBitboard; break;
                case 109: bKing = pieceBitboard; break;
            }
            allBlackPieces = bPawns | bKnights | bBishops | bRooks | bQueens | bKing;
        }
        allpieces &= ~(1UL << move.StartSquare);
        allpieces |= 1UL << move.EndSquare;

        // Update king position if it moves
        ulong kingPosition = (turn == 0) ? wKing : bKing;
        if (move.pieceType == 99 || move.pieceType == 109)
        {
            kingPosition = (1UL << move.EndSquare);

            // Check if the king's destination is adjacent to the opponent's king
            ulong opponentKing = (turn == 0) ? bKing : wKing;
            ulong opponentKingAttacks = GenerateKingAttacks(opponentKing);

            if ((kingPosition & opponentKingAttacks) != 0)
            {
                return false; // Move is illegal as the king moves adjacent to the opponent's king
            }
        }

        // Check if move leaves the king in check
        bool check = IsKingInCheck(kingPosition, allpieces, allWhitePieces, allBlackPieces, turn, move, chessBoard);
        return !check;
    }
    private static ulong GenerateKingAttacks(ulong kingPosition)
    {
        ulong attacks = 0;
        attacks |= (kingPosition << 8); // Up
        attacks |= (kingPosition >> 8); // Down
        attacks |= (kingPosition << 1) & ~0x8080808080808080UL; // Right
        attacks |= (kingPosition >> 1) & ~0x0101010101010101UL; // Left
        attacks |= (kingPosition << 9) & ~0x8080808080808080UL; // Up-Right
        attacks |= (kingPosition << 7) & ~0x0101010101010101UL; // Up-Left
        attacks |= (kingPosition >> 7) & ~0x8080808080808080UL; // Down-Right
        attacks |= (kingPosition >> 9) & ~0x0101010101010101UL; // Down-Left
        return attacks;
    }
    private static bool IsKingInCheck(ulong kingPosition, ulong allPieces, ulong whitePieces, ulong blackPieces,
                                  int turn, MoveObject move, int[] chessBoard)
    {
        // Get all attack squares in int format
        var allAttacks = GetAllAttacksForPiece(move, chessBoard, 1 - turn); // Switched turn to opponent's.

        // Convert the attack squares to ulong format
        ulong attackBitboard = 0;
        foreach (var square in allAttacks)
        {
            attackBitboard |= 1UL << square; // Set the corresponding bit
        }

        // Check if the king's position overlaps with attackBitboard
        return (kingPosition & attackBitboard) != 0;
    }



    public static List<int> GetAllAttacksForPiece(MoveObject move, int[] board, int turn)
    {
        int piece = move.pieceType;
        int square = move.EndSquare;

        List<int> attacks = new List<int>();

        switch (piece)
        {
            case 9:
            case 19:
                attacks = Queens.GenerateMovesForSquareByBitboard(square, turn, board).Select(m => m.EndSquare).ToList();
                break;

            case 5:
            case 15:
                attacks = Rooks.GenerateMovesForSquareByBitboard(square, turn, board).Select(m => m.EndSquare).ToList();
                break;

            case 4:
            case 14:
                attacks = Bishops.GenerateMovesForSquareByBitboard(square, turn, board).Select(m => m.EndSquare).ToList();
                break;

            case 3:
            case 13:
                attacks = Knights.GenerateMovesForSquareByBitboard(square, turn, board).Select(m => m.EndSquare).ToList();
                break;

            default:
                break;
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
    private static int[] ApplyMove(int[] board, MoveObject move, int turn)
    {
        int[] shadowBoard = (int[])board.Clone();
        MoveHandler.RegisterStaticStates();
        MoveHandler.MakeMove(shadowBoard, move, turn);
        return shadowBoard;
    }

}