
namespace Engine;
using Engine.Core;
using Engine.PieceMotions;
public static class MoveGenerator
{
    public static readonly int whiteKing = Piece.King;
    public static readonly int whiteQueen = Piece.Queen;
    public static readonly int whiteRook = Piece.Rook;
    public static readonly int whiteKnight = Piece.Knight;
    public static readonly int whiteBishop = Piece.Bishop;
    public static readonly int whitePawn = Piece.Pawn;

    public static readonly int blackKing = Piece.King + Piece.BlackPieceOffset;
    public static readonly int blackQueen = Piece.Queen + Piece.BlackPieceOffset;
    public static readonly int blackRook = Piece.Rook + Piece.BlackPieceOffset;
    public static readonly int blackKnight = Piece.Knight + Piece.BlackPieceOffset;
    public static readonly int blackBishop = Piece.Bishop + Piece.BlackPieceOffset;
    public static readonly int blackPawn = Piece.Pawn + Piece.BlackPieceOffset;
    public static readonly int None = Piece.None;

    //////////////////////////////////////   ENGINE CORE LOOP 

    public static List<MoveObject> GenerateAllMoves(int[] chessBoard, int turn, bool filter = false)
    {
        List<MoveObject> moves = new List<MoveObject>();

        List<MoveObject> pseudoMoves = GeneratePseudoLegalMoves(chessBoard, turn);

        if (filter)
        {
            foreach (var move in pseudoMoves)
            {
                if (IsMoveLegal(move, chessBoard, turn)) moves.Add(move);
            }
        }
        else
        {
            moves = pseudoMoves;
        }
        return moves;
    }

