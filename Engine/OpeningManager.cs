using System.Collections.Generic;

namespace Engine
{
    internal class OpeningManager
    {
       
        public string GetOpeningMove(int[]board, int turn)
        {
            string positionFen = Globals.BoardToFen(board, turn);



           
            return null;
        }

        public string GetFen(string fen)
        {
            return string.Empty;// have to connect to database 
        }
    }
}
