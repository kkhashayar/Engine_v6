namespace Engine;

public sealed class Globals
{
    
    public bool WhiteShortCastle { get; set; }
    public bool WhiteLongCastle { get; set; }
    public bool BlackShortCastle { get; set; }
    public bool BlackLongCastle { get; set; }
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
        copy.ChessBoard = (int[])instanceToClone.ChessBoard.Clone();
        copy.WhiteShortCastle = instanceToClone.WhiteShortCastle;
        copy.WhiteLongCastle = instanceToClone.WhiteLongCastle;
        copy.BlackShortCastle = instanceToClone.BlackShortCastle;
        copy.BlackLongCastle = instanceToClone.BlackLongCastle;
        copy.Turn = instanceToClone.Turn;
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


}
