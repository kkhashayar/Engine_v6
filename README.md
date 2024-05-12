Chess Engine in C#


Overview
This C# chess engine is an ongoing project intended for personal learning and improving programming skills. It is currently implemented as a command-line interface, with potential future upgrades to include a graphical user interface. The engine supports standard chess rules including piece movements, castling, en passant, and promotions. Instead of using bitboards, this engine adopts a board-centric approach utilizing arrays of integers to represent the chessboard. Each piece on the board is represented by an integer, facilitating the handling and movement of pieces.
Features

    Core Chess Mechanics: Implements all the standard chess moves including special moves like castling and en passant.
    Move Generation: Generates all possible moves for a given board state, including checks for legal moves.
    Evaluation: Simple material-based evaluation function to assess board positions.
    Perft Testing: Includes functionality for performance testing using perft, which helps in validating the move generation logic by comparing,
    the number of positions generated to known correct values.
    State Management: Tracks game states like castling rights and move histories to ensure moves are legal and to provide undo functionality.
    Search Algorithm: Utilizes a simple search method to predict optimal moves based on the current board state.

Development Status

This project is actively being developed and may contain bugs. It serves as a tool for understanding and implementing the complexities involved in developing a chess engine and for experimenting with different algorithms and techniques in game programming.
Getting Started

To get started with this project:

    Clone the repository to your local machine.
    Ensure you have a C# development environment set up, such as Visual Studio or VSCode with the C# extension.
    Compile and run the program from the command line or through your IDE.
