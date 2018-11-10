using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        private int posx, posy;
                

        public ChessPiece()
        {
            
            TeamColour = "None";
            PieceType = "None";
            PieceImage = new Image();
            PieceImage.Width = 80;
            PieceImage.Height = 80;
            PieceImage.SetValue(Canvas.ZIndexProperty, 100);
            PieceImage.SetValue(Canvas.IsHitTestVisibleProperty,false);
            SquareSelect.Click += Piece_Click;
            SquareSelect.Style = (Style)Application.Current.Resources["Piece"];

            SquareSelect.Width = 80;
            SquareSelect.Height = 80;
            SquareSelect.HorizontalAlignment = HorizontalAlignment.Center;
            SquareSelect.VerticalAlignment = VerticalAlignment.Center;
            SquareSelect.Margin = new Thickness(0, 0, 0, 0);
            SquareSelect.Content = "";
            SquareSelect.IsEnabled = false;
            SquareSelect.Background = new SolidColorBrush(Color.FromArgb(100,0,0,20));
            SquareSelect.BorderThickness = new Thickness(0);
            //SquareSelect.PointerMoved += Highlight;
            //SquareSelect.PointerEntered += Highlight;
            //SquareSelect.PointerExited += Unhighlight;
            

            ChessSquare.Width = 80;
            ChessSquare.HorizontalAlignment = HorizontalAlignment.Left;
            ChessSquare.VerticalAlignment = VerticalAlignment.Top;
            ChessSquare.Height = 80;
            ChessSquare.Children.Add(GetImage());
            ChessSquare.Children.Add(SquareSelect);
        }
                
        private void Highlight (object sender, PointerRoutedEventArgs e)
        {
            SquareSelect.Background = new SolidColorBrush(Color.FromArgb(100, 50, 50, 150));
            
        }
        private void Unhighlight(object sender, PointerRoutedEventArgs e)
        {
            SquareSelect.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 20));
        }

        public void RefreshTable(string Turn)
        {
            PiecesGrid.Children.Clear();
            Selected = false;
            for (int i = 0; i < 8; i++)
            {
                
                for (int j = 0; j < 8; j++)
                {
                    BoardArray[i][j].SetLocation(new Thickness(80 * i, 80 * j, 0, 0), i, j);
                    PiecesGrid.Children.Add(BoardArray[i][j].GetSquare());
                    BoardArray[i][j].DisablePlayer();
                    BoardArray[i][j].EnablePlayer(Turn,"");
                    
                }
            }
        }

        

        public void EnablePlayer(string TeamTurn, string Secondary = "None",string Enemy = "")//by team
        {
            if(TeamColour==TeamTurn||TeamColour==Secondary||TeamColour == Enemy)
            SquareSelect.IsEnabled = true;
        }

        public void DisablePlayer()
        {
            SquareSelect.IsEnabled = false;
        }

        public void SetLocation(Thickness x, int i, int j)
        {
            ChessSquare.Margin = x;
            List<string> sqnames = new List<string>(new string[] { "A", "B", "C", "D", "E", "F", "G", "H" });
            SquareSelect.Content = sqnames[i] + j.ToString();
            posx = i;
            posy = j;

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
            if(BoardArray[endx][endy].TeamColour != BoardArray[stx][sty].TeamColour)
            {
                ChessPiece Piece = BoardArray[stx][sty];
                BoardArray[endx][endy] = Piece;
                BoardArray[stx][sty] = new ChessPiece();
            }
            else
            {
                ChessPiece Piece = BoardArray[stx][sty];
                BoardArray[stx][sty] = BoardArray[endx][endy];
                BoardArray[endx][endy] = Piece;
            }
        }

        private void Piece_Click(object sender, RoutedEventArgs e)
        {
            int direction;//-1 up/left, 1 down/right;
            string enemy = "Black";
            if(Turn == "White") { direction = -1; enemy = "Black"; } else { direction = 1; enemy = "White"; }
            if (TeamColour == Turn)
            {
                if (!Selected)
                {
                    Selected = true;
                    stx = posx;
                    sty = posy;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            BoardArray[i][j].DisablePlayer();
                            BoardArray[i][j].EnablePlayer(Turn,"");
                        }
                    }
                    BoardArray[posx][posy].EnablePlayer(Turn);
                    switch (PieceType)
                    {
                        case "Pawn": //attack mode not implemented
                            if (Turn == "White")
                            {
                                if (posy + direction >= 0)
                                    BoardArray[posx][posy + direction].EnablePlayer(Turn, Enemy: enemy);
                            }
                            else
                            {
                                if (posy + direction < 8)
                                    BoardArray[posx][posy + direction].EnablePlayer(Turn, Enemy: enemy);
                            }
                            break;

                        case "Knight":
                            if (posx - 1 >= 0 & posy - 2 >= 0)
                            {
                                BoardArray[posx - 1][posy - 2].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx + 1 < 8 & posy + 2 < 8)
                            {
                                BoardArray[posx + 1][posy + 2].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx + 1 < 8 & posy - 2 >= 0)
                            {
                                BoardArray[posx + 1][posy - 2].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx - 1 >= 0 & posy + 2 < 8)
                            {
                                BoardArray[posx - 1][posy + 2].EnablePlayer(Turn, Enemy: enemy);
                            }

                            if (posx - 2 >= 0 & posy - 1 >= 0)
                            {
                                BoardArray[posx - 2][posy - 1].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx + 2 < 8 & posy + 1 < 8)
                            {
                                BoardArray[posx + 2][posy + 1].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx + 2 < 8 & posy - 1 >= 0)
                            {
                                BoardArray[posx + 2][posy - 1].EnablePlayer(Turn, Enemy: enemy);
                            }
                            if (posx - 2 >= 0 & posy + 1 < 8)
                            {
                                BoardArray[posx - 2][posy + 1].EnablePlayer(Turn, Enemy: enemy);
                            }
                            break;

                        case "Bishop":
                            
                            for(int i = 1; i<8; i++)
                            {

                                if (posx + i < 8 & posy + i < 8)
                                {
                                    if (!BoardArray[posx + i][posy + i].HasPiece)
                                    {
                                        BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy);
                                    }
                                    else if (BoardArray[posx + i][posy + i].TeamColour==enemy)
                                    {
                                        BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy);
                                        break;
                                    }
                                    else break;
                                }
                            }
                            for (int i = 1; i < 8; i++)
                            {

                                if (posx - i >= 0 & posy + i < 8)
                                {
                                    if (!BoardArray[posx - i][posy + i].HasPiece)
                                    {
                                        BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy);
                                    }
                                    else if (BoardArray[posx - i][posy + i].TeamColour == enemy)
                                    {
                                        BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy);
                                        break;
                                    }
                                    else break;
                                }
                                
                            }
                            for (int i = 1; i < 8; i++)
                            {

                                if (posx + i < 8 & posy - i >= 0)
                                {
                                    if (!BoardArray[posx + i][posy - i].HasPiece)
                                    {
                                        BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy);
                                    }
                                    else if(BoardArray[posx + i][posy - i].TeamColour == enemy)
                                    {
                                        BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy);
                                        break;
                                    }
                                    else break;
                                }
                                
                            }
                            for (int i = 1; i < 8; i++)
                            {

                                if (posx - i >= 0 & posy - i >= 0)
                                {
                                    if (!BoardArray[posx - i][posy - i].HasPiece)
                                    {
                                        BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy);
                                    }
                                    else if(BoardArray[posx - i][posy - i].TeamColour == enemy)
                                    {
                                        BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy);
                                        break;
                                    }
                                    else break;
                                }
                            }
                            break;
                    }

                }
                else
                {
                    Selected = false;
                    for (int i = 0; i < 8; i++)
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            BoardArray[i][j].DisablePlayer();
                            BoardArray[i][j].EnablePlayer(Turn, "");
                        }
                    }
                }
                
            }
            else
            {
                endx = posx;
                endy = posy;
                MovePiece();
                if(Turn =="White") { Turn = "Black"; } else { Turn = "White"; }
                RefreshTable(Turn);

            }
            
        }
        
    }
}
