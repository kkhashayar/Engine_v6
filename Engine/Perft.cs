using System.Diagnostics;

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
            int[] shadowBoard = (int[])board.Clone();
            MakeMove(move, shadowBoard);


            //////////////////////////////////////   DEBUG BOARD 
            count++;
            ShowDebugBoard(shadowBoard, 2000);
            //////////////////////////////////////   DEBUG BOARD 


            ulong childNodes = CalculateNodes(shadowBoard, depth - 1, turn ^ 1, maxDepth);
            nodes += childNodes;

            if (depth == maxDepth)
            {
                // Output each move and its nodes
                Console.WriteLine($"{MoveToString(move)}: {childNodes}"); 
            }
        }

        if (depth == maxDepth)
        {
            // Output total nodes searched at the initial call
            Console.WriteLine($"Nodes searched: {nodes}");
        }

        return nodes;
    }

    private static string MoveToString(MoveObject move)
    {
       return $"{Globals.GetSquareCoordinate(move.StartSquare)}{Globals.GetSquareCoordinate(move.EndSquare)}"; 
    }

    private static void MakeMove(MoveObject move, int[] board)
    {
        move.CapturedPiece = board[move.EndSquare];
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;
    }

    public static void ShowDebugBoard(int []board, int speedMs)
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
        Thread.Sleep(speedMs);
        
    }

}
