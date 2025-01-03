﻿
using Engine.Core;

namespace Engine.PieceMotions;

internal static class Pawns
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board, int enPassantSquare = -1)
    {
        List<MoveObject> moves = new List<MoveObject>();
        int direction = turn == 0 ? -8 : 8; // White moves up, Black moves down
        int startRank = turn == 0 ? 6 : 1; // Starting ranks for White and Black
        int promotionRank = turn == 0 ? 0 : 8; // Promotion ranks for White and Black

        // Forward movement
        int forwardSquare = square + direction;
        if (Globals.IsValidSquare(forwardSquare) && board[forwardSquare] == 0)
        {
            AddPawnMove(square, forwardSquare, promotionRank, moves, turn);

            // Double move from starting position
            if (square / 8 == startRank)
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
                if (Globals.PawnJumpCaptureSquares.Contains(square) && Globals.PawnJumpCaptureSquares.Contains(potentialCaptureSquare)) continue;
                if (board[potentialCaptureSquare] != 0)
                {
                    string colorOfCapturedPiece = Piece.GetColor(board[potentialCaptureSquare]);
                    if (turn == 0 && colorOfCapturedPiece == "Black" || turn == 1 && colorOfCapturedPiece == "White")
                    {
                        AddPawnMove(square, potentialCaptureSquare, promotionRank, moves, turn);
                    }
                }

                // En Passant capture
                if (Globals.LastMoveWasPawn is true)
                {
                    int currentPawnRank = Globals.BoardOfRanks[square];
                    if (turn == 0 && currentPawnRank == 5 || turn == 1 && currentPawnRank == 4)
                    {
                        if (currentPawnRank == 5)
                        {
                            enPassantSquare = Globals.LastEndSquare - 8;
                        }
                        else if (currentPawnRank == 4)
                        {
                            enPassantSquare = Globals.LastEndSquare + 8;
                        }
                        if (potentialCaptureSquare == enPassantSquare)
                        {
                            int pawnBeingCapturedSquare = turn == 0 ? enPassantSquare + 8 : enPassantSquare - 8;
                            if (board[pawnBeingCapturedSquare] == (turn == 0 ? -1 : 1))
                            {
                                MoveObject enPassantMove = new MoveObject
                                {
                                    pieceType = turn == 0 ? 1 : -1,
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
        if (endSquare / 8 == 0 || endSquare / 8 == 7)
        {
            if (turn == 0)
            {
                moves.AddRange(new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = 1, IsPromotion = true, PromotionPiece = 9},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = 1, IsPromotion = true, PromotionPiece = 5},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = 1, IsPromotion = true, PromotionPiece = 4},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = 1, IsPromotion = true, PromotionPiece = 3}
                });
            }
            else if (turn == 1)
            {
                moves.AddRange(new[]
                {
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = -1, IsPromotion = true, PromotionPiece = -9},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = -1, IsPromotion = true, PromotionPiece = -5},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = -1, IsPromotion = true, PromotionPiece = -4},
                    new MoveObject { StartSquare = startSquare, EndSquare = endSquare, pieceType = -1, IsPromotion = true, PromotionPiece = -3}
                });
            }
        }
        else
        {
            // Regular pawn move
            moves.Add(new MoveObject
            {
                pieceType = turn == 0 ? 1 : -1,
                StartSquare = startSquare,
                EndSquare = endSquare
            });
        }
    }
}
