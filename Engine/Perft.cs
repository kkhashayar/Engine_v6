using System.Diagnostics;

namespace Engine;

public static class Perft
{
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
        List<MoveObject> moves = MoveGenenerator.GenerateAllMoves(board, turn);

        foreach (MoveObject move in moves)
        {
            MakeMove(move, board);
            ulong childNodes = CalculateNodes(board, depth - 1, turn ^ 1, maxDepth);
            UnmakeMove(move, board);

            // Print each move and nodes at the final layer of the desired depth
            if (depth == maxDepth)
            {
                Console.WriteLine($"{Globals.GetSquareCoordinate(move.StatrSquare)}{Globals.GetSquareCoordinate(move.EndSquare)}: {childNodes}");
            }

            nodes += childNodes;
        }

        // Print the total nodes at the end of the top call
        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
            Console.Beep(1500, 50);
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
}
