﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    internal static  class Evaluators
    {
        public static decimal GetByMaterial(int[] chessBoard)
        {
            decimal score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += 1;
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += 3;
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += 3;
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += 5;
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += 9;
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += 100;
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= 1;
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= 3;
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= 3;
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= 5;
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= 9;
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= 100;
                }
            }
            return score;
        }

    }
}