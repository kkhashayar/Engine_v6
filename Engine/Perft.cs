using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
            // int[] shadowBoard = (int[])board.Clone();

            // MakeMove(move, shadowBoard);

            ///////////// I was here, need to pass the board to MoveHandler /////////////
            
            MoveHandler.MakeMove(ref globals, move, board);

            if (move.pieceType == MoveGenerator.whitePawn || move.pieceType == MoveGenerator.blackPawn) Globals.LastMoveWasPawn = true;
            else Globals.LastMoveWasPawn = false;

            // only last end Square 
            Globals.LastendSquare = move.EndSquare;
            //////////////////////////////////////   DEBUG BOARD 
            count++;
            ShowDebugBoard(board, 500);
            //////////////////////////////////////   DEBUG BOARD 


            ///////////// I was here, need to use MoveHandler, Not using the cloaning.
            //ulong childNodes = CalculateNodes(shadowBoard, depth - 1, turn ^ 1, maxDepth);
            ulong childNodes = CalculateNodes(board, depth - 1, turn ^ 1, maxDepth);

            MoveHandler.UndoMove(ref globals, move, board);
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

    private static void MakeMove(MoveObject move, int[] shadowBoard)
    {
        // White castlings 
        if (move.pieceType == MoveGenerator.whiteKing && move.ShortCastle)
        {
            move.CapturedPiece = 0;
            shadowBoard[62] = move.pieceType;
            shadowBoard[60] = 0;
            shadowBoard[63] = 0;
            shadowBoard[61] = MoveGenerator.whiteRook;

        }
        else if (move.pieceType == MoveGenerator.whiteKing && move.LongCastle)
        {
            move.CapturedPiece = 0;
            shadowBoard[58] = move.pieceType;
            shadowBoard[60] = 0;
            shadowBoard[56] = 0;
            shadowBoard[59] = MoveGenerator.whiteRook;
        }

        // Black castlings 
        else if(move.pieceType == MoveGenerator.blackKing && move.ShortCastle)
        {
            move.CapturedPiece = 0;
            shadowBoard[6] = move.pieceType;
            shadowBoard[4] = 0;
            shadowBoard[7] = 0;
            shadowBoard[5] = MoveGenerator.blackRook;
        }
        else if(move.pieceType == MoveGenerator.blackKing && move.LongCastle)
        {
            move.CapturedPiece = 0;
            shadowBoard[move.EndSquare] = move.pieceType;
            shadowBoard[4] = 0;
            shadowBoard[0] = 0;
            shadowBoard[3] = MoveGenerator.blackRook;
        }
        
        // Applying normal moves 
        else
        {
            move.CapturedPiece = shadowBoard[move.EndSquare];
            //if (move.pieceType == MoveGenerator.whitePawn && move.IsPromotion) move.pieceType = MoveGenerator.whitePawn;
            //else if(move.pieceType == MoveGenerator.blackPawn && move.IsPromotion) move.pieceType = MoveGenerator.blackPawn;
            shadowBoard[move.EndSquare] = move.pieceType;
            shadowBoard[move.StartSquare] = 0;
        }
        
    }

    public static void ShowDebugBoard(int[] board, int speedMs)
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
