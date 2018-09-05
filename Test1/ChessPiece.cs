using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test1
{
    public class ChessPiece
    {
        public ChessPiece()
        {

        }

        private bool TeamColour;
        private string PieceType;

        private void MovePiece()
        {

        }

        private List<string> PieceTypes = new List<string>(new string[] { "Pawn", "Knight", "Bishop" , "Rook" , "Queen" , "King" });
    }
}
