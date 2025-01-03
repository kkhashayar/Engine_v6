﻿using Engine;
using Engine.Core;
using Engine.External_Resources;

#region Entry loop

// fen: 6k1/5p1p/2Q1p1p1/5n1r/N7/1B3P1P/1PP3PK/4q3 b - - 0 1                mate in 3
// fen: rn4k1/pp1r1pp1/1q1b4/5QN1/5N2/4P3/PP3PPP/3R1RK1 w - - 1 0           mate in 3
// fen: r1b1rk2/ppq3p1/2nbpp2/3pN1BQ/2PP4/7R/PP3PPP/R5K1 w - - 1 0          mate in 4
// fen: br1qr1k1/b1pnnp2/p2p2p1/P4PB1/3NP2Q/2P3N1/B5PP/R3R1K1 w - - 1 0     mate in 4
// fen: rn3rk1/pbppq1pp/1p2pb2/4N2Q/3PN3/3B4/PPP2PPP/R3K2R w KQ - 7 11      mate in 7
// fen: rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1

// two kings:           8/8/7k/8/7K/8/8/8 w - - 0 1
string fen = "6k1/5p1p/2Q1p1p1/5n1r/N7/1B3P1P/1PP3PK/4q3 b - - 0 1";

Globals globals = Globals.FenReader(fen);

//////////////////   PERFT And stockfish verification
// Still some mistakes in positions with pawns! 
int perftDepth = 4;
RunPerft(fen, globals, perftDepth);
//////////////////   PERFT And stockfish verification

///////// SETTINGS
Globals.OpeningTime = 5;
Globals.MiddleGameTime = 45;
Globals.EndGameTime = 15;
Globals.MaxDepth = 20;
Globals.QuQuiescenceSwitch = true;
Globals.QuiescenceDepth = 4;
Globals.DepthBalancer = 2;

Run();
Console.Clear();
void Run()
{
    Console.Clear();
    Console.WriteLine();
    if (Globals.InitialTurn == 0) printBoardWhiteDown(globals.ChessBoard);
    else if (Globals.InitialTurn == 1) printBoardBlackDown(globals.ChessBoard);

    Console.WriteLine();
    Console.WriteLine();

    bool running = true;
    

    while (running)
    {
        MoveObject move = new MoveObject();
        move = Search.GetBestMove(globals.ChessBoard, Globals.Turn);
        
        var testmove = move;
        
        MoveHandler.MakeMove(globals.ChessBoard, move, Globals.Turn);

        Console.Beep(800, 70);

        Globals.moveHistory.Add(move);

        Globals.Turn ^= 1;

        Console.Clear();
        Console.WriteLine();

        if (Globals.InitialTurn == 0) printBoardWhiteDown(globals.ChessBoard);
        else if (Globals.InitialTurn == 1) printBoardBlackDown(globals.ChessBoard);

        Console.WriteLine();

        if (Globals.CheckmateWhite || Globals.CheckmateBlack || Globals.Stalemate)
        {
            running = false;
            
            break;
        }
        
    }

    Console.WriteLine();
    Console.WriteLine($"Position: {fen} \n");
    

    Console.WriteLine();
    foreach (var move in Globals.moveHistory)
    {
        Console.Write(Globals.MoveToString(move));
    }
    
    Console.ReadKey();
}
#endregion


#region Board Printing and data boards
void printBoardWhiteDown(int[] board)
{
    Console.OutputEncoding = System.Text.Encoding.Unicode;
    string[] fileNames = { "A", "B", "C", "D", "E", "F", "G", "H" };
    var ranks = new int[] { 8, 7, 6, 5, 4, 3, 2, 1 };
    for (int rank = 0; rank < 8; rank++)
    {
        Console.Write($"{ranks[rank]}  ");  // Print rank number on the left of the board
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
    showBoardValuesWhite(board);
}

void printBoardBlackDown(int[] board)
{
    Console.ResetColor();
    Console.ForegroundColor = ConsoleColor.Black;
    Console.OutputEncoding = System.Text.Encoding.Unicode;
    string[] fileNames = { "H", "G", "F", "E", "D", "C", "B", "A" };

    for (int rank = 7; rank >= 0; rank--)
    {
        Console.Write((8 - rank) + " ");
        foreach (var file in fileNames.Select((value, index) => new { value, index }))
        {
            int index = rank * 8 + (7 - file.index);
            char pieceChar = Globals.GetUnicodeCharacter(board[index]);
            Console.Write(pieceChar + " "); // Print the piece
        }
        Console.WriteLine();
    }

    Console.Write("  "); // Align file names with the board
    foreach (var fileName in fileNames)
    {
        Console.Write(fileName + " "); // Print file names
    }
    
}
void showBoardValuesWhite(int[] board)
{
    Console.WriteLine();
    for (int rank = 0; rank < 8; rank++)
    {
        Console.Write((8 - rank) + " ");
        for (int file = 0; file < 8; file++)
        {
            int index = rank * 8 + file;
            int pieceValue = board[index];
            Console.Write(pieceValue.ToString().PadLeft(3));
        }
        Console.WriteLine();
    }
   
}

void showBoardValuesBlack(int[] board)
{
    Console.WriteLine();

    for (int rank = 7; rank >= 0; rank--)
    {

        Console.Write((8 - rank) + " ");
        // Loop through each file in reverse order using 'foreach'
        for (int file = 7; file >= 0; file--)
        {
            int index = rank * 8 + file;
            int pieceValue = board[index]; // Get the value representing the piece
            Console.Write(pieceValue.ToString().PadLeft(3) + " ");
        }
        Console.WriteLine();
    }
    Console.ReadKey();
}

#endregion

#region Perft and Stockfish Verification
void VerifyWithStockfish(string fen, int depth)
{
    string stockfishPath = "\"D:\\DATA\\stockfish_15.1_win_x64_avx2\\stockfish-windows-2022-x86-64-avx2.exe\"";
    StockfishIntegration stockfish = new StockfishIntegration(stockfishPath);
    stockfish.StartStockfish();

    // Send FEN to Stockfish
    stockfish.SendCommand($"position fen {fen}");
    stockfish.SendCommand($"go perft {depth}");

    // Read the output
    string output;

    while ((output = stockfish.ReadOutput()) != null)
    {

        Console.WriteLine(output);
        if (output.StartsWith("Stockfish result:  ")) break;
    }

}
void RunPerft(string fen, Globals globals, int perftDepth)
{

    Console.ForegroundColor = ConsoleColor.Black;
    Console.WriteLine("******* Engine 6 *******  \n");
    Console.WriteLine($"Perft test in depth: {perftDepth} on: \n");
    Console.WriteLine($"{fen} \n");
    Perft.Calculate(globals.ChessBoard, perftDepth, Globals.Turn);
    Console.WriteLine();
    Console.Beep(2000, 50);
    Console.WriteLine("Press 'V' to verify with Stockfish or 'I' \n");
    Console.WriteLine("Press 'I' to increase depth and test again \n");
    Console.WriteLine("Enter to return to Boards \n");
    char input = Console.ReadKey().KeyChar;
    if (input == 'v' || input == 'V')
    {
        VerifyWithStockfish(fen, perftDepth);
        Console.WriteLine("Press any key to continue\n");
        Console.ReadKey();
        RunPerft(fen, globals, perftDepth);

    }
    else if (input == 'i' || input == 'I')
    {
        RunPerft(fen, globals, perftDepth + 1);
        Console.Beep(1500, 50);
        Console.Beep(1500, 50);
    }
}
#endregion



