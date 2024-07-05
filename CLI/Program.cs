using Engine;
using Engine.Core;
using Engine.External_Resources;

// test fen: 6k1/5p1p/2Q1p1p1/5n1r/N7/1B3P1P/1PP3PK/4q3 b - - 0 1            mate in 3
// test fen: rn4k1/pp1r1pp1/1q1b4/5QN1/5N2/4P3/PP3PPP/3R1RK1 w - - 1 0       mate in 3

// test fen: r1b1rk2/ppq3p1/2nbpp2/3pN1BQ/2PP4/7R/PP3PPP/R5K1 w - - 1 0      mate in 4
// test fen: br1qr1k1/b1pnnp2/p2p2p1/P4PB1/3NP2Q/2P3N1/B5PP/R3R1K1 w - - 1 0 mate in 4
// test fen: rn3rk1/pbppq1pp/1p2pb2/4N2Q/3PN3/3B4/PPP2PPP/R3K2R w KQ - 7 11  mate in 7


// test fen: 8/8/3k4/8/4R3/3K4/8/8 w - - 0 1     KkR
// test fen:  8/8/3rk3/8/8/5K2/8/8 b - - 0 1     Kkr

string fen = "";


Globals globals = Globals.FenReader(fen);

//////////////////   PERFT And stockfish verification
// int perftDepth = 3;
// RunPerft(fen, globals, perftDepth);
//////////////////   PERFT And stockfish verification



int searchDepth = Globals.MaxDepth;
Globals.GetGamePhase();

TimeSpan maxTime = TimeSpan.FromSeconds(Globals.ThinkingTime);

//Console.WriteLine($"Max time: {Globals.ThinkingTime}  Max Depth: {searchDepth}");
//Console.WriteLine("Press a key to continue");
//Console.ReadKey();  

Run();

void Run()
{
    Console.WriteLine();
    if (Globals.InitialTurn == 0) printBoardWhiteDown(globals.ChessBoard);
    else if (Globals.InitialTurn == 1) printBoardBlackDown(globals.ChessBoard);
    
    Console.WriteLine();
    Console.WriteLine();

    bool running = true;
    Globals.TotalTime.Restart();
    
    while (running)
    {
        MoveObject move = new MoveObject();
        move = Search.GetBestMove(globals.ChessBoard, Globals.Turn, searchDepth, maxTime);
        
        MoveHandler.MakeMove(globals.ChessBoard, move);
        Console.Beep(2000, 100);
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
            Globals.TotalTime.Stop();
            break;
        }

    }

    Console.WriteLine();
    Console.WriteLine($"Position: {fen} \n");
    Console.WriteLine("Solved on: " + (Globals.TotalTime.ElapsedMilliseconds / 1000.0).ToString() + " seconds");

    Console.WriteLine();
    foreach (var move in Globals.moveHistory)
    {
        Console.Write(Globals.MoveToString(move));
    }
    
    Console.Beep(1500, 500);
    Console.Beep(1500, 500);
    Console.ReadKey();
}


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
    //Console.WriteLine();
    //showBoardValuesBlack(board);
    //Console.ReadKey();
}


//Data  boards 
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
    Console.ReadLine();
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

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////////////////////////////////////////////  UCI   /////////////////////////////////////////////////////////////////////////// 
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

//Globals globals = Globals.FenReader("");
void StartUCIMode()
{
    while (true)
    {
        string input = Console.ReadLine();
        Console.WriteLine($"Received command: {input}");

        if (input == "uci")
        {
            Console.WriteLine("id name KChess.v6");
            Console.WriteLine("id author Khashayar Nariman");
            Console.WriteLine("uciok");
        }
        else if (input == "isready")
        {
            Console.WriteLine("readyok");
        }
        else if (input.StartsWith("position"))
        {
            HandlePositionCommand(input);
        }
        else if (input.StartsWith("go"))
        {
            HandleGoCommand(input);
        }
        else if (input == "quit")
        {
            break;
        }
    }
}

