//using System;
//using Engine.Tables;

//namespace Engine
//{
//    internal static class Evaluators
//    {
//        private static readonly decimal[] PieceValues = 
//        {
//            0,    // Empty
//            10,   // White Pawn
//            30,   // White Knight
//            35,   // White Bishop
//            50,   // White Rook
//            90,   // White Queen
//            500,  // White King
//            -10,  // Black Pawn
//            -30,  // Black Knight
//            -35,  // Black Bishop
//            -50,  // Black Rook
//            -90,  // Black Queen
//            -500  // Black King
//        };

//        public static decimal GetByMaterial(int[] chessBoard, int numberOfWhiteMoves, int numberOfBlackMoves)
//        {
//            decimal score = 0;
//            for (int i = 0; i < 64; i++)
//            {
//                int piece = chessBoard[i];

//                if (piece == MoveGenerator.whitePawn)
//                {
//                    score += PieceValues[1];
//                    score += Tables.Pawns.GetWhiteSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.whiteKnight)
//                {
//                    score += PieceValues[2];
//                    score += Tables.Knights.GetWhiteSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.whiteBishop)
//                {
//                    score += PieceValues[3];
//                    score += Tables.Bishops.GetWhiteSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.whiteRook)
//                {
//                    score += PieceValues[4];
//                    // Add positional evaluation for rooks
//                }
//                else if (piece == MoveGenerator.whiteQueen)
//                {
//                    score += PieceValues[5];
//                    // Add positional evaluation for queens
//                }
//                else if (piece == MoveGenerator.whiteKing)
//                {
//                    score += PieceValues[6];
//                    // Add positional evaluation for kings
//                    if (Globals.CheckmateWhite)
//                    {
//                        score += 1000;
//                    }
//                }
//                else if (piece == MoveGenerator.blackPawn)
//                {
//                    score += PieceValues[7];
//                    score += Tables.Pawns.GetBlackSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.blackKnight)
//                {
//                    score += PieceValues[8];
//                    score += Tables.Knights.GetBlackSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.blackBishop)
//                {
//                    score += PieceValues[9];
//                    score += Tables.Bishops.GetBlackSquareWeight(i);
//                }
//                else if (piece == MoveGenerator.blackRook)
//                {
//                    score += PieceValues[10];
//                    // Add positional evaluation for rooks
//                }
//                else if (piece == MoveGenerator.blackQueen)
//                {
//                    score += PieceValues[11];
//                    // Add positional evaluation for queens
//                }
//                else if (piece == MoveGenerator.blackKing)
//                {
//                    score += PieceValues[12];
//                    // Add positional evaluation for kings
//                    if (Globals.CheckmateBlack)
//                    {
//                        score -= 1000;
//                    }
//                }
//            }

//            // Penalize hanging pieces
//            score -= GetHangingPiecesPenalty(chessBoard);

//            decimal mobilityScore = (numberOfWhiteMoves - numberOfBlackMoves) * 1.03m;
//            score += mobilityScore;

//            return score;
//        }

//        private static decimal GetHangingPiecesPenalty(int[] chessBoard)
//        {
//            decimal penalty = 0;
//            for (int i = 0; i < 64; i++)
//            {
//                int piece = chessBoard[i];
//                if (piece != 0)
//                {
//                    bool isWhite = !Piece.IsBlack(piece);
//                    int opponentColor;
//                    if (isWhite)
//                    {
//                        opponentColor = 1;
//                    }
//                    else
//                    {
//                        opponentColor = 0;
//                    }
//                    var enemyMoves = MoveGenerator.GenerateAllMoves(chessBoard, opponentColor, false);
//                    foreach (var move in enemyMoves)
//                    {
//                        if (move.EndSquare == i)
//                        {
//                            penalty += PieceValues[piece % 10];
//                            break;
//                        }
//                    }
//                }
//            }
//            return penalty;
//        }
//    }
//}

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
            10,   // Black Pawn (will use positive values but subtracted for black)
            30,   // Black Knight
            35,   // Black Bishop
            50,   // Black Rook
            90,   // Black Queen
            500   // Black King
        };

        public static decimal GetByMaterial(int[] chessBoard, int numberOfWhiteMoves, int numberOfBlackMoves)
        {
            decimal score = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];

                if (piece == MoveGenerator.whitePawn)
                {
                    score += PieceValues[1];
                    score += Tables.Pawns.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteKnight)
                {
                    score += PieceValues[2];
                    score += Tables.Knights.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteBishop)
                {
                    score += PieceValues[3];
                    score += Tables.Bishops.GetWhiteSquareWeight(i);
                }
                else if (piece == MoveGenerator.whiteRook)
                {
                    score += PieceValues[4];
                }
                else if (piece == MoveGenerator.whiteQueen)
                {
                    score += PieceValues[5];
                }
                else if (piece == MoveGenerator.whiteKing)
                {
                    score += PieceValues[6];
                    if (Globals.CheckmateWhite)
                    {
                        score += 1000;
                    }
                }
                else if (piece == MoveGenerator.blackPawn)
                {
                    score -= PieceValues[7];
                    score += Tables.Pawns.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackKnight)
                {
                    score -= PieceValues[8];
                    score += Tables.Knights.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackBishop)
                {
                    score -= PieceValues[9];
                    score += Tables.Bishops.GetBlackSquareWeight(i);
                }
                else if (piece == MoveGenerator.blackRook)
                {
                    score -= PieceValues[10];
                }
                else if (piece == MoveGenerator.blackQueen)
                {
                    score -= PieceValues[11];
                }
                else if (piece == MoveGenerator.blackKing)
                {
                    score -= PieceValues[12];
                    if (Globals.CheckmateBlack)
                    {
                        score -= 1000;
                    }
                }
            }

            // Penalize hanging pieces
            score -= GetHangingPiecesPenalty(chessBoard);

            decimal mobilityScore = (numberOfWhiteMoves - numberOfBlackMoves) * 1.03m;
            score += mobilityScore;

            return score;
        }

        private static decimal GetHangingPiecesPenalty(int[] chessBoard)
        {
            decimal penalty = 0;
            for (int i = 0; i < 64; i++)
            {
                int piece = chessBoard[i];
                if (piece != 0)
                {
                    string pieceColor = Piece.GetColor(piece);
                    int opponentColor;
                    if (pieceColor == "Black")
                    {
                        opponentColor = 1; // White's turn
                    }
                    else
                    {
                        opponentColor = 0; // Black's turn
                    }

                    var enemyMoves = MoveGenerator.GenerateAllMoves(chessBoard, opponentColor, false);
                    foreach (var move in enemyMoves)
                    {
                        if (move.EndSquare == i)
                        {
                            int pieceIndex;
                            if (pieceColor == "Black")
                            {
                                pieceIndex = piece - Piece.BlackPieceOffset + 6; // Adjust for black pieces index
                            }
                            else
                            {
                                pieceIndex = piece;
                            }
                            penalty += PieceValues[pieceIndex];
                            break;
                        }
                    }
                }
            }
            return penalty;
        }


    }
}
