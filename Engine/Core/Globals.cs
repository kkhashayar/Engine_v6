using System.Diagnostics;
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

    public static List<MoveObject> PrincipalVariation = new List<MoveObject>();
    public static MoveObject? LastMoveMade { get; set; } = null;
    // Tracking enpassant 
    public static int LastEndSquare { get; set; } = -1;

    public static List<MoveObject> moveHistory = new List<MoveObject>();
    public static int Turn { get; set; }
    public static int InitialTurn { get; set; }
    public static bool InitialDepthAdjusted { get; set; } = false;
    public static string CurrentFEN { get; set; }

    public static Stopwatch TotalTime = new Stopwatch();

    public static GamePhase GamePhase { get; set; }
    public static GamePhase GameStateForWhiteKing { get; set; }
    public static GamePhase GameStateForBlackKing { get; set; }
    public static GamePhase GameStateForWhiteRook { get; set; } 
    public static GamePhase GameStateForBlackRook { get; set; }
    public static int ThinkingTime { get; set; } = 0;
    public static int MaxDepth = 20; 

    public static List<int> OnBoardPieces = new List<int>();

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

    public static readonly int[] LightSquares =
    {
        0, 2, 4, 6, 9, 11, 13, 15, 16, 18, 20, 22, 25, 27, 29, 31,
        32, 34, 36, 38, 41, 43, 45, 47, 48, 50, 52, 54, 57, 59, 61, 63
    };
    public static readonly int[] DarkSquares =
    {
        1, 3, 5, 7, 8, 10, 12, 14, 17, 19, 21, 23, 24, 26, 28, 30,
        33, 35, 37, 39, 40, 42, 44, 46, 49, 51, 53, 55, 56, 58, 60, 62
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

    public static int GetBlackKingSquare(int[] board)
    {
        for (int i = 0; i < 64; i++)
        {
            if (board[i] == 109)
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
            if (board[i] == 99)
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
        int rankDifference = endSquare / 8 - startSquare / 8;
        int fileDifference = endSquare % 8 - startSquare % 8;

        if (rankDifference > 0 && fileDifference > 0)
            return 9; // Moving up-right
        else if (rankDifference > 0 && fileDifference < 0)
            return 7; // Moving up-left
        else if (rankDifference < 0 && fileDifference > 0)
            return -7; // Moving down-right
        else
            return -9; // Moving down-left
    }

    // Diagonal Breaks Mask
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

    // Only in use with Old cli version
    public static string MoveToString(MoveObject move)
    {
        string promotion = string.Empty;
        string castle = string.Empty;
        if (move is not null)
        {
            promotion = move.IsPromotion ? $"({Piece.GetPieceName(move.PromotionPiece)})" : "";
            castle = move.ShortCastle ? "O-O" : move.LongCastle ? "O-O-O" : "";
        }
        

        if (!string.IsNullOrEmpty(castle))
        {
            return castle;
        }
        if (move == null) return "";
        // if (move.StartSquare == move.EndSquare) return "";
        if (move.IsCheck)
        {
            return $"{Piece.GetPieceName(move.pieceType)}{GetSquareCoordinate(move.StartSquare)}-{GetSquareCoordinate(move.EndSquare)}{promotion} \"+\" ";
        }
        return $"{Piece.GetPieceName(move.pieceType)}{GetSquareCoordinate(move.StartSquare)}-{GetSquareCoordinate(move.EndSquare)}{promotion} ";
    }

    // In use with UCI protocol
    public static string ConvertMoveToString(MoveObject move)
    {
        int startSquare = move.StartSquare;
        int endSquare = move.EndSquare;

        char startFile = (char)('a' + startSquare % 8);
        char startRank = (char)('1' + startSquare / 8);
        char endFile = (char)('a' + endSquare % 8);
        char endRank = (char)('1' + endSquare / 8);

        return $"{startFile}{startRank}{endFile}{endRank}";
    }

    public static MoveObject ConvertStringToMoveObject(string move)
    {
        // Extract file and rank from the string
        char startFile = move[0];
        char startRank = move[1];
        char endFile = move[2];
        char endRank = move[3];

        // Convert characters back to board indices
        int startSquare = (startRank - '1') * 8 + (startFile - 'a');
        int endSquare = (endRank - '1') * 8 + (endFile - 'a');

        // Create a MoveObject with the calculated indices
        return new MoveObject
        {
            StartSquare = startSquare,
            EndSquare = endSquare,
            pieceType = 0,
            CapturedPiece = 0,
            PromotionPiece = 0,
            ShortCastle = false,
            LongCastle = false,
            IsCapture = false,
            IsEnPassant = false,
            IsPromotion = false,
            IsCheck = false,
            Priority = 0,
            Score = 0
        };
    }


    public static string BoardToFen(int[] board, int turn)
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
    public static GamePhase GetGamePhase()
    {
        if (NumberOfWhitePieces + NumberOfBlackPieces <= 10)
        {
            ThinkingTime = 5;
            return GamePhase.EndGame;
        }
        else if (NumberOfWhitePieces + NumberOfBlackPieces >= 18 && NumberOfWhitePieces + NumberOfBlackPieces <= 30)
        {
            ThinkingTime = 60;
            return GamePhase.MiddleGame;
        }

        else
        {
            ThinkingTime = 10;
            return GamePhase.Opening;
        }
    }

    public static EndGames GetEndGameType(int[]board) 
    { 
        if(IsSingleRookOnBoard(board)) return EndGames.RookKing;
        return EndGames.None;
    }

    public static void GetOnBoardPieces(int[]board)
    {
        OnBoardPieces.Clear(); 
        OnBoardPieces = board.Where(x => x > 0).ToList();
       
    }
    public static bool IsSingleRookOnBoard(int[] board)
    {
        int rookCount = 0;

        foreach (int piece in board)
        {
            if (piece == MoveGenerator.whiteRook || piece == MoveGenerator.blackRook)
            {
                rookCount++;

                if (rookCount > 1)
                {
                    return false;
                }
            }
            else if (piece != 0 && piece != MoveGenerator.whiteKing && piece != MoveGenerator.blackKing)
            {
                return false;
            }
        }

        return rookCount == 1;
    }

    // Usefull for king based end games.
    public static int ManhattanDistance(MoveObject move, int otherKingPosition)
    {
        int endFile = move.EndSquare % 8;
        int endRank = move.EndSquare / 8;

        int kingFile = otherKingPosition % 8;
        int kingRank = otherKingPosition / 8;

        int distance = Math.Abs(endFile - kingFile) + Math.Abs(endRank - kingRank);

        return distance;
    }
    private static char GetFenSymbol(int piece)
    {
        return PieceToFenMap.TryGetValue(piece, out char fenSymbol) ? fenSymbol : ' ';
    }

    private static readonly Dictionary<int, char> PieceToFenMap = new Dictionary<int, char>
        {
            { Piece.Pawn, 'P' },
            { Piece.Knight, 'N' },
            { Piece.Bishop, 'B' },
            { Piece.Rook, 'R' },
            { Piece.Queen, 'Q' },
            { Piece.King, 'K' },
            { Piece.Pawn + Piece.BlackPieceOffset, 'p' },
            { Piece.Knight + Piece.BlackPieceOffset, 'n' },
            { Piece.Bishop + Piece.BlackPieceOffset, 'b' },
            { Piece.Rook + Piece.BlackPieceOffset, 'r' },
            { Piece.Queen + Piece.BlackPieceOffset, 'q' },
            { Piece.King + Piece.BlackPieceOffset, 'k' }
        };

}