void HandlePositionCommand(string input)
{
    // Determine if the input is for a starting position or a specific FEN
    if (input.StartsWith("position startpos"))
    {
        globals = Globals.FenReader("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
    else if (input.StartsWith("position fen"))
    {
        string fen = input.Substring(13);
        globals = Globals.FenReader(fen);
    }

    // Check for moves after the position setup
    string[] parts = input.Split(' ');
    int moveIndex = Array.IndexOf(parts, "moves");
    if (moveIndex != -1)
    {
        for (int i = moveIndex + 1; i < parts.Length; i++)
        {
            MoveObject move = StringToMove(parts[i]);
            MoveHandler.MakeMove(globals.ChessBoard, move);
            Globals.Turn ^= 1;  // Switch turns with each move made
            Console.WriteLine($"Made move: {parts[i]}");
        }
    }
}


void HandleGoCommand(string input)
{
    //Globals.Turn ^= 1;
    Console.WriteLine("Received go command");

    int maxDepth = 1;
    TimeSpan maxTime = TimeSpan.FromSeconds(15);

    try
    {
        MoveObject bestMove = default;

        bestMove = Search.GetBestMove(globals.ChessBoard, Globals.Turn, searchDepth, maxTime);

        string bestMoveString = Globals.ConvertMoveToString(bestMove);
        Console.WriteLine($"bestmove {bestMoveString}");

        MoveHandler.MakeMove(globals.ChessBoard, bestMove);
        //Globals.Turn ^= 1;  // Switch turns after making a move
        Globals.CurrentFEN = Globals.BoardToFen(globals.ChessBoard, Globals.Turn);
        Console.WriteLine("Move calculation completed");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during move calculation: {ex.Message}");
    }
}


MoveObject StringToMove(string moveString)
{
    int startSquare = ConvertToBoardIndex(moveString.Substring(0, 2));
    int endSquare = ConvertToBoardIndex(moveString.Substring(2, 2));

    return new MoveObject
    {
        StartSquare = startSquare,
        EndSquare = endSquare,
        pieceType = globals.ChessBoard[startSquare],
        IsCapture = globals.ChessBoard[endSquare] != Piece.None

    };
}

int ConvertToBoardIndex(string position)
{
    int file = position[0] - 'a';
    int rank = position[1] - '1';
    return rank * 8 + file;
}

// In order to test the UCI mode, comment out the Run() method and call StartUCIMode() instead
//StartUCIMode();
// --------------------------- End of UCI Section ---------------------------


// List of UCI commands
/*
 
uci                 // Initializes the engine and it responds with its options and sends 'uciok' when ready.
debug [on/off]      // Turns debug mode on or off.
isready             // Checks if the engine is ready, to which it should respond with 'readyok'.
setoption name [optionname] value [value] // Sets an engine option.
register name [name] code [code]          // Registers the engine if required.
ucinewgame          // Indicates that a new game is starting.
position [startpos | fen fenstring] moves [move1] ... [moveN] // Sets up the board position.
go                  // Starts the engine calculation. Subcommands include:
  ponder            // Engine thinks in the background.
  searchmoves move1 ... moveN // Restrict search to these moves.
  wtime [time]      // Time remaining for white in milliseconds.
  btime [time]      // Time remaining for black in milliseconds.
  winc [time]       // Increment per move for white in milliseconds.
  binc [time]       // Increment per move for black in milliseconds.
  movestogo [num]   // Number of moves to the next time control.
  depth [x]         // Search x plies only.
  nodes [n]         // Search n nodes only.
  mate [m]          // Search for a mate in m moves.
  movetime [ms]     // Think exactly ms milliseconds.
  infinite          // Search until the 'stop' command.
stop                // Stops the engine's calculation.
ponderhit           // Used when the opponent makes the expected move.
quit                // Shuts down the engine.

 */