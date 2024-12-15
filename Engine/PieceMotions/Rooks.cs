
using System.Numerics;
using Engine.Core;

namespace Engine.PieceMotions;

internal static class Rooks
{
    public static List<int> GetMasksForSquare(int square)
    {
        //List<int> squares = new();

        //int[] directions = { 1, -1, 8, -8 }; // right, left, up, down
        //int originalRank = square / 8;
        //int originalFile = square % 8;

        //foreach (int direction in directions)
        //{
        //    int currentSquare = square + direction;
        //    while (Globals.IsValidSquare(currentSquare) && !Globals.IsVerHorBreaksMask(currentSquare, direction, originalRank, originalFile))
        //    {
        //        squares.Add(currentSquare);
        //        currentSquare += direction;
        //    }
        //}

        //return squares;
        return new List<int>(RookMoves[square]);
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
                pieceType = (turn == 0) ? 5 : 15,
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




    public static readonly int[][] RookMoves = new int[][]
{
    // 0 (a8)
    new int[]{1,2,3,4,5,6,7,8,16,24,32,40,48,56},
    // 1 (b8)
    new int[]{0,2,3,4,5,6,7,9,17,25,33,41,49,57},
    // 2 (c8)
    new int[]{0,1,3,4,5,6,7,10,18,26,34,42,50,58},
    // 3 (d8)
    new int[]{0,1,2,4,5,6,7,11,19,27,35,43,51,59},
    // 4 (e8)
    new int[]{0,1,2,3,5,6,7,12,20,28,36,44,52,60},
    // 5 (f8)
    new int[]{0,1,2,3,4,6,7,13,21,29,37,45,53,61},
    // 6 (g8)
    new int[]{0,1,2,3,4,5,7,14,22,30,38,46,54,62},
    // 7 (h8)
    new int[]{0,1,2,3,4,5,6,15,23,31,39,47,55,63},

    // 8 (a7)
    new int[]{0,9,10,11,12,13,14,15,16,24,32,40,48,56},
    // 9 (b7)
    new int[]{1,8,10,11,12,13,14,15,17,25,33,41,49,57},
    // 10 (c7)
    new int[]{2,8,9,11,12,13,14,15,18,26,34,42,50,58},
    // 11 (d7)
    new int[]{3,8,9,10,12,13,14,15,19,27,35,43,51,59},
    // 12 (e7)
    new int[]{4,8,9,10,11,13,14,15,20,28,36,44,52,60},
    // 13 (f7)
    new int[]{5,8,9,10,11,12,14,15,21,29,37,45,53,61},
    // 14 (g7)
    new int[]{6,8,9,10,11,12,13,15,22,30,38,46,54,62},
    // 15 (h7)
    new int[]{7,8,9,10,11,12,13,14,23,31,39,47,55,63},

    // 16 (a6)
    new int[]{0,8,17,18,19,20,21,22,23,24,32,40,48,56},
    // 17 (b6)
    new int[]{1,9,16,18,19,20,21,22,23,25,33,41,49,57},
    // 18 (c6)
    new int[]{2,10,16,17,19,20,21,22,23,26,34,42,50,58},
    // 19 (d6)
    new int[]{3,11,16,17,18,20,21,22,23,27,35,43,51,59},
    // 20 (e6)
    new int[]{4,12,16,17,18,19,21,22,23,28,36,44,52,60},
    // 21 (f6)
    new int[]{5,13,16,17,18,19,20,22,23,29,37,45,53,61},
    // 22 (g6)
    new int[]{6,14,16,17,18,19,20,21,23,30,38,46,54,62},
    // 23 (h6)
    new int[]{7,15,16,17,18,19,20,21,22,31,39,47,55,63},

    // 24 (a5)
    new int[]{0,8,16,25,26,27,28,29,30,31,32,40,48,56},
    // 25 (b5)
    new int[]{1,9,17,24,26,27,28,29,30,31,33,41,49,57},
    // 26 (c5)
    new int[]{2,10,18,24,25,27,28,29,30,31,34,42,50,58},
    // 27 (d5)
    new int[]{3,11,19,24,25,26,28,29,30,31,35,43,51,59},
    // 28 (e5)
    new int[]{4,12,20,24,25,26,27,29,30,31,36,44,52,60},
    // 29 (f5)
    new int[]{5,13,21,24,25,26,27,28,30,31,37,45,53,61},
    // 30 (g5)
    new int[]{6,14,22,24,25,26,27,28,29,31,38,46,54,62},
    // 31 (h5)
    new int[]{7,15,23,24,25,26,27,28,29,30,39,47,55,63},

    // 32 (a4)
    new int[]{0,8,16,24,33,34,35,36,37,38,39,40,48,56},
    // 33 (b4)
    new int[]{1,9,17,25,32,34,35,36,37,38,39,41,49,57},
    // 34 (c4)
    new int[]{2,10,18,26,32,33,35,36,37,38,39,42,50,58},
    // 35 (d4)
    new int[]{3,11,19,27,32,33,34,36,37,38,39,43,51,59},
    // 36 (e4)
    new int[]{4,12,20,28,32,33,34,35,37,38,39,44,52,60},
    // 37 (f4)
    new int[]{5,13,21,29,32,33,34,35,36,38,39,45,53,61},
    // 38 (g4)
    new int[]{6,14,22,30,32,33,34,35,36,37,39,46,54,62},
    // 39 (h4)
    new int[]{7,15,23,31,32,33,34,35,36,37,38,47,55,63},

    // 40 (a3)
    new int[]{0,8,16,24,32,41,42,43,44,45,46,47,48,56},
    // 41 (b3)
    new int[]{1,9,17,25,33,40,42,43,44,45,46,47,49,57},
    // 42 (c3)
    new int[]{2,10,18,26,34,40,41,43,44,45,46,47,50,58},
    // 43 (d3)
    new int[]{3,11,19,27,35,40,41,42,44,45,46,47,51,59},
    // 44 (e3)
    new int[]{4,12,20,28,36,40,41,42,43,45,46,47,52,60},
    // 45 (f3)
    new int[]{5,13,21,29,37,40,41,42,43,44,46,47,53,61},
    // 46 (g3)
    new int[]{6,14,22,30,38,40,41,42,43,44,45,47,54,62},
    // 47 (h3)
    new int[]{7,15,23,31,39,40,41,42,43,44,45,46,55,63},

    // 48 (a2)
    new int[]{0,8,16,24,32,40,49,50,51,52,53,54,55,56},
    // 49 (b2)
    new int[]{1,9,17,25,33,41,48,50,51,52,53,54,55,57},
    // 50 (c2)
    new int[]{2,10,18,26,34,42,48,49,51,52,53,54,55,58},
    // 51 (d2)
    new int[]{3,11,19,27,35,43,48,49,50,52,53,54,55,59},
    // 52 (e2)
    new int[]{4,12,20,28,36,44,48,49,50,51,53,54,55,60},
    // 53 (f2)
    new int[]{5,13,21,29,37,45,48,49,50,51,52,54,55,61},
    // 54 (g2)
    new int[]{6,14,22,30,38,46,48,49,50,51,52,53,55,62},
    // 55 (h2)
    new int[]{7,15,23,31,39,47,48,49,50,51,52,53,54,63},

    // 56 (a1)
    new int[]{0,8,16,24,32,40,48,57,58,59,60,61,62,63},
    // 57 (b1)
    new int[]{1,9,17,25,33,41,49,56,58,59,60,61,62,63},
    // 58 (c1)
    new int[]{2,10,18,26,34,42,50,56,57,59,60,61,62,63},
    // 59 (d1)
    new int[]{3,11,19,27,35,43,51,56,57,58,60,61,62,63},
    // 60 (e1)
    new int[]{4,12,20,28,36,44,52,56,57,58,59,61,62,63},
    // 61 (f1)
    new int[]{5,13,21,29,37,45,53,56,57,58,59,60,62,63},
    // 62 (g1)
    new int[]{6,14,22,30,38,46,54,56,57,58,59,60,61,63},
    // 63 (h1)
    new int[]{7,15,23,31,39,47,55,56,57,58,59,60,61,62}
};

    //public static List<int> GetMasksForSquare(int square)
    //{
    //    With the above hard-coded RookMoves array:
    //    return new List<int>(RookMoves[square]);
    //}

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


