using Engine;

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

var startCoordinate = "e5";
var squareIndex = Globals.GetSquareIndex(startCoordinate);
var destMoves = MoveGenenerator.GetKnightMoves(squareIndex);

Console.WriteLine($"From square: {startCoordinate}:" );
foreach (var move in destMoves)
{
    Console.WriteLine($"{Globals.GetSquareCoordinate(move)}");
}
Console.ReadLine(); 

/// End of the ask tests 










string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
if (String.IsNullOrEmpty(fen))
{
    fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
}
Globals board = Globals.FenReader(fen);






Run();
void Run()
{
    bool running = true;
    while (running)
    {
        Console.Clear();
        printBoardWhiteDown(board);
    }
}

void printBoardWhiteDown(Globals board)
{
    Console.OutputEncoding = System.Text.Encoding.Unicode;
    string[] fileNames = { "A", "B", "C", "D", "E", "F", "G", "H" };

    for (int rank = 7; rank >= 0; rank--)
    {
        Console.Write((rank + 1) + " "); 
        for (int file = 0; file < 8; file++)
        {
            int index = rank * 8 + file;
            char pieceChar = GetUnicodeCharacter(board.ChessBoard[index]);
            Console.Write(pieceChar + " ");
        }
        Console.WriteLine();
    }
    Console.Write("  ");
    foreach (var fileName in fileNames)
    {
        Console.Write(fileName + " ");
    }
    // Value board
    Console.WriteLine();
    Console.WriteLine();
    showBoardValuesWhite(board);
    // TODO: user input 
    Console.ReadKey(); 
}

void printBoardBlackDown(Globals board)
{
    Console.OutputEncoding = System.Text.Encoding.Unicode;
    string[] fileNames = { "H", "G", "F", "E", "D", "C", "B", "A" }; // Reversed file names

    for (int rank = 0; rank < 8; rank++)
    {
        Console.Write((8 - rank) + " "); // Print the rank number (8-1) on the left
        for (int file = 7; file >= 0; file--)
        {
            int index = rank * 8 + file;
            char pieceChar = GetUnicodeCharacter(board.ChessBoard[index]);
            Console.Write(pieceChar + " ");
        }
        Console.WriteLine();
    }

    // Print reversed file names (H-A) at the bottom
    Console.Write("  ");
    foreach (var fileName in fileNames)
    {
        Console.Write(fileName + " ");
    }
    Console.WriteLine();
    Console.WriteLine();
    showBoardValuesBlack(board);
    // TODO: user input 
    Console.ReadKey();
}

// Value boards 
void showBoardValuesWhite(Globals board)
{
    for (int rank = 7; rank >= 0; rank--)
    {
        for (int file = 0; file < 8; file++)
        {
            int index = rank * 8 + file;
            int pieceValue = board.ChessBoard[index];
            Console.Write(pieceValue.ToString().PadLeft(3)); // Pad each number to 3 characters wide
        }
        Console.WriteLine();
    }
    Console.ReadKey();
}

void showBoardValuesBlack(Globals board)
{
    for (int rank = 0; rank < 8; rank++)
    {
        for (int file = 7; file >= 0; file--)
        {
            int index = rank * 8 + file;
            int pieceValue = board.ChessBoard[index];
            Console.Write(pieceValue.ToString().PadLeft(3)); // Pad each number to 3 characters wide
        }
        Console.WriteLine();
    }
    Console.ReadKey();
}




char GetUnicodeCharacter(int pieceCode)
{
    switch (pieceCode)
    {
        case 1: return '\u2659'; // White Pawn
        case 3: return '\u2658'; // White Knight
        case 4: return '\u2657'; // White Bishop
        case 5: return '\u2656'; // White Rook
        case 9: return '\u2655'; // White Queen
        case 99: return '\u2654'; // White King
        case 11: return '\u265F'; // Black Pawn
        case 13: return '\u265E'; // Black Knight
        case 14: return '\u265D'; // Black Bishop
        case 15: return '\u265C'; // Black Rook
        case 19: return '\u265B'; // Black Queen
        case 109: return '\u265A'; // Black King
        default: return '.'; // Empty square
    }
}

List<char> Unicodes = new List<char>
        {
            '\u2659','\u2658','\u2657','\u2656',
            '\u2655','\u2654','\u265F','\u265E',
            '\u265D','\u265C','\u265B','\u265A'
        };

