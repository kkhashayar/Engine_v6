using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine;

public class Helpers
{
    public static char GetUnicodeCharacter(int pieceCode)
    {
        switch (pieceCode)
        {
            case 1: return '\u2659'; // White Pawn
            case 3: return '\u2658'; // White Knight
            case 4: return '\u2657'; // White Bishop
            case 5: return '\u2656'; // White Rook
            case 9: return '\u2655'; // White Queen
            case 100: return '\u2654'; // White King
            case -1: return '\u265F'; // Black Pawn
            case -3: return '\u265E'; // Black Knight
            case -4: return '\u265D'; // Black Bishop
            case -5: return '\u265C'; // Black Rook
            case -9: return '\u265B'; // Black Queen
            case -100: return '\u265A'; // Black King
            default: return '.'; // Empty square
        }
    }

    static List<char> Unicodes = new List<char>
    {
            '\u2659','\u2658','\u2657','\u2656',
            '\u2655','\u2654','\u265F','\u265E',
            '\u265D','\u265C','\u265B','\u265A'
    };

    public enum Coordinates
    {
        a8, b8, c8, d8, e8, f8, g8, h8,
        a7, b7, c7, d7, e7, f7, g7, h7,
        a6, b6, c6, d6, e6, f6, g6, h6,
        a5, b5, c5, d5, e5, f5, g5, h5,
        a4, b4, c4, d4, e4, f4, g4, h4,
        a3, b3, c3, d3, e3, f3, g3, h3,
        a2, b2, c2, d2, e2, f2, g2, h2,
        a1, b1, c1, d1, e1, f1, g1, h1
    };


    // TODO: Implement castling, captures, check, checkmate, 
    public static string MoveToString(MoveObject move)
    {
        // Convert the start and end squares to coordinates using the enum
        Coordinates startCoordinate = (Coordinates)move.StartSquare;
        Coordinates endCoordinate = (Coordinates)move.EndSquare;

        // Return the move as a string in the format "e2e4", for example
        return $"{startCoordinate}{endCoordinate}";
    }



    public static void PrintBitboard(ulong bitboard)
    {
        Console.WriteLine($"Bitboard in Hex: 0x{bitboard:X16}\n"); // Print hex representation of the bitboard

        // Adjusted rank loop to start from 0 and go to 7, matching Coordinates enum
        for (int rank = 0; rank < 8; rank++)
        {
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                ulong bit = 1UL << square;
                Console.Write((bitboard & bit) != 0 ? "1 " : ". ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nBit positions:");

        // Print the bit positions where the bit is set to 1
        for (int i = 0; i < 64; i++)
        {
            if ((bitboard & (1UL << i)) != 0)
            {
                Console.WriteLine($"Bit {i} is set.");
            }
        }

        Console.WriteLine();
    }

    public static List<MoveObject> ConvertBitboardToMoveObjects(ulong bitboard, int startSquare, int pieceType)
    {
        List<MoveObject> moveList = new();
        List<int> endSquares = BitboardToSquares(bitboard);

        foreach (int endSquare in endSquares)
        {
            moveList.Add(new MoveObject
            {
                StartSquare = startSquare,
                EndSquare = endSquare,
                pieceType = pieceType
            });
        }

        return moveList;
    }

    // Helper method to convert a bitboard into a list of squares (indices)
    public static List<int> BitboardToSquares(ulong bitboard)
    {
        List<int> squares = new List<int>();

        while (bitboard != 0)
        {
            int lsbIndex = BitOperations.TrailingZeroCount(bitboard); // Get least significant bit index
            squares.Add(lsbIndex);
            bitboard &= bitboard - 1;  // Reset the least significant bit
        }

        return squares;
    }

}
