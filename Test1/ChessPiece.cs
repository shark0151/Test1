using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Test1
{
    public class ChessPiece
    {
        private bool TeamColour;
        private string PieceType;
        private bool HasPiece = false;
        private Image PieceImage = new Image();

        public ChessPiece()
        {
            PieceImage.Width = 80;
            PieceImage.Height = 80;
        }
        
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

        public void SetPieceActive(bool pieceactive)
        {
            HasPiece = pieceactive;
        }
        public Image GetImage()
        {
            return PieceImage;
        }
        public void SetImage(BitmapImage imgsource)
        {
            PieceImage.Source = imgsource;
        }


    }
}
