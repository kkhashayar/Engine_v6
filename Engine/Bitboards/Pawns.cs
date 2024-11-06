using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.BItBoards;

public static class Pawns
{
    private const ulong Rank2 = 0xff000000000000;  // 2nd rank for white pawns double moves
    private const ulong Rank7 = 0x00FF000000000000UL;  // 7th rank for black pawns double moves

    private const ulong notAFile = 0xfefefefefefefefeUL;  // Not A-file for wrapping prevention
    private const ulong notHFile = 0x7f7f7f7f7f7f7f7fUL;  // Not H-file for wrapping prevention

    // Method to generate quiet moves
    public static ulong GetWhitePawnMoves(ulong whitePawns, ulong occupiedSquares)
    {
        // Shift pawns one step forward
        ulong singleMove = (whitePawns << 8) & ~occupiedSquares;

        // Shift pawn two steps forward
        ulong doubleMove = ((whitePawns & Rank2) << 8) & ~occupiedSquares;
        doubleMove = (doubleMove << 8) & ~occupiedSquares;

        return singleMove | doubleMove;
    }

    // Method to generate attack moves (captures) for white pawns, including en passant
    public static ulong GetWhitePawnAttacks(ulong whitePawns, ulong blackPieces, ulong enPassantTarget = 0UL)
    {
        // Capture diagonally left and prevent wrap-around from the A-file
        ulong captureLeft = (whitePawns << 7) & notAFile & blackPieces;

        // Capture diagonally right and prevent wrap-around from the H-file
        ulong captureRight = (whitePawns << 9) & notHFile & blackPieces;

        // En passant captures (optional)
        ulong enPassantCaptures = (whitePawns << 7 & notAFile | whitePawns << 9 & notHFile) & enPassantTarget;

        return captureLeft | captureRight | enPassantCaptures;  // Combine both left, right captures and en passant
    }

    // Method to generate quiet moves 
    public static ulong GetBlackPawnMoves(ulong blackPawns, ulong occupiedSquares)
    {
        // Shift pawns one step forward
        ulong singleMove = (blackPawns >> 8) & ~occupiedSquares;

        // Shift pawns one step forward
        ulong doubleMove = ((blackPawns & Rank7) >> 8) & ~occupiedSquares;
        doubleMove = (doubleMove >> 8) & ~occupiedSquares;

        return singleMove | doubleMove;
    }

    // Method to generate attack moves (captures) for black pawns, including en passant
    public static ulong GetBlackPawnAttacks(ulong blackPawns, ulong whitePieces, ulong enPassantTarget = 0UL)
    {
        // Capture diagonally left and prevent wrap-around from the H-file
        ulong captureLeft = (blackPawns >> 9) & notHFile & whitePieces;

        // Capture diagonally right and prevent wrap-around from the A-file
        ulong captureRight = (blackPawns >> 7) & notAFile & whitePieces;

        // En passant captures (optional)
        ulong enPassantCaptures = (blackPawns >> 9 & notHFile | blackPawns >> 7 & notAFile) & enPassantTarget;

        return captureLeft | captureRight | enPassantCaptures;  // Combine both left, right captures and en passant
    }
}
