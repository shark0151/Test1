using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using static Test1.Game;


namespace Test1
{
    public class ChessPiece
    {
        private string TeamColour;
        private string PieceType;
        private bool HasPiece = false;
        private Image PieceImage = new Image();
        private Grid ChessSquare = new Grid();
        private Button SquareSelect = new Button();
        private List<int> posxy = new List<int>(new int[] { 0, 0 });
        private static List<int> Spos = new List<int>(new int[] { -1, -1 });
        private List<int> Epos = new List<int>(new int[] { 0, 0 });

        public ChessPiece()
        {
            TeamColour = "None";
            PieceType = "None";
            PieceImage = new Image();
            PieceImage.Width = 80;
            PieceImage.Height = 80;
            SquareSelect.Click += Piece_Click;

            SquareSelect.Width = 80;
            SquareSelect.Height = 80;
            SquareSelect.HorizontalAlignment = HorizontalAlignment.Center;
            SquareSelect.VerticalAlignment = VerticalAlignment.Center;
            SquareSelect.Margin = new Thickness(0, 0, 0, 0);
            SquareSelect.Content = "";
            SquareSelect.IsEnabled = false;


            ChessSquare.Width = 80;
            ChessSquare.HorizontalAlignment = HorizontalAlignment.Left;
            ChessSquare.VerticalAlignment = VerticalAlignment.Top;
            ChessSquare.Height = 80;
            ChessSquare.Children.Add(GetImage());
            ChessSquare.Children.Add(SquareSelect);
        }

        

        public void RefreshTable()
        {
            PiecesGrid.Children.Clear();
            for (int i = 0; i < 8; i++)
            {
                
                for (int j = 0; j < 8; j++)
                {
                    BoardArray[i][j].SetLocation(new Thickness(80 * i, 80 * j, 0, 0), i, j);
                    PiecesGrid.Children.Add(BoardArray[i][j].GetSquare());
                    BoardArray[i][j].EnablePlayer("White");
                }
            }
        }

        public void EnablePlayer(string TeamTurn)//by team
        {
            if(TeamColour==TeamTurn || TeamColour=="None") //enables all. fix this
            SquareSelect.IsEnabled = true;
        }

        public void DisablePlayer()
        {
            SquareSelect.IsEnabled = false;
        }

        public void SetLocation(Thickness x, int i, int j)
        {
            ChessSquare.Margin = x;
            SquareSelect.Content = i.ToString() + j.ToString();
            posxy[0] = i;
            posxy[1] = j;


        }
        public Grid GetSquare()
        {
            return ChessSquare;
        }

        public string GetPieceTeam()
        {
            return TeamColour;
        }
        public string GetPieceType()
        {
            return PieceType;
        }
        public void SetPieceType(string type)
        {
            PieceType = type;
        }

        public void SetPieceTeam(string team)
        {
            TeamColour = team;
        }

        public void SetHasPiece(bool pieceactive)
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

        public void MovePiece()
        {

            //BoardArray[Spos[0]][Spos[1]].SetLocation(new Thickness(80 * Epos[0], 80 * Epos[1], 0, 0), Epos[0], Epos[1]);
            //BoardArray[Epos[0]][Epos[1]].SetLocation(new Thickness(80 * Spos[0], 80 * Spos[1], 0, 0), Spos[0], Spos[1]);
            ChessPiece Piece = BoardArray[Spos[0]][Spos[1]];
            BoardArray[Spos[0]][Spos[1]] = BoardArray[Epos[0]][Epos[1]];
            BoardArray[Epos[0]][Epos[1]] = Piece;
        }

        private void Piece_Click(object sender, RoutedEventArgs e)
        {
            if (Spos[0] < 0)
            {
                Spos = posxy;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        BoardArray[i][j].DisablePlayer();
                    }
                }

                ChessPiece CurrentPiece = BoardArray[posxy[0]][posxy[1]];
                CurrentPiece.EnablePlayer("White");

                if (CurrentPiece.GetPieceType() == "Pawn")//if pawn
                {
                    if (posxy[1] < 8)
                        BoardArray[posxy[0]][posxy[1] - 1].EnablePlayer("White");
                }
            }
            else
            {
                Epos = posxy;
                MovePiece();
                Spos[0] = -1;
                Spos[1] = -1;
                RefreshTable();
            }

        }
        
    }
}
