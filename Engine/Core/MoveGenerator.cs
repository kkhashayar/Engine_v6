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



    static Globals globals = new();
    static Result result = new Result();
    
    //////////////////////////////////////   ENGINE CORE LOOP 

    public static Result GenerateAllMoves(int[] chessBoard, int turn, bool filter = false)
    {
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
                    case 99: pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));   
                        break;
                    case 3:  pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard)); 
                        break;
                    case 5:  pseudoMoves.AddRange(Rooks.GenerateMovesForSquareByBitboard(square, turn, chessBoard));   
                        break;
                    case 4:  pseudoMoves.AddRange(Bishops.GenerateMovesForSquareByBitboard(square, turn, chessBoard)); 
                        break;
                    case 9:  pseudoMoves.AddRange(Queens.GenerateMovesForSquareByBitboard(square, turn, chessBoard));  
                        break;
                    case 1:  pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                        // pseudoMoves.AddRange(Pawns.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                }
            }
            else if (turn == 1)
            {
                switch (piece)
                {
                    case 109: pseudoMoves.AddRange(Kings.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
                        break;
                    case 13:  pseudoMoves.AddRange(Knights.GenerateMovesForSquareByBitboard(square, turn, chessBoard));
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

}