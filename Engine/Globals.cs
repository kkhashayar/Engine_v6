namespace Engine;

public sealed class Globals
{
    public bool WhiteShortCastle { get; set; }
    public bool WhiteLongCastle { get; set; }
    public bool BlackShortCastle { get; set; }
    public bool BlackLongCastle { get; set; }

    public bool CheckmateWhite { get; set; } = false;
    public bool CheckmateBlack { get; set; } = false;
    public static bool CheckWhite { get; set; } = false;
    public static bool CheckBlack { get; set; } = false;

    public bool Stalemate { get; set; } = false;


    public  int Turn { get; set; }

    public int[] ChessBoard = new int[64]
    {
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0
    };


    public static readonly int[] BoardOfRanks = new int[64]
    {
        8, 8, 8, 8, 8, 8, 8, 8,
        7, 7, 7, 7, 7, 7, 7, 7,
        6, 6, 6, 6, 6, 6, 6, 6,
        5, 5, 5, 5, 5, 5, 5, 5,
        4, 4, 4, 4, 4, 4, 4, 4,
        3, 3, 3, 3, 3, 3, 3, 3,
        2, 2, 2, 2, 2, 2, 2, 2,
        1, 1, 1, 1, 1, 1, 1, 1
    };

    public static readonly int[] BoardOfFiles = new int[64]
    {
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8,
        1, 2, 3, 4, 5, 6, 7, 8
    };

    public static readonly int[] LightSquares = new int[32]
    {
        0, 2, 4, 6, 9, 11, 13, 15, 16, 18, 20, 22, 25, 27, 29, 31,
        32, 34, 36, 38, 41, 43, 45, 47, 48, 50, 52, 54, 57, 59, 61, 63
    };
    public static readonly int[] DarkSquares = new int[32]
    {
        1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23, 24, 26, 28, 30,
        33, 35, 37, 39, 40, 42, 44, 46, 49, 51, 53, 55, 56, 58, 60, 62
    };

    public static readonly int[] BoardIndices = new int[64]
   {
        0,  1,   2,   3,   4,   5,   6,   7, 
        8,  9,  10,  11,  12,  13,  14,  15,
       16, 17,  18,  19,  20,  21,  22,  23, 
       24, 25,  26,  27,  28,  29,  30,  31,
       32, 33,  34,  35,  36,  37,  38,  39, 
       40, 41,  42,  43,  44,  45,  46,  47,
       48, 49,  50,  51,  52,  53,  54,  55, 
       56, 57,  58,  59,  60,  61,  62,  63
   };

  

    public static readonly string[] Coordinates = new string[64]
    {
        "a8", "b8", "c8", "d8", "e8", "f8", "g8", "h8",
        "a7", "b7", "c7", "d7", "e7", "f7", "g7", "h7",
        "a6", "b6", "c6", "d6", "e6", "f6", "g6", "h6",
        "a5", "b5", "c5", "d5", "e5", "f5", "g5", "h5",
        "a4", "b4", "c4", "d4", "e4", "f4", "g4", "h4",
        "a3", "b3", "c3", "d3", "e3", "f3", "g3", "h3",
        "a2", "b2", "c2", "d2", "e2", "f2", "g2", "h2",
        "a1", "b1", "c1", "d1", "e1", "f1", "g1", "h1"
    };

    public static bool IsValidSquare(int square)
    {
        return square >= 0 && square < 64;
    }
    public static int GetSquareIndex(string coordinate)
    {
        return Array.IndexOf(Coordinates, coordinate);
    }

    public static string GetSquareCoordinate(int squareIndex)
    {
        return Coordinates[squareIndex];
    }

    public static Globals Clone(Globals instanceToClone)
    {
        var copy = new Globals();
        
        copy.ChessBoard         = (int[])instanceToClone.ChessBoard.Clone();
        copy.WhiteShortCastle   = instanceToClone.WhiteShortCastle;
        copy.WhiteLongCastle    = instanceToClone.WhiteLongCastle;
        copy.BlackShortCastle   = instanceToClone.BlackShortCastle;
        copy.BlackLongCastle    = instanceToClone.BlackLongCastle;
        copy.CheckmateWhite     = instanceToClone.CheckmateWhite;
        copy.CheckmateBlack     = instanceToClone.CheckmateBlack; 
        copy.Stalemate          = instanceToClone.Stalemate;
        copy.Turn               = instanceToClone.Turn;
        
        return copy;
    }



    public static Globals FenReader(string fen)
    {
        var board = new Globals();
        string[] parts = fen.Split(' ');

        // Parse board state
        string[] ranks = parts[0].Split('/');

        if (parts[1] == "w")
        {
            board.Turn = 0;
        }
        else if (parts[1] == "b")
        {
            board.Turn = 1;
        }
        // Parse castling rights
        board.WhiteShortCastle = parts[2].Contains("K");
        board.WhiteLongCastle = parts[2].Contains("Q");
        board.BlackShortCastle = parts[2].Contains("k");
        board.BlackLongCastle = parts[2].Contains("q");

        int index = 0;
        foreach (var rank in ranks)
        {
            foreach (var square in rank)
            {
                if (char.IsDigit(square))
                {
                    // Empty squares indicated by digits in FEN
                    int emptySquares = int.Parse(square.ToString());
                    for (int i = 0; i < emptySquares; i++)
                    {
                        board.ChessBoard[index] = 0; // Empty square
                        index++;
                    }
                }
                else
                {
                    // If it's a piece, convert and place on the board
                    board.ChessBoard[index] = FenCharToPieceCode(square);
                    index++;
                }
            }
        }


        //

        return board;
    }


