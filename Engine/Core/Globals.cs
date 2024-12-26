using System.Text;
using Engine.Enums;
namespace Engine.Core;

public sealed class Globals
{
    public static bool WhiteShortCastle { get; set; }
    public static bool WhiteLongCastle { get; set; }
    public static bool WhiteKingRookMoved { get; set; }
    public static bool WhiteQueenRookMoved { get; set; }

    public static int NumberOfWhitePieces { get; set; }

    public static bool BlackShortCastle { get; set; }
    public static bool BlackLongCastle { get; set; }
    public static bool BlackKingRookMoved { get; set; }
    public static bool BlackQueenRookMoved { get; set; }

    public static int NumberOfBlackPieces { get; set; }
    public static bool CheckmateWhite { get; set; } = false;
    public static bool CheckmateBlack { get; set; } = false;
    public static bool CheckWhite { get; set; } = false;
    public static bool CheckBlack { get; set; } = false;

    public static bool Stalemate { get; set; } = false;

    public static bool LastMoveWasPawn { get; set; } = false;


    // Tracking enpassant 
    public static int LastEndSquare { get; set; } = -1;

    public static List<MoveObject> moveHistory = new List<MoveObject>();
    public static int Turn { get; set; }
    public static int InitialTurn { get; set; }

    //////////// SEARCH SETTINGS ////////////
    public static int OpeningTime { get; set; } = 0;
    public static int MiddleGameTime { get; set; } = 0;
    public static int EndGameTime { get; set; } = 0;
    public static int MaxDepth = 0;
    public static bool QuQuiescenceSwitch { get; set; }
    public static int QuiescenceDepth { get; set; }
    public static int DepthBalancer { get; set; }
    //////////// END OF SEARCH SETTINGS /////


    public int[] ChessBoard =
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


    public static readonly int[] BoardOfRanks =
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

    public static readonly int[] BoardOfFiles =
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

    public static readonly int[] BoardCenterGravity =
    {
        1, 2, 3, 4, 4, 3, 2, 1,
        2, 3, 4, 5, 5, 4, 3, 2,
        3, 4, 5, 6, 6, 5, 4, 3,
        4, 5, 6, 7, 7, 6, 5, 4,
        4, 5, 6, 7, 7, 6, 5, 4,
        3, 4, 5, 6, 6, 5, 4, 3,
        2, 3, 4, 5, 5, 4, 3, 2,
        1, 2, 3, 4, 4, 3, 2, 1
    };

    public static readonly int[] BoardCornersGravity =
    {
        8, 7, 6, 5, 5, 6, 7, 8,
        7, 6, 5, 4, 4, 5, 6, 7,
        6, 5, 4, 3, 3, 4, 5, 6,
        5, 4, 3, 2, 2, 3, 4, 5,
        5, 4, 3, 2, 2, 3, 4, 5,
        6, 5, 4, 3, 3, 4, 5, 6,
        7, 6, 5, 4, 4, 5, 6, 7,
        8, 7, 6, 5, 5, 6, 7, 8
    };
    public static readonly int[] PawnJumpCaptureSquares =
    {
        56,48,40,32,24,16,8,0,
        63,55,47,39,31,23,15,7
    };

    public static readonly string[] Coordinates =
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


    public static readonly int[] BoardIndices =
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
    public static ulong GetBitboard(int[] board)
    {
        ulong bitboard = 0;
        for (int square = 0; square < 64; square++)
        {
            if (board[square] != 0)
            {
                bitboard |= (1UL << square);
            }
        }
        return bitboard;
    }
    public static int GetBlackKingSquare(int[] board)
    {
        for (int i = 0; i < 64; i++)
        {
            if (board[i] == -1000)
            {
                return i;
            }
        }
        return -1;
    }
    public static int GetWhiteKingSquare(int[] board)
    {
        for (int i = 0; i < 64; i++)
        {
            if (board[i] == 1000)
            {
                return i;
            }
        }
        return -1;
    }

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

    public static string FenWriter(int[] board, int turn)
    {
        StringBuilder fenBuilder = new StringBuilder();

        int emptyCount = 0;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                emptyCount++;
            }
            else
            {
                if (emptyCount > 0)
                {
                    fenBuilder.Append(emptyCount);
                    emptyCount = 0;
                }
                fenBuilder.Append(GetFenSymbol(board[i]));
            }

