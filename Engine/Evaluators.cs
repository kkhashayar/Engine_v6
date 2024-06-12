using System.Diagnostics.CodeAnalysis;

namespace Engine;
internal static class Evaluators
{
    // Piece weights
    private static readonly int PawnWeight = 10;
    private static readonly int KnightWeight = 30;
    private static readonly int BishopWeight = 35;
    private static readonly int RookWeight = 50;
    private static readonly int QueenWeight = 90;
    private static readonly int KingWeight = 10000;

    private static int NumberOfWhitePieces { get; set; }
    private static int NumberOfBlackPieces { get; set; }
    private static int WhiteStaticScore { get; set; }
    private static int BlackStaticScore { get; set; }
    private static int WhitePositionValue { get; set; }
    private static int BlackPositionValue { get; set; }
    private static int WhiteKingActivityScore { get; set; }
    private static int BlackKingActivityScore { get; set; }
    private static readonly int KingActivityWeight = 5;
    private static readonly int MaxKingProximityWeight = 10;

    public static int GetByMaterial(int[] board, int turn)
    {
        ResetScores();
        Globals.GamePhase = GetGamePhase();

        for (int i = 0; i < board.Length; ++i)
        {
            int square = board[i];
            if (square == 0) continue;

            if (square == MoveGenerator.whitePawn)
            {
                WhiteStaticScore += PawnWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackPawn)
            {
                BlackStaticScore += PawnWeight;
                NumberOfBlackPieces++;
            }
            if (square == MoveGenerator.whiteKnight)
            {
                WhiteStaticScore += KnightWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackKnight)
            {
                BlackStaticScore += KnightWeight;
                NumberOfBlackPieces++;
            }
            if (square == MoveGenerator.whiteBishop)
            {
                WhiteStaticScore += BishopWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackBishop)
            {
                BlackStaticScore += BishopWeight;
                NumberOfBlackPieces++;
            }
            if (square == MoveGenerator.whiteRook)
            {
                WhiteStaticScore += RookWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackRook)
            {
                BlackStaticScore += RookWeight;
                NumberOfBlackPieces++;
            }
            if (square == MoveGenerator.whiteQueen)
            {
                WhiteStaticScore += QueenWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackQueen)
            {
                BlackStaticScore += QueenWeight;
                NumberOfBlackPieces++;
            }
            if (square == MoveGenerator.whiteKing)
            {
                WhiteStaticScore += KingWeight;
                NumberOfWhitePieces++;
            }
            if (square == MoveGenerator.blackKing)
            {
                BlackStaticScore += KingWeight;
                NumberOfBlackPieces++;
            }
        }
        GetPieceTableValues(board); 
        if (Globals.GamePhase == GamePhase.EndGame)
        {
            EvaluateKingActivity(board);
        }

        if (turn == 0)
        {
            return (WhiteStaticScore - BlackStaticScore)
                + (NumberOfWhitePieces - NumberOfBlackPieces)
                + (WhitePositionValue - BlackPositionValue)
                + (WhiteKingActivityScore - BlackKingActivityScore);
        }
        else
        {
            return (BlackStaticScore - WhiteStaticScore)
                + (NumberOfBlackPieces - NumberOfWhitePieces)
                + (BlackPositionValue - WhitePositionValue)
                + (BlackKingActivityScore - WhiteKingActivityScore);
        }
    }

    private static void GetPieceTableValues(int[] board)
    {
        for (int square = 0; square < board.Length; square++)
        {
            if (square == 0) continue;
            if (board[square] == MoveGenerator.whitePawn) WhitePositionValue += Tables.Pawns.GetWhiteSquareWeight(square);
            if (board[square] == MoveGenerator.blackPawn) BlackPositionValue += Tables.Pawns.GetBlackSquareWeight(square);
            if (board[square] == MoveGenerator.whiteKnight) WhitePositionValue += Tables.Knights.GetWhiteSquareWeight(square);
            if (board[square] == MoveGenerator.blackKnight) BlackPositionValue += Tables.Knights.GetBlackSquareWeight(square);
            if (board[square] == MoveGenerator.whiteBishop) WhitePositionValue += Tables.Bishops.GetWhiteSquareWeight(square);
            if (board[square] == MoveGenerator.blackBishop) BlackPositionValue += Tables.Bishops.GetBlackSquareWeight(square);
            if (board[square] == MoveGenerator.whiteRook) WhitePositionValue += Tables.Rooks.GetWhiteSquareWeight(square);
            if (board[square] == MoveGenerator.blackRook) BlackPositionValue += Tables.Rooks.GetBlackSquareWeight(square);
            //if (square == MoveGenerator.whiteQueen) WhitePositionValue += Tables.Queens.GetWhiteSquareWeight(board[square]);
            //if (square == MoveGenerator.blackQueen) BlackPositionValue += Tables.Queens.GetBlackSquareWeight(board[square]);
            if (board[square] == MoveGenerator.whiteKing) WhitePositionValue += Tables.Kings.GetWhiteSquareWeight(square);
            if (board[square] == MoveGenerator.blackKing) BlackPositionValue += Tables.Kings.GetBlackSquareWeight(square);
        }
    }

    private static void ResetScores()
    {
        WhiteStaticScore = 0;
        BlackStaticScore = 0;
        NumberOfWhitePieces = 0;
        NumberOfBlackPieces = 0;
        WhitePositionValue = 0;
        BlackPositionValue = 0;
        WhiteKingActivityScore = 0;
        BlackKingActivityScore = 0;
    }

    private static void EvaluateKingActivity(int[] board)
    {
        int whiteKingSquare = Globals.GetWhiteKingSquare(board);
        int blackKingSquare = Globals.GetBlackKingSquare(board);
        WhiteKingActivityScore = CalculateKingActivityScore(board, 0, whiteKingSquare, blackKingSquare);
        BlackKingActivityScore = CalculateKingActivityScore(board, 1, blackKingSquare, whiteKingSquare);
    }

    private static int CalculateKingActivityScore(int[] board, int turn, int kingSquare, int opponentKingSquare)
    {
        var kingMoves = MoveGenerator.GetKingAttacks(board, turn);
        int activityScore = 0;

        foreach (var move in kingMoves)
        {
            int distanceToOpponentKing = GetManhattanDistance(move.EndSquare, opponentKingSquare);
            int proximityWeight = MaxKingProximityWeight - distanceToOpponentKing;
            activityScore += KingActivityWeight + proximityWeight;
        }

        return activityScore;
    }

    private static int GetManhattanDistance(int square1, int square2)
    {
        int x1 = square1 % 8;
        int y1 = square1 / 8;
        int x2 = square2 % 8;
        int y2 = square2 / 8;
        return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
    }

    private static GamePhase GetGamePhase()
    {
        if (NumberOfWhitePieces + NumberOfBlackPieces <= 10)
        {
            return GamePhase.EndGame;
        }
        else if (NumberOfWhitePieces + NumberOfBlackPieces <= 20)
        {
            return GamePhase.MiddleGame;
        }
        else
        {
            return GamePhase.Opening;
        }
    }
}


public enum GamePhase
{
    Opening,
    MiddleGame,
    EndGame
}
