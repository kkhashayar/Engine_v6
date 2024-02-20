using System.Diagnostics;
using System.Reflection.Metadata;

namespace Engine;

public static class Perft
{
    public static Stack<StateSnapshotBase> stateSnapshots = new Stack<StateSnapshotBase>();
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
            StateSnapshotBase snapshot = new StateSnapshotBase
            {
                WhiteShortCastle = Globals.WhiteShortCastle,
                WhiteLongCastle = Globals.WhiteLongCastle,
                BlackShortCastle = Globals.BlackShortCastle,
                BlackLongCastle = Globals.BlackLongCastle,
                CheckmateWhite = Globals.CheckmateWhite,
                CheckmateBlack = Globals.CheckmateBlack,
                CheckWhite = Globals.CheckWhite,
                CheckBlack = Globals.CheckBlack,
                Stalemate = Globals.Stalemate,
                LastMoveWasPawn = Globals.LastMoveWasPawn,
                LastendSquare = Globals.LastendSquare,
                Turn = Globals.Turn
            };

            int[] shadowBoard = (int[])board.Clone();

            stateSnapshots.Push(snapshot);

            shadowBoard[move.EndSquare] = move.pieceType;
            shadowBoard[move.StartSquare] = 0; 
            if(move.pieceType == MoveGenerator.whiteKing && move.LongCastle)
            {
                board[56] = 0; board[59] = MoveGenerator.whiteRook;
            }
            else if(move.pieceType == MoveGenerator.whiteKing && move.ShortCastle)
            {
                board[63] = 0; board[61] = MoveGenerator.whiteRook;
            }
            else if(move.pieceType == MoveGenerator.blackKing && move.LongCastle)
            {
                board[0] = 0; board[3] = MoveGenerator.blackRook;
            }
            else if(move.pieceType == MoveGenerator.blackKing && move.ShortCastle)
            {
                board[7] = 0; board[5] = 0;
            }

            //////////////////////////////////////   DEBUG BOARD 
            //count++;
            //ShowDebugBoard(shadowBoard, 750, move);
            //////////////////////////////////////   DEBUG BOARD 


            ulong childNodes = CalculateNodes(shadowBoard, depth - 1, turn ^ 1, maxDepth);


            nodes += childNodes;
            if (depth == maxDepth)
            {
                Console.WriteLine($"{MoveToString(move)}: {childNodes}");
            }

            StateSnapshotBase lastSnapshot = stateSnapshots.Pop();
            RestoreStateFromSnapshot(lastSnapshot);

        }

        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
        }

        return nodes;
    }

    public static void RestoreStateFromSnapshot(StateSnapshotBase snapshot)
    {
        Globals.WhiteShortCastle = snapshot.WhiteShortCastle;
        Globals.WhiteLongCastle = snapshot.WhiteLongCastle;
        Globals.BlackShortCastle = snapshot.BlackShortCastle;
        Globals.BlackLongCastle = snapshot.BlackLongCastle;
        Globals.CheckmateWhite = snapshot.CheckmateWhite;
        Globals.CheckmateBlack = snapshot.CheckmateBlack;
        Globals.CheckWhite = snapshot.CheckWhite;
        Globals.CheckBlack = snapshot.CheckBlack;
        Globals.Stalemate = snapshot.Stalemate;
        Globals.LastMoveWasPawn = snapshot.LastMoveWasPawn;
        Globals.LastendSquare = snapshot.LastendSquare;
        Globals.Turn = snapshot.Turn;
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
