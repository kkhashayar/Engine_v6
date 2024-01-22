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
        int[] captureDirections = { direction - 1, direction + 1 }; // Diagonal capture
        foreach (int captureDirection in captureDirections)
        {
            int potentialCaptureSquare = square + captureDirection;
            if (Globals.IsValidSquare(potentialCaptureSquare))
            {
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
                if(Globals.LastMoveWasPawn is true)
                {
                    //Console.WriteLine($"Last move is pawn {Globals.LastMoveWasPawn}");
                    //Thread.Sleep(2000);
                   
                    int currentPawnRank = Globals.BoardOfRanks[square];
                    if ((turn == 0 && currentPawnRank == 5) || (turn == 1 && currentPawnRank == 4))
                    {
                        if(currentPawnRank == 5)
                        {
                            enPassantSquare = Globals.LastendSquare - 8;
                        }
                        else if(currentPawnRank == 4)
                        {
                            enPassantSquare = Globals.LastendSquare + 8 ;
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
                                // board[Globals.LastendSquare] = 0;
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
            // Potential bug
            if (turn == 0)
            {
                moves.AddRange(new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteQueen, IsPromotion = true},
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteRook, IsPromotion = true },
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteBishop, IsPromotion = true },
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.whiteKnight, IsPromotion = true }
                });

            }
            // Bug: black pawn in original square before promotion changes to Knight :|
            else if (turn == 1)
            {
                moves.AddRange(new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackQueen, IsPromotion = true },
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackRook, IsPromotion = true },
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackBishop , IsPromotion = true},
                    //new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = MoveGenerator.blackKnight, IsPromotion = true }
                });
            }

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
