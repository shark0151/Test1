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

        public bool GetPieceTeam()
        {
            //false = black, true = white
            if (TeamColour == true)
                return true;
            else return false;
        }
        public string GetPieceType()
        {
            return PieceType;
        }
        public void SetPieceType(string type)
        {
            PieceType = type;
        }

        public void SetPieceTeam(bool team)
        {
            TeamColour = team;
        }

        private class MovePiece
        {

        }

        
    }
}
