using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Bitboards
{
    public static class Kings
    {
        public static ulong GetWhiteKingMoves(int kingSquareIndex)
        {
            ulong kingPosition = 1UL << kingSquareIndex;

            // Masks for 'a' and 'h' file boundaries
            ulong maskAFile = 0x0101010101010101UL;  // Blocks wraparound from 'a' file
            ulong maskHFile = 0x8080808080808080UL;  // Blocks wraparound from 'h' file

            ulong potentialMoves = 0;

            // Left move (check if king is not on 'a' file)
            if ((kingPosition & maskAFile) == 0)
            {
                potentialMoves |= kingPosition >> 1;  // Move left
            }

            // Right move (check if king is not on 'h' file)
            if ((kingPosition & maskHFile) == 0)
            {
                potentialMoves |= kingPosition << 1;  // Move right
            }

            // Up and down moves (no wraparound needed)
            potentialMoves |= (kingPosition >> 8) | (kingPosition << 8);

            // Diagonal moves with boundary checks
            if ((kingPosition & maskAFile) == 0)  // Not on 'a' file
            {
                potentialMoves |= (kingPosition >> 9) | (kingPosition << 7);  // Top-left, bottom-left
            }

            if ((kingPosition & maskHFile) == 0)  // Not on 'h' file
            {
                potentialMoves |= (kingPosition >> 7) | (kingPosition << 9);  // Top-right, bottom-right
            }

            return potentialMoves;
        }

        public static ulong GetBlackKingMoves(int kingSquareIndex)
        {
            ulong kingPosition = 1UL << kingSquareIndex;

            // Masks for 'a' and 'h' file boundaries
            ulong maskAFile = 0x0101010101010101UL;  // Blocks wraparound from 'a' file
            ulong maskHFile = 0x8080808080808080UL;  // Blocks wraparound from 'h' file

            ulong potentialMoves = 0;

            // Left move (check if king is not on 'a' file)
            if ((kingPosition & maskAFile) == 0)
            {
                potentialMoves |= kingPosition >> 1;  // Move left
            }

            // Right move (check if king is not on 'h' file)
            if ((kingPosition & maskHFile) == 0)
            {
                potentialMoves |= kingPosition << 1;  // Move right
            }

            // Up and down moves (no wraparound needed)
            potentialMoves |= (kingPosition >> 8) | (kingPosition << 8);

            // Diagonal moves with boundary checks
            if ((kingPosition & maskAFile) == 0)  // Not on 'a' file
            {
                potentialMoves |= (kingPosition >> 9) | (kingPosition << 7);  // Top-left, bottom-left
            }

            if ((kingPosition & maskHFile) == 0)  // Not on 'h' file
            {
                potentialMoves |= (kingPosition >> 7) | (kingPosition << 9);  // Top-right, bottom-right
            }

            return potentialMoves;
        }
    }
}
