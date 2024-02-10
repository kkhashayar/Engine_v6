using System.Diagnostics;

namespace Engine;

public static class Perft
{
    public static Globals globals  = new Globals();
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

            MoveHandler.MakeMove(move, board);

            //////////////////////////////////////   DEBUG BOARD 
            count++;
            ShowDebugBoard(board, 1000, move);
            //////////////////////////////////////   DEBUG BOARD 


            ulong childNodes = CalculateNodes(board, depth - 1, turn ^ 1, maxDepth);

           
           
            nodes += childNodes;
            if (depth == maxDepth)
            {
                Console.WriteLine($"{MoveToString(move)}: {childNodes}");
            }
            MoveHandler.UndoMove(move, board);
        }

        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
        }

        return nodes;
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
