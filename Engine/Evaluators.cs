//using System.Diagnostics.CodeAnalysis;

//namespace Engine;

//public static class Evaluators
//{

//    public static double GetByMaterial(int[] board, int turn, int currentSideNumberOfPossibleMoves)
//    {
//        double score = 0;
//        var opponentSideNumberOfPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn ^ 1).Count;

//        if(turn == 0)
//        {
//            for (int i = 0; i < 64; i++)
//            {
//                int piece = board[i];
//                if (piece == 0) continue;
//                score += GetPieceValue(piece);
//            }
//            score *= (currentSideNumberOfPossibleMoves - opponentSideNumberOfPossibleMoves);

//            return score;   
//        }


//        for (int i = 0; i < 64; i++)
//        {
//            int piece = board[i];
//            if (piece == 0) continue;
//            score += GetPieceValue(piece);
//        }
//        score *= (currentSideNumberOfPossibleMoves - opponentSideNumberOfPossibleMoves);

//        return score;


//    }

//    private static double GetPieceValue(int piece)
//    {
//        if (piece == MoveGenerator.whitePawn) return 1;
//        else if (piece == MoveGenerator.blackKnight) return 3; 
//        else if (piece == MoveGenerator.whiteBishop) return 3.1;
//        else if (piece == MoveGenerator.blackRook) return 5;
//        else if (piece == MoveGenerator.whiteQueen) return 9;
//        else if (piece == MoveGenerator.blackKing) return 100;

//        else if (piece == MoveGenerator.blackPawn) return -1;
//        else if (piece == MoveGenerator.whiteKnight) return -3;
//        else if (piece == MoveGenerator.blackBishop) return -3.1;
//        else if (piece == MoveGenerator.whiteRook) return -5;
//        else if (piece == MoveGenerator.blackQueen) return -9;
//        else if (piece == MoveGenerator.whiteKing) return -100;

//        else return 0;
//    }
//}

using Engine;

public static class Evaluators
{
    public static double GetByMaterial(int[] board, int turn, int currentSideNumberOfPossibleMoves)
    {
        double score = 0;
        var opponentSideNumberOfPossibleMoves = MoveGenerator.GenerateAllMoves(board, turn ^ 1).Count;
        for (int i = 0; i < 64; i++)
        {
            int piece = board[i];
            if (piece == 0) continue;
            score += GetPieceValue(piece);
        }

      
        double mobilityFactor = 0.1; 
        score += mobilityFactor * (currentSideNumberOfPossibleMoves - opponentSideNumberOfPossibleMoves);

        return score;
    }
    private static double GetPieceValue(int piece)
    {
        if (piece == MoveGenerator.whitePawn) return 1;
        else if (piece == MoveGenerator.whiteKnight) return 3;
        else if (piece == MoveGenerator.whiteBishop) return 3.1;
        else if (piece == MoveGenerator.whiteRook) return 5;
        else if (piece == MoveGenerator.whiteQueen) return 9;
        else if (piece == MoveGenerator.whiteKing) return 100;
        else if (piece == MoveGenerator.blackPawn) return -1;
        else if (piece == MoveGenerator.blackKnight) return -3;
        else if (piece == MoveGenerator.blackBishop) return -3.1;
        else if (piece == MoveGenerator.blackRook) return -5;
        else if (piece == MoveGenerator.blackQueen) return -9;
        else if (piece == MoveGenerator.blackKing) return -100;
        else return 0;  // Default case if no known piece is matched
    }

}
