namespace Engine;

public static class Evaluators
{
    public static bool IsEndGame { get; set; }
    public static bool IsMiddleGame { get; set; }
    public static bool WhitePairBishops { get; set; }
    public static bool BlackPairBishops { get; set; }

    public static int whiteMaterialScore { get; set; } = 0;
    public static int blackMaterialScore { get; set; } = 0;

    public static List<int> listWhitePieces { get; set; } = new List<int>();  
    public static List<int> listBlackPieces { get; set; } = new List<int>();

    public static double whitePositionalWeight { get; set; } = 0;   
    public static double blackPositionalWeight { get; set; } = 0;

    public static int opponentNumberOfPossibleMoves;
    public static int numberOfMovesForCurrentSide;
    public static int whitePieceNumbers;
    public static int blackPieceNumbers;
    public static int numberOfTotalPieces;



   
    public static void GetGameStageAndPositionalFacts(int[] board, int turn, int numberOfMovesForCurrentSide)
    {
        var opponent = turn ^ 1;

        numberOfMovesForCurrentSide = numberOfMovesForCurrentSide; 
        opponentNumberOfPossibleMoves = MoveGenerator.GenerateAllMoves(board, opponent).Count;

        // I will divide the game in two general stages, Middle and End game. Delibrately ignoring the opening stage for now. 
        var whitePieceNumbers = 0;
        var blackPieceNumbers = 0;
        var numberOfTotalPieces = 0;    

        listWhitePieces = new List<int>();  
        listBlackPieces = new List<int>();


        foreach (var piece in board)
        {
            if(piece == 0) continue;
            if(piece == MoveGenerator.whiteKing)
            {
                whitePieceNumbers += 1;   
                listWhitePieces.Add(piece);
                whitePositionalWeight += Tables.Kings.GetWhiteSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.whiteQueen)
            {
                whitePieceNumbers += 1;   
                listWhitePieces.Add(piece);
            }
            else if(piece == MoveGenerator.whiteRook)
            {
                whitePieceNumbers += 1;   
                listWhitePieces.Add(piece);
            }
            else if(piece == MoveGenerator.whiteBishop)
            {
                whitePieceNumbers += 1;   
                listWhitePieces.Add(piece);
                whitePositionalWeight += Tables.Bishops.GetWhiteSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.whiteKnight)
            {
                whitePieceNumbers += 1;
                listWhitePieces.Add(piece);
                whitePositionalWeight += Tables.Knights.GetWhiteSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.whitePawn)
            {
                whitePieceNumbers += 1;   
                listWhitePieces.Add(piece);
                whitePositionalWeight += Tables.Pawns.GetWhiteSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.blackKing)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
                blackPositionalWeight += Tables.Kings.GetBlackSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.blackQueen)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
            }
            else if(piece == MoveGenerator.blackRook)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
            }
            else if(piece == MoveGenerator.blackBishop)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
                blackPositionalWeight += Tables.Bishops.GetBlackSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.blackKnight)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
                blackPositionalWeight += Tables.Knights.GetBlackSquareWeight(Array.IndexOf(board, piece));
            }
            else if(piece == MoveGenerator.blackPawn)
            {
                blackPieceNumbers += 1;   
                listBlackPieces.Add(piece);
                blackPositionalWeight += Tables.Pawns.GetBlackSquareWeight(Array.IndexOf(board, piece));
            }
            numberOfTotalPieces = whitePieceNumbers + blackPieceNumbers;
        }

        if(numberOfTotalPieces == 32 || numberOfTotalPieces >= 20) IsMiddleGame = true;
        else if(numberOfTotalPieces <= 20 && (!listWhitePieces.Contains(MoveGenerator.whiteQueen) && !listBlackPieces.Contains(MoveGenerator.blackQueen)))
        {
            IsMiddleGame = false;
            IsEndGame = true;
        }
    }


    public static int GetByMobility()
    {
       return numberOfMovesForCurrentSide - opponentNumberOfPossibleMoves;  
    }
    public static double GetByMaterial()
    {
        double whiteMaterialScore = 0;
        double blackMaterialResponse = 0;

        foreach (var piece in listWhitePieces)
        {
            whiteMaterialScore += piece;
        }

        foreach (var piece in listBlackPieces)
        {
            blackMaterialResponse += (piece - Piece.BlackPieceOffset);
        }

        int whitePieceNumbers = listWhitePieces.Count;
        int blackPieceNumbers = listBlackPieces.Count;

        double score = whiteMaterialScore - blackMaterialResponse + (whitePieceNumbers - blackPieceNumbers);

        return score;
    }

}