            if ((i + 1) % 8 == 0)
            {
                if (emptyCount > 0)
                {
                    fenBuilder.Append(emptyCount);
                    emptyCount = 0;
                }
                if (i < board.Length - 1)
                {
                    fenBuilder.Append('/');
                }
            }
        }

        fenBuilder.Append(turn == 0 ? " w " : " b ");
        fenBuilder.Append("- - 0 1"); // Default castling rights

        return fenBuilder.ToString();
    }
    public static Globals FenReader(string fen)
    {

        if (string.IsNullOrEmpty(fen))
        {
            fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        }

        var globals = new Globals();
        string[] parts = fen.Split(' ');

        // Parse board state
        string[] ranks = parts[0].Split('/');

        if (parts[1] == "w")
        {
            Turn = 0;
            InitialTurn = 0;
        }
        else if (parts[1] == "b")
        {
            Turn = 1;
            InitialTurn = 1;
        }
        // Parse castling rights nad rook first move states 
        WhiteShortCastle = parts[2].Contains("K");
        if (WhiteShortCastle) WhiteKingRookMoved = false;

        WhiteLongCastle = parts[2].Contains("Q");
        if (WhiteLongCastle) WhiteQueenRookMoved = false;

        BlackShortCastle = parts[2].Contains("k");
        if (BlackShortCastle) BlackKingRookMoved = false;

        BlackLongCastle = parts[2].Contains("q");
        if (BlackLongCastle) BlackQueenRookMoved = false;

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
                        globals.ChessBoard[index] = 0; // Empty square
                        index++;
                    }
                }
                else
                {
                    // If it's a piece, convert and place on the board
                    globals.ChessBoard[index] = FenCharToPieceCode(square);
                    // Count the number of pieces for each side
                    if (Piece.IsWhite(globals.ChessBoard[index]))
                    {
                        NumberOfWhitePieces++;
                    }
                    else if (Piece.IsBlack(globals.ChessBoard[index]))
                    {
                        NumberOfBlackPieces++;
                    }
                    index++;
                }
            }
        }
        return globals;
    }

    private static int FenCharToPieceCode(char c)
    {
        int pieceCode;
        switch (c)
        {
            case 'p': pieceCode = Piece.p; break;
            case 'n': pieceCode = Piece.n; break;
            case 'b': pieceCode = Piece.b; break;
            case 'r': pieceCode = Piece.r; break;
            case 'q': pieceCode = Piece.q; break;
            case 'k': pieceCode = Piece.k; break;
            case 'P': pieceCode = Piece.P; break;
            case 'N': pieceCode = Piece.N; break;
            case 'B': pieceCode = Piece.B; break;
            case 'R': pieceCode = Piece.R; break;
            case 'Q': pieceCode = Piece.Q; break;
            case 'K': pieceCode = Piece.K; break;
            default: return Piece.None;
        }
        return pieceCode;

    }
    //public static bool IsCrossSliderPathClear(int startSquare, int endSquare, int[] board)
    //{
    //    int direction = GetCrossDirection(startSquare, endSquare);
    //    int currentSquare = startSquare + direction;
    //    bool pieceColor = Piece.IsBlack(board[startSquare]);

    //    while (currentSquare != endSquare)
    //    {
    //        if (board[currentSquare] != 0) return false;
    //        currentSquare += direction;
    //    }

    //    return board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != pieceColor;
    //}


    public static bool IsCrossSliderPathClear(int startSquare, int endSquare, int[] board)
    {
        int direction = GetCrossDirection(startSquare, endSquare);
        int currentSquare = startSquare + direction;
        bool pieceColor = Piece.IsBlack(board[startSquare]);

        while (Globals.IsValidSquare(currentSquare) && currentSquare != endSquare)
        {
            if (board[currentSquare] != 0) return false;
            currentSquare += direction;
        }

        // Ensure endSquare is valid and check its value
        return Globals.IsValidSquare(endSquare) && (board[endSquare] == 0 || Piece.IsBlack(board[endSquare]) != pieceColor);
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

    // Vertical Horizontal Breaks Mask
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
            case 1000: return '\u2654'; // White King
            case -1: return '\u265F'; // Black Pawn
            case -3: return '\u265E'; // Black Knight
            case -4: return '\u265D'; // Black Bishop
            case -5: return '\u265C'; // Black Rook
            case -9: return '\u265B'; // Black Queen
            case -1000: return '\u265A'; // Black King
            default: return '.'; // Empty square
        }
    }

    // Only in use with old cli version
    public static string MoveToString(MoveObject move)
    {
        if (move == null) return "";

        // Handle castling
        if (move.ShortCastle) return "O-O";
        if (move.LongCastle) return "O-O-O";

        // Determine capture symbol
        string capture = move.IsCapture ? "x" : "-";

        // Handle promotion
        string promotion = move.IsPromotion ? Piece.GetPieceName(move.PromotionPiece).ToLower() : "";

        // Handle check
        string check = move.IsCheck ? "+" : "";

        // Build the move string
        string moveString = $"{Piece.GetPieceName(move.pieceType)}{GetSquareCoordinate(move.StartSquare)}{capture}{GetSquareCoordinate(move.EndSquare)}{promotion}{check}";

        return moveString;
    }

    public static EndGames GetEndGameType(int[] board)
    {
        if (IsSingleRookOnBoard(board)) return EndGames.RookKing;
        return EndGames.None;
    }

    public static bool IsSingleRookOnBoard(int[] board)
    {
        int rookCount = 0;

        foreach (int piece in board)
        {
            if (piece == 5 || piece == -5)
            {
                rookCount++;

                if (rookCount > 1)
                {
                    return false;
                }
            }
            else if (piece != 0 && piece != 1000 && piece != -1000)
            {
                return false;
            }
        }

        return rookCount == 1;
    }


    private static char GetFenSymbol(int piece)
    {
        return PieceToFenMap.TryGetValue(piece, out char fenSymbol) ? fenSymbol : ' ';
    }

    private static readonly Dictionary<int, char> PieceToFenMap = new Dictionary<int, char>
        {
            { Piece.P, 'P' },
            { Piece.N, 'N' },
            { Piece.B, 'B' },
            { Piece.R, 'R' },
            { Piece.Q, 'Q' },
            { Piece.K, 'K' },

            { Piece.p, 'p' },
            { Piece.n, 'n' },
            { Piece.b, 'b' },
            { Piece.r, 'r' },
            { Piece.q, 'q' },
            { Piece.k, 'k' }
        };

}

