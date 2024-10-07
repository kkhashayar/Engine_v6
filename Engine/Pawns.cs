namespace Engine;

internal static class Pawns
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board, int enPassantSquare = -1)
    {
        List<MoveObject> moves = new List<MoveObject>();
        int direction = turn == 0 ? -8 : 8; // White moves up, Black moves down
        int startRank = turn == 0 ? 6 : 1; // Starting ranks for White and Black
        int promotionRank = turn == 0 ? 0 : 7; // Promotion ranks for White and Black

        // Forward movement
        int forwardSquare = square + direction;
        if (Globals.IsValidSquare(forwardSquare) && board[forwardSquare] == 0)
        {
            AddPawnMove(square, forwardSquare, promotionRank, moves, turn);

            // Double move from starting position
            if ((square / 8) == startRank)
            {
                int doubleForwardSquare = square + 2 * direction;
                if (board[doubleForwardSquare] == 0)
                {
                    AddPawnMove(square, doubleForwardSquare, promotionRank, moves, turn);
                }
            }
        }

        // Capturing
        int[] captureDirections = (turn == 0) ? new int[] { -9, -7 } : new int[] { 7, 9 };
        foreach (int captureDirection in captureDirections)
        {
            int potentialCaptureSquare = square + captureDirection;
            if (Globals.IsValidSquare(potentialCaptureSquare))
            {
                // Prevent wrap-around from `a` or `h` file
                if ((turn == 0 && ((captureDirection == -9 && square % 8 == 0) || (captureDirection == -7 && (square + 1) % 8 == 0))) ||
                    (turn == 1 && ((captureDirection == 7 && square % 8 == 0) || (captureDirection == 9 && (square + 1) % 8 == 0))))
                {
                    continue;
                }

                // Regular capture
                if (board[potentialCaptureSquare] != 0)
                {
                    string colorOfCapturedPiece = Piece.GetColor(board[potentialCaptureSquare]);
                    if ((turn == 0 && colorOfCapturedPiece == "Black") || (turn == 1 && colorOfCapturedPiece == "White"))
                    {
                        AddPawnMove(square, potentialCaptureSquare, promotionRank, moves, turn);
                    }
                }

                // En Passant capture
                if (Globals.LastMoveWasPawn)
                {
                    int currentPawnRank = Globals.BoardOfRanks[square];
                    if ((turn == 0 && currentPawnRank == 5) || (turn == 1 && currentPawnRank == 4))
                    {
                        if (turn == 0)
                        {
                            enPassantSquare = Globals.LastEndSquare - 8;
                        }
                        else if (turn == 1)
                        {
                            enPassantSquare = Globals.LastEndSquare + 8;
                        }
                        if (potentialCaptureSquare == enPassantSquare)
                        {
                            int pawnBeingCapturedSquare = turn == 0 ? enPassantSquare + 8 : enPassantSquare - 8;
                            if (board[pawnBeingCapturedSquare] == (turn == 0 ? MoveGenerator.blackPawn : MoveGenerator.whitePawn))
                            {
                                MoveObject enPassantMove = new MoveObject
                                {
                                    pieceType = turn == 0 ? MoveGenerator.whitePawn : MoveGenerator.blackPawn,
                                    StartSquare = square,
                                    EndSquare = enPassantSquare,
                                    IsEnPassant = true
                                };
                                moves.Add(enPassantMove);
                            }
                        }
                    }
                }
            }
        }

        return moves;
    }

    private static void AddPawnMove(int startSquare, int endSquare, int promotionRank, List<MoveObject> moves, int turn)
    {
        if ((endSquare / 8) == promotionRank)
        {
            MoveObject[] promotionMoves;
            if (turn == 0)
            {
                promotionMoves = new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteQueen, IsPromotion = true, PromotionPiece = MoveGenerator.whiteQueen},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteRook, IsPromotion = true, PromotionPiece = MoveGenerator.whiteRook},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteBishop, IsPromotion = true, PromotionPiece = MoveGenerator.whiteBishop},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteKnight, IsPromotion = true, PromotionPiece = MoveGenerator.whiteKnight}
                };
            }
            else
            {
                promotionMoves = new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackQueen, IsPromotion = true, PromotionPiece = MoveGenerator.blackQueen},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackRook, IsPromotion = true, PromotionPiece = MoveGenerator.blackRook},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackBishop, IsPromotion = true, PromotionPiece = MoveGenerator.blackBishop},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackKnight, IsPromotion = true, PromotionPiece = MoveGenerator.blackKnight}
                };
            }
            moves.AddRange(promotionMoves);
        }
        else
        {
            // Regular pawn move
            moves.Add(new MoveObject
            {
                pieceType = turn == 0 ? MoveGenerator.whitePawn : MoveGenerator.blackPawn,
                StartSquare = startSquare,
                EndSquare = endSquare
            });
        }
    }
}