    private static List<MoveObject> GeneratePseudoLegalMoves(int[] chessBoard, int turn)
    {
        List<MoveObject> pseudoMoves = new();

        for (int square = 0; square < 64; square++)
        {
            if (!Globals.IsValidSquare(square)) continue;

            int piece = chessBoard[square];

            // Generate moves for white pieces
            if (turn == 0)
            {
                if (piece == whiteKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whiteQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == whitePawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                }
            }
            
            
            else if (turn == 1)
            {
                if (piece == blackKing)
                {
                    pseudoMoves.AddRange(Kings.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackKnight)
                {
                    pseudoMoves.AddRange(Knights.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackRook)
                {
                    pseudoMoves.AddRange(Rooks.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackBishop)
                {
                    pseudoMoves.AddRange(Bishops.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackQueen)
                {
                    pseudoMoves.AddRange(Queens.GenerateMovesForSquare(square, turn, chessBoard));
                }
                else if (piece == blackPawn)
                {
                    pseudoMoves.AddRange(Pawns.GenerateMovesForSquare(square, turn, chessBoard));
                }
            }
        }

        // Check for checkmate or stalemate
        if (pseudoMoves.Count == 0)
        {
            if (Globals.CheckWhite || Globals.CheckBlack)
            {
                if (turn == 0)
                    Globals.CheckmateWhite = true;
                else
                    Globals.CheckmateBlack = true;
            }
            else
            {
                Globals.Stalemate = true;
            }
        }

        return pseudoMoves;
    }

    private static bool IsMoveLegal(MoveObject move, int[] board, int turn)
    {
        MoveHandler.RegisterStaticStates();
        int[] shadowBoard = (int[])board.Clone();
        
        MoveHandler.MakeMove(shadowBoard, move);
        // Kings positions
        int kingSquare = (turn == 0) ? Globals.GetWhiteKingSquare(shadowBoard) : Globals.GetBlackKingSquare(shadowBoard);
        int enemyKingSquare = (turn == 0) ? Globals.GetBlackKingSquare(shadowBoard) : Globals.GetWhiteKingSquare(shadowBoard);

        int opponentTurn = turn ^ 1;

        /* prevent overlapping kings */ 
        List<MoveObject> enemyKingMoves = Kings.GenerateMovesForSquare(enemyKingSquare, opponentTurn, shadowBoard);
        HashSet<int> enemyKingAttackSquares = enemyKingMoves.Select(m => m.EndSquare).ToHashSet();

        // Check if the move is a king move
        if (move.pieceType == MoveGenerator.whiteKing || move.pieceType == MoveGenerator.blackKing)
        {
            // If it's a king move, remove the squares attacked by the enemy king
            if (enemyKingAttackSquares.Contains(move.EndSquare))
            {
                return false; // Moving into the same square attacked by enemy king is illegal
            }
        }
        /* prevent overlapping kings */


        // Generate all opponent pseudo-legal moves
        List<MoveObject> opponentMoves = GeneratePseudoLegalMoves(shadowBoard, opponentTurn);

        // Check if any of the opponent's moves attack the king's square
        bool kingInCheck = opponentMoves.Any(m => m.EndSquare == kingSquare);

        // Also consider vertical, horizontal, and diagonal attacks to confirm legality
        var verticalAndHorizontalAttacks = GenerateVerticalAndHorizontalAttacks(shadowBoard, opponentTurn);
        var diagonalAttacks = GenerateDiagonalAttacks(shadowBoard, opponentTurn);

        // Final king check
        if (verticalAndHorizontalAttacks.Contains(kingSquare) || diagonalAttacks.Contains(kingSquare)) return false;

        // If the king is in check after the move, it's illegal

        MoveHandler.RestoreStateFromSnapshot(); 
        return !kingInCheck;
    }



    private static HashSet<int> GenerateVerticalAndHorizontalAttacks(int[] board, int opponentTurn)
    {
        HashSet<int> attackSquares = new HashSet<int>();

        for (int square = 0; square < 64; square++)
        {
            int piece = board[square];
            if ((opponentTurn == 0 && (piece == MoveGenerator.whiteRook || piece == MoveGenerator.whiteQueen)) ||
                (opponentTurn == 1 && (piece == MoveGenerator.blackRook || piece == MoveGenerator.blackQueen)))
            {
                attackSquares.UnionWith(GenerateRookAttackLines(square, board));
            }
        }

        return attackSquares;
    }

    private static HashSet<int> GenerateDiagonalAttacks(int[] board, int opponentTurn)
    {
        HashSet<int> attackSquares = new HashSet<int>();

        for (int square = 0; square < 64; square++)
        {
            int piece = board[square];
            if ((opponentTurn == 0 && (piece == MoveGenerator.whiteBishop || piece == MoveGenerator.whiteQueen)) ||
                (opponentTurn == 1 && (piece == MoveGenerator.blackBishop || piece == MoveGenerator.blackQueen)))
            {
                attackSquares.UnionWith(GenerateBishopAttackLines(square, board));
            }
        }

        return attackSquares;
    }

    private static HashSet<int> GenerateRookAttackLines(int startSquare, int[] board)
    {
        HashSet<int> attackSquares = new HashSet<int>();

        int[] directions = { -8, 8, -1, 1 }; // Up, down, left, right
        foreach (int direction in directions)
        {
            int currentSquare = startSquare + direction;
            while (Globals.IsValidSquare(currentSquare) && IsSameRankOrFile(startSquare, currentSquare, direction))
            {
                attackSquares.Add(currentSquare);
                if (board[currentSquare] != 0) break; // Stop if we hit a piece
                currentSquare += direction;
            }
        }

        return attackSquares;
    }

    private static HashSet<int> GenerateBishopAttackLines(int startSquare, int[] board)
    {
        HashSet<int> attackSquares = new HashSet<int>();

        int[] directions = { -9, -7, 7, 9 }; // Diagonals
        foreach (int direction in directions)
        {
            int currentSquare = startSquare + direction;
            while (Globals.IsValidSquare(currentSquare) && IsSameDiagonal(startSquare, currentSquare))
            {
                attackSquares.Add(currentSquare);
                if (board[currentSquare] != 0) break; // Stop if we hit a piece
                currentSquare += direction;
            }
        }

        return attackSquares;
    }

    private static bool IsSameRankOrFile(int startSquare, int currentSquare, int direction)
    {
        if (direction == -8 || direction == 8)
        {
            return true; // Moving up or down, always valid in same file
        }
        if (direction == -1 || direction == 1)
        {
            return (startSquare / 8) == (currentSquare / 8); // Moving left or right, check if same rank
        }
        return false;
    }


    public static int GetTotalNumberOfAttackSquares(int[] board, int turn)
    {
        // Generate attack lines based on the opponent's pieces
        int opponentTurn = turn ^ 1;

        // Get attack squares for rooks and queens (vertical and horizontal)
        var verticalAndHorizontalAttacks = GenerateVerticalAndHorizontalAttacks(board, opponentTurn);

        // Get attack squares for bishops and queens (diagonal)
        var diagonalAttacks = GenerateDiagonalAttacks(board, opponentTurn);

        // Combine both attack square sets and return the count
        HashSet<int> allAttacks = new HashSet<int>(verticalAndHorizontalAttacks);
        allAttacks.UnionWith(diagonalAttacks);

        // Return the total number of attack squares
        return allAttacks.Count;
    }


    private static bool IsSameDiagonal(int startSquare, int currentSquare)
    {
        int startRank = startSquare / 8;
        int startFile = startSquare % 8;
        int currentRank = currentSquare / 8;
        int currentFile = currentSquare % 8;

        return Math.Abs(startRank - currentRank) == Math.Abs(startFile - currentFile);
    }


}
