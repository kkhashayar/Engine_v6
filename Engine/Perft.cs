using System.Diagnostics;
using System.Reflection.Metadata;

namespace Engine;

public static class Perft
{
    public static int count = 0;
    public static ulong Calculate(int[] board, int depth, int turn)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        ulong nodes = CalculateNodes(board, depth, turn, depth);

        stopwatch.Stop();

        Console.WriteLine($"Depth: {depth}, Nodes: {nodes}, Time: {stopwatch.Elapsed.TotalSeconds} seconds");
        return nodes;
    }

    public static ulong CalculateNodes(int[] board, int depth, int turn, int maxDepth)
    {
        if (depth == 0) return 1;

        ulong nodes = 0;
        List<MoveObject> moves = MoveGenerator.GenerateAllMoves(board, turn, true);

        if (moves.Count == 0 && (Globals.CheckBlack || Globals.CheckWhite))
        {
            return 1;
        }

        foreach (MoveObject move in moves)
        {
            RegisterStaticStates();

            int[] shadowBoard = (int[])board.Clone();

            shadowBoard[move.EndSquare] = move.pieceType;
            shadowBoard[move.StartSquare] = 0;
            if (move.pieceType == MoveGenerator.whiteKing && move.LongCastle)
            {
                board[56] = 0; board[59] = MoveGenerator.whiteRook;
            }
            else if (move.pieceType == MoveGenerator.whiteKing && move.ShortCastle)
            {
                board[63] = 0; board[61] = MoveGenerator.whiteRook;
            }
            else if (move.pieceType == MoveGenerator.blackKing && move.LongCastle)
            {
                board[0] = 0; board[3] = MoveGenerator.blackRook;
            }
            else if (move.pieceType == MoveGenerator.blackKing && move.ShortCastle)
            {
                board[7] = 0; board[5] = 0;
            }

            //////////////////////////////////////   DEBUG BOARD 
            //count++;
            //ShowDebugBoard(shadowBoard, 300, move);
            //////////////////////////////////////   DEBUG BOARD 


            ulong childNodes = CalculateNodes(shadowBoard, depth - 1, turn ^ 1, maxDepth);


            nodes += childNodes;
            if (depth == maxDepth)
            {
                Console.WriteLine($"{MoveToString(move)}: {childNodes}");
            }

            
            

        }

        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
        }

        return nodes;
    }

    private static void RegisterStaticStates()
    {
        StateSnapshotBase.WhiteShortCastle = Globals.WhiteShortCastle;
        StateSnapshotBase.WhiteLongCastle = Globals.WhiteLongCastle;
        StateSnapshotBase.WhiteKingRookMoved = Globals.WhiteKingRookMoved;
        StateSnapshotBase.WhiteQueenRookMoved = Globals.WhiteQueenRookMoved;

        StateSnapshotBase.BlackShortCastle = Globals.BlackShortCastle;
        StateSnapshotBase.BlackLongCastle = Globals.BlackLongCastle;
        StateSnapshotBase.BlackKingRookMoved = Globals.BlackKingRookMoved;
        StateSnapshotBase.BlackQueenRookMoved = Globals.BlackQueenRookMoved;

        StateSnapshotBase.CheckmateWhite = Globals.CheckmateWhite;
        StateSnapshotBase.CheckmateBlack = Globals.CheckmateBlack;
        StateSnapshotBase.CheckWhite = Globals.CheckWhite;
        StateSnapshotBase.CheckBlack = Globals.CheckBlack;
        StateSnapshotBase.Stalemate = Globals.Stalemate;
        StateSnapshotBase.LastMoveWasPawn = Globals.LastMoveWasPawn;
        StateSnapshotBase.LastEndSquare = Globals.LastEndSquare;
        StateSnapshotBase.Turn = Globals.Turn;
    }

    public static void RestoreStateFromSnapshot()
    {
        Globals.WhiteShortCastle = StateSnapshotBase.WhiteShortCastle;
        Globals.WhiteLongCastle = StateSnapshotBase.WhiteLongCastle;
        Globals.WhiteKingRookMoved = StateSnapshotBase.WhiteKingRookMoved; 
        Globals.WhiteQueenRookMoved = StateSnapshotBase.WhiteQueenRookMoved;

        Globals.BlackShortCastle = StateSnapshotBase.BlackShortCastle;
        Globals.BlackLongCastle = StateSnapshotBase.BlackLongCastle;
        Globals.BlackKingRookMoved = StateSnapshotBase.BlackKingRookMoved;
        Globals.BlackQueenRookMoved = StateSnapshotBase.BlackQueenRookMoved;

        Globals.CheckmateWhite = StateSnapshotBase.CheckmateWhite;
        Globals.CheckmateBlack = StateSnapshotBase.CheckmateBlack;
        Globals.CheckWhite = StateSnapshotBase.CheckWhite;
        Globals.CheckBlack = StateSnapshotBase.CheckBlack;
        Globals.Stalemate = StateSnapshotBase.Stalemate;
        Globals.LastMoveWasPawn = StateSnapshotBase.LastMoveWasPawn;
        Globals.LastEndSquare = StateSnapshotBase.LastEndSquare;
        Globals.Turn = StateSnapshotBase.Turn;
    }

    private static string MoveToString(MoveObject move)
    {
        return $"{Globals.GetSquareCoordinate(move.StartSquare)}{Globals.GetSquareCoordinate(move.EndSquare)}";
    }

    public static void ShowDebugBoard(int[] board, int speedMs, MoveObject move)
    {
        Console.Clear();
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        string[] fileNames = { "A", "B", "C", "D", "E", "F", "G", "H" };

        for (int rank = 0; rank < 8; rank++)
        {
            Console.Write((rank + 1) + " ");  // Print rank number on the left of the board
            foreach (var file in fileNames.Select((value, index) => new { value, index }))
            {
                int index = rank * 8 + file.index;  // Calculate the index for the current position using file index
                char pieceChar = Globals.GetUnicodeCharacter(board[index]);
                Console.Write(pieceChar + " ");
            }
            Console.WriteLine();
        }

        Console.Write("  ");  // Align file names with the board
        foreach (var fileName in fileNames)
        {
            Console.Write(fileName + " ");  // Print file name
        }
        Console.WriteLine();
        Console.WriteLine($"Ply:{count}");
        Console.WriteLine();
        Console.WriteLine($"{Piece.GetPieceName(move.pieceType)}{Globals.GetSquareCoordinate(move.StartSquare)}-{Globals.GetSquareCoordinate(move.EndSquare)}");
        Thread.Sleep(speedMs);

    }

}
