using Engine.Core;

namespace Engine.PieceMotions;

internal static class Rooks
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square, board); // Pass board for path checking

        List<MoveObject> moves = new();

        if (turn == 0) // White's turn
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetSquareColor = Piece.GetColor(board[targetSquare]);

                // Skip squares occupied by friendly pieces
                if (targetSquareColor == "White") continue;

                // Add the valid move for the white rook
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.whiteRook,
                    StartSquare = square,
                    EndSquare = targetSquare
                });
            }
        }
        else if (turn == 1) // Black's turn
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetSquareColor = Piece.GetColor(board[targetSquare]);

                // Skip squares occupied by friendly pieces
                if (targetSquareColor == "Black") continue;

                // Add the valid move for the black rook
                moves.Add(new MoveObject
                {
                    pieceType = MoveGenerator.blackRook,
                    StartSquare = square,
                    EndSquare = targetSquare
                });
            }
        }

        return moves;
    }

    public static List<int> GetMasksForSquare(int square, int[] board)
    {
        List<int> potentialSquares = RookMovesTable[square].ToList(); // Get all possible moves from the hardcoded table
        List<int> validSquares = new List<int>();

        foreach (int targetSquare in potentialSquares)
        {
            // Check if the path between the current square and the target square is clear
            if (Globals.IsCrossSliderPathClear(square, targetSquare, board))
            {
                // Add the square to the valid list
                validSquares.Add(targetSquare);

                // Stop moving further if there's a piece on the target square (either enemy or friendly)
                if (board[targetSquare] != 0) break;
            }
        }

        return validSquares;
    }


    private static readonly int[][] RookMovesTable = new int[64][]
{
    // a8 (index 0)
    new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 16, 24, 32, 40, 48, 56 },
    
    // b8 (index 1)
    new int[] { 0, 2, 3, 4, 5, 6, 7, 9, 17, 25, 33, 41, 49, 57 },
    
    // c8 (index 2)
    new int[] { 0, 1, 3, 4, 5, 6, 7, 10, 18, 26, 34, 42, 50, 58 },
    
    // d8 (index 3)
    new int[] { 0, 1, 2, 4, 5, 6, 7, 11, 19, 27, 35, 43, 51, 59 },
    
    // e8 (index 4)
    new int[] { 0, 1, 2, 3, 5, 6, 7, 12, 20, 28, 36, 44, 52, 60 },
    
    // f8 (index 5)
    new int[] { 0, 1, 2, 3, 4, 6, 7, 13, 21, 29, 37, 45, 53, 61 },
    
    // g8 (index 6)
    new int[] { 0, 1, 2, 3, 4, 5, 7, 14, 22, 30, 38, 46, 54, 62 },
    
    // h8 (index 7)
    new int[] { 0, 1, 2, 3, 4, 5, 6, 15, 23, 31, 39, 47, 55, 63 },
    
    // a7 (index 8)
    new int[] { 0, 16, 24, 32, 40, 48, 56, 9, 10, 11, 12, 13, 14, 15 },
    
    // b7 (index 9)
    new int[] { 1, 17, 25, 33, 41, 49, 57, 8, 10, 11, 12, 13, 14, 15 },
    
    // c7 (index 10)
    new int[] { 2, 18, 26, 34, 42, 50, 58, 8, 9, 11, 12, 13, 14, 15 },
    
    // d7 (index 11)
    new int[] { 3, 19, 27, 35, 43, 51, 59, 8, 9, 10, 12, 13, 14, 15 },
    
    // e7 (index 12)
    new int[] { 4, 20, 28, 36, 44, 52, 60, 8, 9, 10, 11, 13, 14, 15 },
    
    // f7 (index 13)
    new int[] { 5, 21, 29, 37, 45, 53, 61, 8, 9, 10, 11, 12, 14, 15 },
    
    // g7 (index 14)
    new int[] { 6, 22, 30, 38, 46, 54, 62, 8, 9, 10, 11, 12, 13, 15 },
    
    // h7 (index 15)
    new int[] { 7, 23, 31, 39, 47, 55, 63, 8, 9, 10, 11, 12, 13, 14 },
    
    // a6 (index 16)
    new int[] { 0, 8, 24, 32, 40, 48, 56, 17, 18, 19, 20, 21, 22, 23 },
    
    // b6 (index 17)
    new int[] { 1, 9, 25, 33, 41, 49, 57, 16, 18, 19, 20, 21, 22, 23 },
    
    // c6 (index 18)
    new int[] { 2, 10, 26, 34, 42, 50, 58, 16, 17, 19, 20, 21, 22, 23 },
    
    // d6 (index 19)
    new int[] { 3, 11, 27, 35, 43, 51, 59, 16, 17, 18, 20, 21, 22, 23 },
    
    // e6 (index 20)
    new int[] { 4, 12, 28, 36, 44, 52, 60, 16, 17, 18, 19, 21, 22, 23 },
    
    // f6 (index 21)
    new int[] { 5, 13, 29, 37, 45, 53, 61, 16, 17, 18, 19, 20, 22, 23 },
    
    // g6 (index 22)
    new int[] { 6, 14, 30, 38, 46, 54, 62, 16, 17, 18, 19, 20, 21, 23 },
    
    // h6 (index 23)
    new int[] { 7, 15, 31, 39, 47, 55, 63, 16, 17, 18, 19, 20, 21, 22 },
    
    // a5 (index 24)
    new int[] { 0, 8, 16, 32, 40, 48, 56, 25, 26, 27, 28, 29, 30, 31 },
    
    // b5 (index 25)
    new int[] { 1, 9, 17, 33, 41, 49, 57, 24, 26, 27, 28, 29, 30, 31 },
    
    // c5 (index 26)
    new int[] { 2, 10, 18, 34, 42, 50, 58, 24, 25, 27, 28, 29, 30, 31 },
    
    // d5 (index 27)
    new int[] { 3, 11, 19, 35, 43, 51, 59, 24, 25, 26, 28, 29, 30, 31 },
    
    // e5 (index 28)
    new int[] { 4, 12, 20, 36, 44, 52, 60, 24, 25, 26, 27, 29, 30, 31 },
    
    // f5 (index 29)
    new int[] { 5, 13, 21, 37, 45, 53, 61, 24, 25, 26, 27, 28, 30, 31 },
    
    // g5 (index 30)
    new int[] { 6, 14, 22, 38, 46, 54, 62, 24, 25, 26, 27, 28, 29, 31 },
    
    // h5 (index 31)
    new int[] { 7, 15, 23, 39, 47, 55, 63, 24, 25, 26, 27, 28, 29, 30 },
    
    // a4 (index 32)
    new int[] { 0, 8, 16, 24, 40, 48, 56, 33, 34, 35, 36, 37, 38, 39 },
    
    // b4 (index 33)
    new int[] { 1, 9, 17, 25, 41, 49, 57, 32, 34, 35, 36, 37, 38, 39 },
    
    // c4 (index 34)
    new int[] { 2, 10, 18, 26, 42, 50, 58, 32, 33, 35, 36, 37, 38, 39 },
    
    // d4 (index 35)
    new int[] { 3, 11, 19, 27, 43, 51, 59, 32, 33, 34, 36, 37, 38, 39 },
    
    // e4 (index 36)
    new int[] { 4, 12, 20, 28, 44, 52, 60, 32, 33, 34, 35, 37, 38, 39 },
    
    // f4 (index 37)
    new int[] { 5, 13, 21, 29, 45, 53, 61, 32, 33, 34, 35, 36, 38, 39 },
    
    // g4 (index 38)
    new int[] { 6, 14, 22, 30, 46, 54, 62, 32, 33, 34, 35, 36, 37, 39 },
    
    // h4 (index 39)
    new int[] { 7, 15, 23, 31, 47, 55, 63, 32, 33, 34, 35, 36, 37, 38 },
    
    // a3 (index 40)
    new int[] { 0, 8, 16, 24, 32, 48, 56, 41, 42, 43, 44, 45, 46, 47 },
    
    // b3 (index 41)
    new int[] { 1, 9, 17, 25, 33, 49, 57, 40, 42, 43, 44, 45, 46, 47 },
    
    // c3 (index 42)
    new int[] { 2, 10, 18, 26, 34, 50, 58, 40, 41, 43, 44, 45, 46, 47 },
    
    // d3 (index 43)
    new int[] { 3, 11, 19, 27, 35, 51, 59, 40, 41, 42, 44, 45, 46, 47 },
    
    // e3 (index 44)
    new int[] { 4, 12, 20, 28, 36, 52, 60, 40, 41, 42, 43, 45, 46, 47 },
    
    // f3 (index 45)
    new int[] { 5, 13, 21, 29, 37, 53, 61, 40, 41, 42, 43, 44, 46, 47 },
    
    // g3 (index 46)
    new int[] { 6, 14, 22, 30, 38, 54, 62, 40, 41, 42, 43, 44, 45, 47 },
    
    // h3 (index 47)
    new int[] { 7, 15, 23, 31, 39, 55, 63, 40, 41, 42, 43, 44, 45, 46 },
    
    // a2 (index 48)
    new int[] { 0, 8, 16, 24, 32, 40, 56, 49, 50, 51, 52, 53, 54, 55 },
    
    // b2 (index 49)
    new int[] { 1, 9, 17, 25, 33, 41, 57, 48, 50, 51, 52, 53, 54, 55 },
    
    // c2 (index 50)
    new int[] { 2, 10, 18, 26, 34, 42, 58, 48, 49, 51, 52, 53, 54, 55 },
    
    // d2 (index 51)
    new int[] { 3, 11, 19, 27, 35, 43, 59, 48, 49, 50, 52, 53, 54, 55 },
    
    // e2 (index 52)
    new int[] { 4, 12, 20, 28, 36, 44, 60, 48, 49, 50, 51, 53, 54, 55 },
    
    // f2 (index 53)
    new int[] { 5, 13, 21, 29, 37, 45, 61, 48, 49, 50, 51, 52, 54, 55 },
    
    // g2 (index 54)
    new int[] { 6, 14, 22, 30, 38, 46, 62, 48, 49, 50, 51, 52, 53, 55 },
    
    // h2 (index 55)
    new int[] { 7, 15, 23, 31, 39, 47, 63, 48, 49, 50, 51, 52, 53, 54 },
    
    // a1 (index 56)
    new int[] { 0, 8, 16, 24, 32, 40, 48, 57, 58, 59, 60, 61, 62, 63 },
    
    // b1 (index 57)
    new int[] { 1, 9, 17, 25, 33, 41, 49, 56, 58, 59, 60, 61, 62, 63 },
    
    // c1 (index 58)
    new int[] { 2, 10, 18, 26, 34, 42, 50, 56, 57, 59, 60, 61, 62, 63 },
    
    // d1 (index 59)
    new int[] { 3, 11, 19, 27, 35, 43, 51, 56, 57, 58, 60, 61, 62, 63 },
    
    // e1 (index 60)
    new int[] { 4, 12, 20, 28, 36, 44, 52, 56, 57, 58, 59, 61, 62, 63 },
    
    // f1 (index 61)
    new int[] { 5, 13, 21, 29, 37, 45, 53, 56, 57, 58, 59, 60, 62, 63 },
    
    // g1 (index 62)
    new int[] { 6, 14, 22, 30, 38, 46, 54, 56, 57, 58, 59, 60, 61, 63 },
    
    // h1 (index 63)
    new int[] { 7, 15, 23, 31, 39, 47, 55, 56, 57, 58, 59, 60, 61, 62 }
};

}