    private static int FenCharToPieceCode(char c)
    {
        int pieceCode;
        switch (char.ToLower(c))
        {
            case 'p': pieceCode = Piece.Pawn; break;
            case 'n': pieceCode = Piece.Knight; break;
            case 'b': pieceCode = Piece.Bishop; break;
            case 'r': pieceCode = Piece.Rook; break;
            case 'q': pieceCode = Piece.Queen; break;
            case 'k': pieceCode = Piece.King; break;
            default: return Piece.None; 
        }

        if (char.IsUpper(c))
        {
            return pieceCode; // White pieces
        }
        else
        {
            return pieceCode + Piece.BlackPieceOffset; // Black pieces
        }
    }
    public static bool IsCrossSliderPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetCrossDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;
        bool pieceColor = Piece.IsBlack(board[startSquare]);

        while (currentSquare != endSquare)
        {
            if (board[currentSquare] != 0) return false;
            currentSquare += direction;
        }

        return board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != pieceColor;
    }


    public static int GetCrossDirection(int startSquare, int endSquare)
    {
        if (endSquare > startSquare) // Moving up or right
        {
            if (endSquare % 8 == startSquare % 8) // Vertical move
                return 8;
            else // Horizontal move
                return 1;
        }
        else // Moving down or left
        {
            if (endSquare % 8 == startSquare % 8) // Vertical move
                return -8;
            else // Horizontal move
                return -1;
        }
    }

    public static bool IsDiagonalPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetDiagonalDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;
        bool pieceColor = Piece.IsBlack(board[startSquare]);

        while (currentSquare != endSquare)
        {
            if (board[currentSquare] != 0) return false;
            currentSquare += direction;
        }

        // The path is clear if the end square is empty or occupied by an opponent's piece
        return board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != pieceColor;
    }

    public static int GetDiagonalDirection(int startSquare, int endSquare)
    {
        int rankDifference = (endSquare / 8) - (startSquare / 8);
        int fileDifference = (endSquare % 8) - (startSquare % 8);

        if (rankDifference > 0 && fileDifference > 0)
            return 9; // Moving up-right
        else if (rankDifference > 0 && fileDifference < 0)
            return 7; // Moving up-left
        else if (rankDifference < 0 && fileDifference > 0)
            return -7; // Moving down-right
        else
            return -9; // Moving down-left
    }


    public static bool IsDiagBreaksMask(int square, int direction, int originalRank, int originalFile)
    {
        int newRank = square / 8;
        int newFile = square % 8;

        // Check if the square is outside the bounds of the board
        if (newRank < 0 || newRank > 7 || newFile < 0 || newFile > 7)
            return true;

        // Check for diagonal consistency
        int rankDifference = Math.Abs(newRank - originalRank);
        int fileDifference = Math.Abs(newFile - originalFile);

        return rankDifference != fileDifference;
    }

    public static bool IsVerHorBreaksMask(int square, int direction, int originalRank, int originalFile)
    {
        int newRank = square / 8;
        int newFile = square % 8;

        // Vertical movement check
        if (direction == 8 || direction == -8)
        {
            // Break the mask if it's off-board vertically
            if (newRank < 0 || newRank > 7) return true;
            // Ensure vertical movement stays in the same file
            if (newFile != originalFile) return true;
        }
        // Horizontal movement check
        else if (direction == 1 || direction == -1)
        {
            // Break the mask if it's off-board horizontally
            if (newFile < 0 || newFile > 7) return true;
            // Ensure horizontal movement stays in the same rank
            if (newRank != originalRank) return true;
        }

        return false;
    }

    public static char GetUnicodeCharacter(int pieceCode)
    {
        switch (pieceCode)
        {
            case 1: return '\u2659'; // White Pawn
            case 3: return '\u2658'; // White Knight
            case 4: return '\u2657'; // White Bishop
            case 5: return '\u2656'; // White Rook
            case 9: return '\u2655'; // White Queen
            case 99: return '\u2654'; // White King
            case 11: return '\u265F'; // Black Pawn
            case 13: return '\u265E'; // Black Knight
            case 14: return '\u265D'; // Black Bishop
            case 15: return '\u265C'; // Black Rook
            case 19: return '\u265B'; // Black Queen
            case 109: return '\u265A'; // Black King
            default: return '.'; // Empty square
        }
    }

    static List<char> Unicodes = new List<char>
        {
            '\u2659','\u2658','\u2657','\u2656',
            '\u2655','\u2654','\u265F','\u265E',
            '\u265D','\u265C','\u265B','\u265A'
        };



}
