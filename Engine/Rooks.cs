
using System.Net.Quic;

namespace Engine;

internal static class Rooks
{
    public static List<MoveObject> GenerateMovesForSquare(int square, int turn, int[] board)
    {
        List<int> targetSquares = GetMasksForSquare(square);

        List<MoveObject> moves = new();

        if(turn == 0)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if(Globals.IsCrossSliderPathClear(square, targetSquare, board))
                {
                    if (targetsquareColor == "White") continue; 
           
                    else
                    {
                        moves.Add(new MoveObject
                        {
                            pieceType = MoveGenerator.whiteRook,
                            StartSquare = square,
                            EndSquare = targetSquare
                        });
                    }
                }
            }
            return moves;
        }
        else if(turn == 1)
        {
            foreach (int targetSquare in targetSquares)
            {
                var targetsquareColor = Piece.GetColor(board[targetSquare]);
                if (Globals.IsCrossSliderPathClear(square, targetSquare, board))
                {
                    if (targetsquareColor == "Black") continue;

                    else
                    {
                        moves.Add(new MoveObject
                        {
                            pieceType = MoveGenerator.blackRook,
                            StartSquare = square,
                            EndSquare = targetSquare
                        });
                    }
                }
            }
        }

        return moves;   
    }

    public static List<int> GetMasksForSquare(int square)
    {
        List<int> squares = new();
        
        int[] directions = { 1, -1, 8, -8 }; // right, left, up, down
        int originalRank = square / 8;
        int originalFile = square % 8;

        foreach (int direction in directions)
        {
            int currentSquare = square + direction;
            while (Globals.IsValidSquare(currentSquare) && !Globals.IsVerHorBreaksMask(currentSquare, direction, originalRank, originalFile))
            {
                squares.Add(currentSquare);
                currentSquare += direction;
            }
        }

        return squares; 
    }
}
