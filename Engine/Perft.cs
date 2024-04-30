using System.Diagnostics;
using System.Reflection.Metadata;

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
            RegisterStaticStates();
            var pieceMoving = move.pieceType;
            var targetSquare = board[move.EndSquare];
            MakeMove(board, move);

            ////////////////////////////////////   DEBUG BOARD 
            //count++;
            //ShowDebugBoard(board, 300, move);
            ////////////////////////////////////   DEBUG BOARD 

            ulong childNodes = CalculateNodes(board, depth - 1, turn ^ 1, maxDepth);

            UndoMove(board, move, pieceMoving, targetSquare);
            RestoreStateFromSnapshot();
            
            
           
            
            nodes += childNodes;
            if (depth == maxDepth)
            {
                Console.WriteLine($"{MoveToString(move)}: {childNodes}");
            }
        }

        if (depth == maxDepth)
        {
            Console.WriteLine($"Nodes searched: {nodes}");
        }

        return nodes;
    }

    private static void UndoMove(int[] board, MoveObject move, int pieceMoving, int targetSquare)
    {
        board[move.StartSquare] = pieceMoving;
        board[move.EndSquare] = targetSquare;

        if(move.pieceType == MoveGenerator.whiteKing)
        {
            if (move.ShortCastle)
            {
                board[61] = 0;  
                board[63] = MoveGenerator.whiteRook;
            }
        }

        //if(move.ShortCastle && move.pieceType == MoveGenerator.blackKing)
        //{
        //    board[5] = 0; board[6] = 0; board[7] = MoveGenerator.blackRook;
        //}
        //else if(move.LongCastle && move.pieceType == MoveGenerator.blackKing)
        //{
        //    board[3] = 0; board[2] = 0; board[0] = MoveGenerator.blackRook;
        //}
        
    }

    private static void MakeMove(int[] board, MoveObject move)
    {
        board[move.EndSquare] = move.pieceType;
        board[move.StartSquare] = 0;
        
        if(move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 63 && Globals.WhiteKingRookMoved is false)
        {
            Globals.WhiteQueenRookMoved = true;
        }
        else if (move.pieceType == MoveGenerator.whiteRook && move.StartSquare == 56 && Globals.WhiteQueenRookMoved is false)
        {
            Globals.WhiteKingRookMoved = true;
        }

        else if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 0 && Globals.BlackKingRookMoved is false)
        {
            Globals.BlackQueenRookMoved = true;
        }
        else if (move.pieceType == MoveGenerator.blackRook && move.StartSquare == 7 && Globals.BlackQueenRookMoved is false)
        {
            Globals.BlackKingRookMoved = true;
        }

        if (move.pieceType == MoveGenerator.whiteKing && move.LongCastle && Globals.WhiteQueenRookMoved is false)
        {
            board[56] = 0; board[59] = MoveGenerator.whiteRook;
            Globals.WhiteQueenRookMoved = true;
            Globals.WhiteLongCastle = false;
            Globals.WhiteShortCastle = false;   
        }

        else if (move.pieceType == MoveGenerator.whiteKing && move.ShortCastle && Globals.WhiteKingRookMoved is false)
        {
            board[63] = 0; board[61] = MoveGenerator.whiteRook;
            Globals.WhiteKingRookMoved = true;
            Globals.WhiteLongCastle = false;
            Globals.WhiteShortCastle = false;
        }

        if (move.pieceType == MoveGenerator.blackKing && move.LongCastle && Globals.BlackQueenRookMoved is false)
        {
            board[0] = 0; board[3] = MoveGenerator.blackRook;
            Globals.BlackKingRookMoved = true;   
            Globals.BlackLongCastle = false;    
            Globals.BlackShortCastle = false;       
        }
        else if (move.pieceType == MoveGenerator.blackKing && move.ShortCastle && Globals.BlackKingRookMoved is false)
        {
            board[7] = 0; board[5] = 0;
            Globals.BlackQueenRookMoved = true; 
            Globals.BlackLongCastle = false;
            Globals.BlackShortCastle = false;
        }
    }

    private static void RegisterStaticStates()
    {

        StateSnapshotBase.WhiteShortCastle = Globals.WhiteShortCastle;
        StateSnapshotBase.WhiteLongCastle = Globals.WhiteLongCastle;
        StateSnapshotBase.WhiteKingRookMoved = Globals.WhiteKingRookMoved;
        StateSnapshotBase.WhiteQueenRookMoved = Globals.WhiteQueenRookMoved;

        StateSnapshotBase.BlackShortCastle = Globals.BlackShortCastle;
        StateSnapshotBase.BlackLongCastle = Globals.BlackLongCastle;
        StateSnapshotBase.BlackKingRookMoved = Globals.BlackKingRookMoved;
        StateSnapshotBase.BlackQueenRookMoved = Globals.BlackQueenRookMoved;

        StateSnapshotBase.CheckmateWhite = Globals.CheckmateWhite;
        StateSnapshotBase.CheckmateBlack = Globals.CheckmateBlack;
        StateSnapshotBase.CheckWhite = Globals.CheckWhite;
        StateSnapshotBase.CheckBlack = Globals.CheckBlack;
        StateSnapshotBase.Stalemate = Globals.Stalemate;
        StateSnapshotBase.LastMoveWasPawn = Globals.LastMoveWasPawn;
        StateSnapshotBase.LastEndSquare = Globals.LastEndSquare;
        StateSnapshotBase.Turn = Globals.Turn;
    }

    public static void RestoreStateFromSnapshot()
    {
        Globals.WhiteShortCastle = StateSnapshotBase.WhiteShortCastle;
        Globals.WhiteLongCastle = StateSnapshotBase.WhiteLongCastle;
        Globals.WhiteKingRookMoved = StateSnapshotBase.WhiteKingRookMoved; 
        Globals.WhiteQueenRookMoved = StateSnapshotBase.WhiteQueenRookMoved;

        Globals.BlackShortCastle = StateSnapshotBase.BlackShortCastle;
        Globals.BlackLongCastle = StateSnapshotBase.BlackLongCastle;
        Globals.BlackKingRookMoved = StateSnapshotBase.BlackKingRookMoved;
        Globals.BlackQueenRookMoved = StateSnapshotBase.BlackQueenRookMoved;

        Globals.CheckmateWhite = StateSnapshotBase.CheckmateWhite;
        Globals.CheckmateBlack = StateSnapshotBase.CheckmateBlack;
        Globals.CheckWhite = StateSnapshotBase.CheckWhite;
        Globals.CheckBlack = StateSnapshotBase.CheckBlack;
        Globals.Stalemate = StateSnapshotBase.Stalemate;
        Globals.LastMoveWasPawn = StateSnapshotBase.LastMoveWasPawn;
        Globals.LastEndSquare = StateSnapshotBase.LastEndSquare;
        Globals.Turn = StateSnapshotBase.Turn;
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
