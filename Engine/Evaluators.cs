using System;
using Engine.Tables;

namespace Engine
{
    internal static class Evaluators
    {
        private static readonly decimal[] PieceValues =
        {
            0,    // Empty
            10,   // White Pawn
            30,   // White Knight
            35,   // White Bishop
            50,   // White Rook
            90,   // White Queen
            500,  // White King
            10,   // Black Pawn
            30,   // Black Knight
            35,   // Black Bishop
            50,   // Black Rook
            90,   // Black Queen
            500   // Black King
        };

        public static decimal GetByMaterial(int[] chessBoard)
        {
            decimal whiteMaterialValue = 0;
            decimal blackMaterialValue = 0;

            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];
                
                if (piece == 0) continue;

                if (piece == MoveGenerator.whitePawn)
                {
                    whiteMaterialValue += PieceValues[1];
                    whiteMaterialValue += Tables.Pawns.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    whiteMaterialValue += PieceValues[2];
                    whiteMaterialValue += Tables.Knights.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    whiteMaterialValue += PieceValues[3];
                    whiteMaterialValue += Tables.Bishops.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    whiteMaterialValue += PieceValues[4];
               
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    whiteMaterialValue += PieceValues[5];
                    
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    whiteMaterialValue += PieceValues[6];
                    whiteMaterialValue += Tables.Kings.GetWhiteSquareWeight(i);
                    if (Globals.CheckmateWhite)
                    {
                        whiteMaterialValue += 1000;
                    }
                }

                // Black pieces

                else if (piece == MoveGenerator.blackPawn)
                {
                    blackMaterialValue += PieceValues[7];
                    blackMaterialValue += Tables.Pawns.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    blackMaterialValue += PieceValues[8];
                    blackMaterialValue += Tables.Knights.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    blackMaterialValue += PieceValues[9];
                    blackMaterialValue += Tables.Bishops.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    blackMaterialValue += PieceValues[10];
                 
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    blackMaterialValue += PieceValues[11];
                    
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    blackMaterialValue += PieceValues[12];
                    blackMaterialValue += Tables.Kings.GetBlackSquareWeight(i);
                    if (Globals.CheckmateBlack)
                    {
                        blackMaterialValue -= 1000;
                    }
                }
            }
            
            return whiteMaterialValue - blackMaterialValue;
        }
    }
}
