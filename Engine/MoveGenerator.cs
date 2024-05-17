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

    public static bool WhiteKingIsCheck { get; set; } = false;
    public static bool BlackKingIsCheck { get; set; } = false;

    public static List<int>? BlackDefendedSquares { get; set; }
    public static List<int>? WhiteDefendedSquares { get; set; }

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
                    {
                        move.Priority = 1;
                        if (chessBoard[move.EndSquare] != 0) 
                        {
                          
                            move.Priority = 2;
                        }  
                        if(move.EndSquare == blackKingPosition)
                        {
                            move.Priority = 3;
                        }
                        if(move.pieceType == MoveGenerator.whitePawn)
                        {
                            var leftKillSquare = move.EndSquare - 7; 
                            var rightKillSquare = move.EndSquare - 9;
                            if(Globals.IsValidSquare(leftKillSquare) && blackKingPosition == leftKillSquare)
                            {
                                move.Priority = 3;
                            }

                            if(Globals.IsValidSquare(rightKillSquare) && blackKingPosition == rightKillSquare)
                            {
                                move.Priority = 3;
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

                        move.Priority = 1;
                        if (chessBoard[move.EndSquare] != 0)
                        {
                            move.Priority = 2;
                        }
                        if(move.EndSquare == whiteKingPosition)
                        {
                            move.Priority = 3;  
                        }
                        if(move.pieceType == MoveGenerator.blackPawn)
                        {
                            var leftKillSquare = move.EndSquare + 7; 
                            var rightKillSquare = move.EndSquare + 9;
                            if(Globals.IsValidSquare(leftKillSquare) && whiteKingPosition == leftKillSquare)
                            {
                                move.Priority = 3;
                            }

                            if(Globals.IsValidSquare(rightKillSquare) && whiteKingPosition == rightKillSquare)
                            {
                                move.Priority = 3;
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

        //WhiteDefendedSquares = new();
        //BlackDefendedSquares = new();

        for (int square = 0; square < 64; square++)
        {
            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (turn == 0)
            {
                if (piece == whiteKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == whiteBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == whiteQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == whitePawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                }

            }
            // Generate moves for black pieces
            else if (turn == 1)
            {
                if (piece == blackKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }

                else if (piece == blackPawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                }
            }
        }
        if (turn == 0)
        {
            if (pseudoMoves.Count == 0 || pseudoMoves is null) { Globals.CheckmateWhite = true; }
        }
        else if (turn == 1)
        {
            if (pseudoMoves.Count == 0 || pseudoMoves is null) { Globals.CheckmateBlack = true; }
        }
        return pseudoMoves;
    }


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

        ///////////////////////////////////////////////  BLACK TURN 
        if (move.LongCastle)
        {
            var WhiteResponseMovesCastle = GeneratePseudoLegalMoves(shadowBoard, 0);
            if (WhiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 3 || wMove.EndSquare == 2 || wMove.EndSquare == 4)) return false;
        }
        else if (move.ShortCastle)
        {
            var WhiteResponseMovesCastle = GeneratePseudoLegalMoves(shadowBoard, 0);
            if (WhiteResponseMovesCastle.Any(wMove => wMove.EndSquare == 5 || wMove.EndSquare == 6 || wMove.EndSquare == 4)) return false;
        }


        MakeMove(move, shadowBoard);

        int blackKingSquare = Globals.GetBlackKingSquare(shadowBoard);

        var WhiteResponseMoves = GeneratePseudoLegalMoves(shadowBoard, 0);

        if (WhiteResponseMoves.Any(wMove => wMove.EndSquare == blackKingSquare)) return false;

        return true;
    }



    private static void MakeMove(MoveObject move, int[] board)
    {
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
}


