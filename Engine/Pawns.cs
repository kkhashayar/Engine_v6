namespace Engine;

internal static class Pawns
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board, int enPassantSquare = -1)
    {
        List<MoveObject> moves = new List<MoveObject>();
        int direction = turn == 0 ? -8 : 8; // White moves up, Black moves down
        int startRank = turn == 0 ? 6 : 1; // Starting ranks for White and Black
        int promotionRank = turn == 0 ? 0 : 7; // Promotion ranks for White and Black

        // Forward movement
        int forwardSquare = square + direction;
        if (Globals.IsValidSquare(forwardSquare) && board[forwardSquare] == 0)
        {
            AddPawnMove(square, forwardSquare, promotionRank, moves, turn);

            // Double move from starting position
            if ((square / 8) == startRank)
            {
                int doubleForwardSquare = square + 2 * direction;
                if (board[doubleForwardSquare] == 0)
                {
                    AddPawnMove(square, doubleForwardSquare, promotionRank, moves, turn);
                }
            }
        }

        // Capturing
        int[] captureDirections = { direction - 1, direction + 1 }; // Diagonal capture
        foreach (int captureDirection in captureDirections)
        {
            int captureSquare = square + captureDirection;
            if (Globals.IsValidSquare(captureSquare))
            {
                // Regular capture
                if (board[captureSquare] != 0 && Piece.GetColor(board[captureSquare]) != (turn == 0 ? "White" : "Black"))
                {
                    AddPawnMove(square, captureSquare, promotionRank, moves, turn);
                }

                // En Passant
                if (captureSquare == enPassantSquare)
                {
                    moves.Add(new MoveObject
                    {
                        pieceType = turn == 0 ? MoveGenerator.whitePawn : MoveGenerator.blackPawn,
                        StartSquare = square,
                        EndSquare = captureSquare,
                        IsEnPassant = true
                    });
                }
            }
        }

        return moves;
    }

    private static void AddPawnMove(int startSquare, int endSquare, int promotionRank, List<MoveObject> moves, int turn)
    {
        if ((endSquare / 8) == promotionRank)
        {
            // TODO: Have to fix promotion 
            string piecePrefix = turn == 0 ? "white" : "black";
            moves.AddRange(new[]
            {
                new MoveObject { StartSquare = startSquare, EndSquare = endSquare, Promotion = piecePrefix + "Queen" },
                new MoveObject { StartSquare = startSquare, EndSquare = endSquare, Promotion = piecePrefix + "Rook" },
                new MoveObject { StartSquare = startSquare, EndSquare = endSquare, Promotion = piecePrefix + "Bishop" },
                new MoveObject { StartSquare = startSquare, EndSquare = endSquare, Promotion = piecePrefix + "Knight" }
});
        }
        else
        {
            // Regular pawn move
            moves.Add(new MoveObject
            {
                pieceType = turn == 0 ? MoveGenerator.whitePawn : MoveGenerator.blackPawn,
                StartSquare = startSquare,
                EndSquare = endSquare
            });
        }
    }
}
