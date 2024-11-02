
using System.Net.Quic;
using System.Numerics;
using Engine.Core;

namespace Engine.PieceMotions;

internal static class Rooks
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

        List<MoveObject> moves = new();

        if (turn == 0)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (Globals.IsCrossSliderPathClear(square, targetSquare, board))
                {
                    if (targetsquareColor == "White") continue;
                    else
                    {
                        moves.Add(new MoveObject
                        {
                            pieceType = MoveGenerator.whiteRook,
                            StartSquare = square,
                            EndSquare = targetSquare
                        });
                    }
                }
            }
            return moves;
        }
        else if (turn == 1)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (Globals.IsCrossSliderPathClear(square, targetSquare, board))
                {
                    if (targetsquareColor == "Black") continue;

                    else
                    {
                        moves.Add(new MoveObject
                        {
                            pieceType = MoveGenerator.blackRook,
                            StartSquare = square,
                            EndSquare = targetSquare
                        });
                    }
                }
            }
        }

        return moves;
    }

    public static List<int> GetMasksForSquare(int square)
    {
        List<int> squares = new();

        int[] directions = { 1, -1, 8, -8 }; // right, left, up, down
        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && !Globals.IsVerHorBreaksMask(currentSquare, direction, originalRank, originalFile))
            {
                squares.Add(currentSquare);
                currentSquare += direction;
            }
        }

        return squares;
    }


    public static List<MoveObject> GenerateMovesForSquareByBitboard(int square, int turn, int[] board)
    {
        List<MoveObject> moves = new List<MoveObject>();

        // Convert the board to bitboards
        ulong allPiecesBitboard = GetAllPiecesBitboard(board);
        ulong ownPiecesBitboard = GetOwnPiecesBitboard(board, turn);
        ulong enemyPiecesBitboard = GetEnemyPiecesBitboard(board, turn);

        // Generate rook attacks using bitboards
        ulong rookMovesBitboard = GenerateRookMoves(square, allPiecesBitboard);

        // Remove moves that capture own pieces
        rookMovesBitboard &= ~ownPiecesBitboard;

        // Iterate over each possible move
        while (rookMovesBitboard != 0)
        {
            int targetSquare = BitboardHelper.PopLeastSignificantBit(ref rookMovesBitboard);

            moves.Add(new MoveObject
            {
                pieceType = (turn == 0) ? MoveGenerator.whiteRook : MoveGenerator.blackRook,
                StartSquare = square,
                EndSquare = targetSquare
            });
        }

        return moves;
    }

    // Helper methods

    // Convert the board to a bitboard of all pieces
    private static ulong GetAllPiecesBitboard(int[] board)
    {
        ulong bitboard = 0UL;
        for (int i = 0; i < 64; i++)
        {
            if (board[i] != Piece.None)
            {
                bitboard |= 1UL << i;
            }
        }
        return bitboard;
    }

    // Convert the board to a bitboard of own pieces
    private static ulong GetOwnPiecesBitboard(int[] board, int turn)
    {
        ulong bitboard = 0UL;
        int[] ownPieces = (turn == 0) ? Piece.WhitePieces : Piece.BlackPieces;

        for (int i = 0; i < 64; i++)
        {
            if (ownPieces.Contains(board[i]))
            {
                bitboard |= 1UL << i;
            }
        }
        return bitboard;
    }

    // Convert the board to a bitboard of enemy pieces
    private static ulong GetEnemyPiecesBitboard(int[] board, int turn)
    {
        ulong bitboard = 0UL;
        int[] enemyPieces = (turn == 0) ? Piece.BlackPieces : Piece.WhitePieces;

        for (int i = 0; i < 64; i++)
        {
            if (enemyPieces.Contains(board[i]))
            {
                bitboard |= 1UL << i;
            }
        }
        return bitboard;
    }

    // Generate rook moves using magic bitboards or sliding attacks
    private static ulong GenerateRookMoves(int square, ulong allPiecesBitboard)
    {
        //sliding attack generation
        ulong attacks = 0UL;

        int[] directions = { 8, -8, 1, -1 }; // up, down, right, left

        foreach (int direction in directions)
        {
            int targetSquare = square;
            while (true)
            {
                targetSquare += direction;

                // Check if the target square is on the board
                if (targetSquare < 0 || targetSquare >= 64)
                    break;

                // Handle edge cases (wrap-around)
                if ((direction == 1 || direction == -1) && (targetSquare / 8 != (targetSquare - direction) / 8))
                    break;

                attacks |= 1UL << targetSquare;

                // If there's a piece blocking the path, stop in that direction
                if ((allPiecesBitboard & (1UL << targetSquare)) != 0)
                    break;
            }
        }

        return attacks;
    }
}

// Helper class for bitboard operations
internal static class BitboardHelper
{
    // Pop the least significant bit and return its index
    public static int PopLeastSignificantBit(ref ulong bitboard)
    {
        int index = BitOperations.TrailingZeroCount(bitboard);
        bitboard &= bitboard - 1; // Clear the least significant bit
        return index;
    }
}


