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

        // Output results
        Console.WriteLine($"Depth: {depth}, Nodes: {nodes}, Time: {stopwatch.Elapsed.TotalSeconds} seconds");

        return nodes;
    }

    public static ulong CalculateNodes(int[] board, int depth, int turn, int maxDepth)
    {
        if (depth == 0) return 1;

        ulong nodes = 0;
        List<MoveObject> moves = MoveGenerator.GenerateAllMoves(board, turn);

        foreach (MoveObject move in moves)
        {
            MakeMove(move, board);

            //count++;
            //DebugPerft(board);

            ulong childNodes = CalculateNodes(board, depth - 1, turn ^ 1, maxDepth);
            UnmakeMove(move, board);

            // Increment the nodes count
            nodes += childNodes;

            
            if (depth == 2) 
            {
                Console.WriteLine($"{Globals.GetSquareCoordinate(move.StatrSquare)}{Globals.GetSquareCoordinate(move.EndSquare)}: {childNodes}");
            }
        }

        // Print the total nodes at the end of the top call
        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
            Console.Beep(1500, 50); // Optional beep
        }

        
        return nodes;
    }



    private static void MakeMove(MoveObject move, int[] board)
    {
        move.CapturedPiece = board[move.EndSquare];
        board[move.EndSquare] = move.pieceType;
        board[move.StatrSquare] = 0;
    }

    private static void UnmakeMove(MoveObject move, int[] board)
    {
        board[move.EndSquare] = move.CapturedPiece;
        board[move.StatrSquare] = move.pieceType;
    }




    public static void DebugPerft(int []board)
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
        Thread.Sleep(400);
        
    }





}
