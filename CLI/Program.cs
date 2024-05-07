using Engine;
using Engine.External_Resources;
using System.Net.NetworkInformation;

string fen = "8/k2r4/8/2b1B3/8/q5R1/4Q3/1K6 b - - 0 1";
if (String.IsNullOrEmpty(fen))
{
    fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
}

Globals globals = Globals.FenReader(fen);

//////////////////////   PERFT And stockfish verification

int perftDepth = 1;

RunPerft(fen, globals, perftDepth);

//////////////////////   PERFT And stockfish verification


Run();
void Run()
{

    printBoardWhiteDown(globals.ChessBoard);
    bool running = true;
    while (running)
    {
        Console.Clear();

        var move = Search.FindBestMove(globals.ChessBoard, Globals.Turn, 3);
        MoveHandler.MakeMove(globals.ChessBoard, move);
        printBoardWhiteDown(globals.ChessBoard);
        Thread.Sleep(1000); 
    }
}


void printBoardWhiteDown(int[] board)
{
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
    //showBoardValuesWhite(board);

}

void printBoardBlackDown(int[] board)
{
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
    Console.WriteLine();
    showBoardValuesBlack(board);
    Console.ReadKey();
}


// Value boards 
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
    Console.ReadKey();
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



void VerifyWithStockfish(string fen, int depth)
{
    string stockfishPath = "\"C:\\DATA\\stockfish_15.1_win_x64_avx2\\stockfish-windows-2022-x86-64-avx2.exe\"";
    StockfishIntegration stockfish = new StockfishIntegration(stockfishPath);
    stockfish.StartStockfish();

    // Send the FEN to Stockfish
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

//void MachineMove(Globals globals)
//{
//    int searchDepth = 3;  // Can adjust depth based on needs
//    MoveObject bestMove = Search.FindBestMove(globals.ChessBoard, Globals.Turn, searchDepth);
//    if (!bestMove.Equals(default(MoveObject)))
//    {
//        Console.WriteLine($"Best move: {Globals.GetSquareCoordinate(bestMove.StartSquare)} to {Globals.GetSquareCoordinate(bestMove.EndSquare)}");
//        // Apply the move and print the board again to show the result of the best move

//        MoveHandler.RegisterStaticStates();

//        var pieceMoving = bestMove.pieceType;
//        var targetSquare = globals.ChessBoard[bestMove.EndSquare];
//        var promotedTo = bestMove.PromotionPiece;
//        MoveHandler.MakeMove(globals.ChessBoard, bestMove);
//        Console.WriteLine("Board after the best move:");
//        printBoardWhiteDown(globals.ChessBoard);
//        MoveHandler.UndoMove(globals.ChessBoard, bestMove, pieceMoving, targetSquare, promotedTo);  // Optionally undo to keep original state
//    }
//    else
//    {
//        Console.WriteLine("No legal moves available.");
//    }
//}