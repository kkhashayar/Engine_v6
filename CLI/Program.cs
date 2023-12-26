using Engine;
using System;

// Initiate masks 
MaskGenerator.GenerateAllMasks();

// Console.WriteLine($"{MaskGenerators.KingMasks.Count}");
//foreach (var mask in MaskGenerators.KingMasks)
//{
//    foreach (var position in mask)
//    {
//        Console.Write(position);
//    }
//    Console.WriteLine();
//}

//var startCoordinate = "e5";
//var squareIndex = Globals.GetSquareIndex(startCoordinate);
//var destMoves = MoveGenenerator.GetKnightMoves(squareIndex);

//Console.WriteLine($"From square: {startCoordinate}:" );
//foreach (var move in destMoves)
//{
//    Console.WriteLine($"{Globals.GetSquareCoordinate(move)}");
//}
//Console.ReadLine(); 

/// End of the ask tests 



string fen = "k7/8/7n/8/8/8/8/K2N3R w - - 0 1";
if (String.IsNullOrEmpty(fen))
{
    fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
}

// Creating a mock function to represent the FEN parsing loop in C#

Globals globals = Globals.FenReader(fen);
int fenDept = 1;

Perft.Calculate(globals.ChessBoard, fenDept, globals.Turn);
Console.ReadLine();


Run();
void Run()
{
    bool running = true;
    while (running)
    {
        Console.Clear();
        printBoardWhiteDown(globals.ChessBoard);
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
    showBoardValuesWhite(board);
    
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



