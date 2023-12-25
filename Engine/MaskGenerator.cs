﻿namespace Engine;

public static class MaskGenerator
{
    public static  List<List<int>>? KingMasks;
    public static List<List<int>>?  KnightMasks;

    public static void GenerateAllMasks()
    {
        KingMasks = new List<List<int>>();
        GenerateAllKingMasks();
        
        KnightMasks = new List<List<int>>();
        GenerateAllKnightMasks(); 
    }

    static void GenerateAllKingMasks()
    {
        for (int square = 0; square < 64; square++)
        {
            KingMasks.Add(GenerateKingMask(square).ToList());
        }
    }

    static void GenerateAllKnightMasks()
    {
        for (int square = 0; square < 64; square++)
        {
            KnightMasks.Add(GenerateKnightMasks(square).ToList());
        }
    }

    public static IEnumerable<int> GenerateKnightMasks(int square)
    {
        // 8 possible moves for a knight at given square
        (int, int)[] KnightMoves = {(2, 1), (1, 2), (-1, 2), (-2, 1),
                                    (-2, -1), (-1, -2), (1, -2), (2, -1)};

        List<int> moves = new List<int>();
        int originalRow = square / 8;  // Calculating the row of the square
        int originalCol = square % 8;  // Calculating the file of the square

        foreach (var (rowChange, colChange) in KnightMoves)
        {
            int newRow = originalRow + rowChange;  // New row after making the move
            int newCol = originalCol + colChange;  // New column after making the move

            // Check if the new position is still on the board
            if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
            {
                
                moves.Add(newRow * 8 + newCol);
            }
        }

        return moves;
    }
    static IEnumerable<int> GenerateKingMask(int square)
    {
        int[] KingDirections = new int[8] { 9, 8, 7, 1, -9, -7, -8, -1 };

        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in KingDirections)
        {
            int desSquare = square + direction;

            // Calculate rank and file after the move
            int newRank = desSquare / 8;
            int newFile = desSquare % 8;

            // Check if move is within one rank/file step from the original position
            if (Math.Abs(newRank - originalRank) <= 1 && Math.Abs(newFile - originalFile) <= 1)
            {
                if (IsValidSquare(desSquare))
                {
                    yield return desSquare;
                }
            }
        }
    }

    // Only checks if square is in the board
    static bool IsValidSquare(int square)
    {
        return square >= 0 && square < 64;
    }

}

