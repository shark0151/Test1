using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using static Test1.Game;


namespace Test1
{
    public class ChessPiece : IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        private string TeamColour;
        private string PieceType;
        private bool HasPiece = false;
        private int SpecialMove;
        private Image PieceImage = new Image();
        private Grid ChessSquare = new Grid();
        private Button SquareSelect = new Button();
        private int posx, posy;
        private SolidColorBrush highlightcolor = new SolidColorBrush(Color.FromArgb(150, 0, 0, 150));
        private SolidColorBrush normalcolor = new SolidColorBrush(Color.FromArgb(100, 20, 20, 20));
        private string status="Playing";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }

        public ChessPiece()
        {
            Dispose(false);

            SpecialMove = 0;
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
            SquareSelect.Background = normalcolor;
            SquareSelect.BorderThickness = new Thickness(0);
                        

            ChessSquare.Width = 80;
            ChessSquare.HorizontalAlignment = HorizontalAlignment.Left;
            ChessSquare.VerticalAlignment = VerticalAlignment.Top;
            ChessSquare.Height = 80;
            ChessSquare.Children.Add(GetImage());
            ChessSquare.Children.Add(SquareSelect);
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
        public bool GetHasPiece()
        {
            return HasPiece;
        }
        public Image GetImage()
        {
            return PieceImage;
        }
        public int GetSpecial()
        {
            return SpecialMove;
        }
        public void SetLocation(Thickness x, int i, int j)
        {
            ChessSquare.Margin = x;
            List<string> sqnames = new List<string>(new string[] { "A", "B", "C", "D", "E", "F", "G", "H" });
            SquareSelect.Content = sqnames[i] + j.ToString();
            posx = i;
            posy = j;

        }
        public void SetImage(BitmapImage imgsource)
        {
            PieceImage.Source = imgsource;
        }
        public void SetPieceType(string type)
        {
            PieceType = type;
            if (type == "Pawn")
                SpecialMove = 1; //2 for checking En passant. only availabe a round if pawn moved two squares ; 3 for end pos
            if (type == "King")
                SpecialMove = 4;
            if (type == "Rook")
                SpecialMove = 5;
        }
        public void SetPieceTeam(string team)
        {
            TeamColour = team;
        }
        public void SetHasPiece(bool pieceactive)
        {
            HasPiece = pieceactive;
        }
        
        public void EnablePlayer(string TeamTurn, string Secondary = "None", string Enemy = "", bool showmove = false)
        {
            if (TeamColour == TeamTurn || TeamColour == Secondary || TeamColour == Enemy)
            {
                SquareSelect.IsEnabled = true;
                if(PieceType=="Pawn" && SpecialMove == 2)
                { SpecialMove = 0; }              
                if (showmove == true)
                {
                    SquareSelect.Background = highlightcolor; //highlight color
                }
                else SquareSelect.Background = normalcolor; //normal
            }
        }
        public void DisablePlayer()
        {
            SquareSelect.IsEnabled = false;
        }

        private async Task RefreshTable()
        {
            GameOver();
            PiecesGrid.Children.Clear();
            
            Selected = false;
            for (int i = 0; i < 8; i++)
            {

                for (int j = 0; j < 8; j++)
                {
                    BoardArray[i][j].SetLocation(new Thickness(80 * i, 80 * j, 0, 0), i, j);
                    PiecesGrid.Children.Add(BoardArray[i][j].GetSquare());
                    BoardArray[i][j].DisablePlayer();
                    if (status == "Playing")
                    {
                        BoardArray[i][j].EnablePlayer(Turn, "");
                    }

                }
            }

        }

        private void GameOver()
        {
            string Play_as = "None";
            string enemy = "";
            void checkscore(int x,int y)
            {
                if(BoardArray[x][y].TeamColour == enemy)
                switch (BoardArray[x][y].PieceType)
                {
                    
                    case "King":
                        {
                            status = "Check";
                        }
                        break;
                }
            }
            int direction;//-1 up/left, 1 down/right;
            
            for (int u = 0; u < 2; u++)
            {
                if(Play_as=="None")
                {
                    Play_as = "White";
                }
                else
                {
                    Play_as = "Black";
                }
                if (Play_as == "White") { direction = -1; enemy = "Black"; } else { direction = 1; enemy = "White"; }
                {
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {

                            if (BoardArray[x][y].TeamColour == Play_as)
                            {

                                switch (BoardArray[x][y].PieceType)
                                {
                                    case "Pawn":
                                        if (Play_as == "Black")
                                        {
                                            if (y + direction < 8)
                                            {
                                                if (BoardArray[x][y + direction].HasPiece == false)
                                                {
                                                    checkscore(x, y + direction);

                                                }
                                                if (x + 1 < 8)
                                                {
                                                    if (BoardArray[x + 1][y + direction].HasPiece == true)
                                                    {
                                                        checkscore(x + 1, y + direction);

                                                    }
                                                }
                                                if (x - 1 >= 0)
                                                {
                                                    if (BoardArray[x - 1][y + direction].HasPiece == true)
                                                    {
                                                        checkscore(x - 1, y + direction);

                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (y + direction >= 0)
                                            {
                                                if (BoardArray[x][y + direction].HasPiece == false)
                                                {
                                                    checkscore(x, y + direction);
                                                }

                                                if (x + 1 < 8)
                                                {
                                                    if (BoardArray[x + 1][y + direction].HasPiece == true)
                                                    {
                                                        checkscore(x + 1, y + direction);
                                                    }
                                                }
                                                if (x - 1 >= 0)
                                                {
                                                    if (BoardArray[x - 1][y + direction].HasPiece == true)
                                                    {
                                                        checkscore(x - 1, y + direction);
                                                    }
                                                }

                                            }
                                        }
                                        break;

                                    case "Knight":
                                        if (x - 1 >= 0 & y - 2 >= 0)
                                        {
                                            checkscore(x - 1, y - 2);
                                        }
                                        if (x + 1 < 8 & y + 2 < 8)
                                        {
                                            checkscore(x + 1, y + 2);
                                        }
                                        if (x + 1 < 8 & y - 2 >= 0)
                                        {
                                            checkscore(x + 1, y - 2);
                                        }
                                        if (x - 1 >= 0 & y + 2 < 8)
                                        {
                                            checkscore(x - 1, y + 2);
                                        }

                                        if (x - 2 >= 0 & y - 1 >= 0)
                                        {
                                            checkscore(x - 2, y - 1);
                                        }
                                        if (x + 2 < 8 & y + 1 < 8)
                                        {
                                            checkscore(x + 2, y + 1);
                                        }
                                        if (x + 2 < 8 & y - 1 >= 0)
                                        {
                                            checkscore(x + 2, y - 1);
                                        }
                                        if (x - 2 >= 0 & y + 1 < 8)
                                        {
                                            checkscore(x - 2, y + 1);
                                        }
                                        break;

                                    case "Bishop":

                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y + i < 8)
                                            {
                                                if (!BoardArray[x + i][y + i].HasPiece)
                                                {
                                                    checkscore(x + i, y + 1);
                                                }
                                                else if (BoardArray[x + i][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y + i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y + i < 8)
                                            {
                                                if (!BoardArray[x - i][y + i].HasPiece)
                                                {
                                                    checkscore(x - i, y + i);
                                                }
                                                else if (BoardArray[x - i][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y + i);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y - i >= 0)
                                            {
                                                if (!BoardArray[x + i][y - i].HasPiece)
                                                {
                                                    checkscore(x + i, y - i);
                                                }
                                                else if (BoardArray[x + i][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y - i);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y - i >= 0)
                                            {
                                                if (!BoardArray[x - i][y - i].HasPiece)
                                                {
                                                    checkscore(x - i, y - i);
                                                }
                                                else if (BoardArray[x - i][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y - i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        break;

                                    case "Rook":

                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8)
                                            {
                                                if (!BoardArray[x + i][y].HasPiece)
                                                {
                                                    checkscore(x + i, y);
                                                }
                                                else if (BoardArray[x + i][y].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0)
                                            {
                                                if (!BoardArray[x - i][y].HasPiece)
                                                {
                                                    checkscore(x - i, y);
                                                }
                                                else if (BoardArray[x - i][y].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y + i < 8)
                                            {
                                                if (!BoardArray[x][y + i].HasPiece)
                                                {
                                                    checkscore(x, y + i);
                                                }
                                                else if (BoardArray[x][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x, y + i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y - i >= 0)
                                            {
                                                if (!BoardArray[x][y - i].HasPiece)
                                                {
                                                    checkscore(x, y - i);
                                                }
                                                else if (BoardArray[x][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x, y - i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        break;

                                    case "Queen":
                                        // up,down
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8)
                                            {
                                                if (!BoardArray[x + i][y].HasPiece)
                                                {
                                                    checkscore(x + i, y);
                                                }
                                                else if (BoardArray[x + i][y].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0)
                                            {
                                                if (!BoardArray[x - i][y].HasPiece)
                                                {
                                                    checkscore(x - i, y);
                                                }
                                                else if (BoardArray[x - i][y].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y + i < 8)
                                            {
                                                if (!BoardArray[x][y + i].HasPiece)
                                                {
                                                    checkscore(x, y + i);
                                                }
                                                else if (BoardArray[x][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x, y + i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y - i >= 0)
                                            {
                                                if (!BoardArray[x][y - i].HasPiece)
                                                {
                                                    checkscore(x, y - i);
                                                }
                                                else if (BoardArray[x][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x, y - i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        //diagonally
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y + i < 8)
                                            {
                                                if (!BoardArray[x + i][y + i].HasPiece)
                                                {
                                                    checkscore(x + i, y + i);
                                                }
                                                else if (BoardArray[x + i][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y + i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y + i < 8)
                                            {
                                                if (!BoardArray[x - i][y + i].HasPiece)
                                                {
                                                    checkscore(x - i, y + i);
                                                }
                                                else if (BoardArray[x - i][y + i].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y + i);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y - i >= 0)
                                            {
                                                if (!BoardArray[x + i][y - i].HasPiece)
                                                {
                                                    checkscore(x + i, y - i);
                                                }
                                                else if (BoardArray[x + i][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x + i, y - i);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y - i >= 0)
                                            {
                                                if (!BoardArray[x - i][y - i].HasPiece)
                                                {
                                                    checkscore(x - i, y - i);
                                                }
                                                else if (BoardArray[x - i][y - i].TeamColour == enemy)
                                                {
                                                    checkscore(x - i, y - i);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        break;
                                    case "King":
                                        //up/down
                                        if (x + 1 < 8)
                                        {
                                            if (!BoardArray[x + 1][y].HasPiece)
                                            {
                                                checkscore(x + 1, y);
                                            }
                                            else if (BoardArray[x + 1][y].TeamColour == enemy)
                                            {
                                                checkscore(x + 1, y);

                                            }
                                        }
                                        if (x - 1 >= 0)
                                        {
                                            if (!BoardArray[x - 1][y].HasPiece)
                                            {
                                                checkscore(x - 1, y);
                                            }
                                            else if (BoardArray[x - 1][y].TeamColour == enemy)
                                            {
                                                checkscore(x - 1, y);

                                            }

                                        }
                                        if (y + 1 < 8)
                                        {
                                            if (!BoardArray[x][y + 1].HasPiece)
                                            {
                                                checkscore(x, y + 1);
                                            }
                                            else if (BoardArray[x][y + 1].TeamColour == enemy)
                                            {
                                                checkscore(x, y + 1);
                                            }

                                        }
                                        if (y - 1 >= 0)
                                        {
                                            if (!BoardArray[x][y - 1].HasPiece)
                                            {
                                                checkscore(x, y - 1);
                                            }
                                            else if (BoardArray[x][y - 1].TeamColour == enemy)
                                            {
                                                checkscore(x, y - 1);

                                            }

                                        }
                                        //diagonally
                                        if (x - 1 >= 0 & y - 1 >= 0)
                                        {
                                            if (!BoardArray[x - 1][y - 1].HasPiece)
                                            {
                                                checkscore(x - 1, y - 1);
                                            }
                                            else if (BoardArray[x - 1][y - 1].TeamColour == enemy)
                                            {
                                                checkscore(x - 1, y - 1);

                                            }

                                        }
                                        if (x + 1 < 8 & y + 1 < 8)
                                        {
                                            if (!BoardArray[x + 1][y + 1].HasPiece)
                                            {
                                                checkscore(x + 1, y + 1);
                                            }
                                            else if (BoardArray[x + 1][y + 1].TeamColour == enemy)
                                            {
                                                checkscore(x + 1, y + 1);

                                            }

                                        }
                                        if (x - 1 >= 0 & y + 1 < 8)
                                        {
                                            if (!BoardArray[x - 1][y + 1].HasPiece)
                                            {
                                                checkscore(x - 1, y + 1);
                                            }
                                            else if (BoardArray[x - 1][y + 1].TeamColour == enemy)
                                            {
                                                checkscore(x - 1, y + 1);

                                            }

                                        }
                                        if (x + 1 < 8 & y - 1 >= 0)
                                        {
                                            if (!BoardArray[x + 1][y - 1].HasPiece)
                                            {
                                                checkscore(x + 1, y - 1);
                                            }
                                            else if (BoardArray[x + 1][y - 1].TeamColour == enemy)
                                            {
                                                checkscore(x + 1, y - 1);

                                            }

                                        }
                                        break;
                                }

                            }

                        }
                    }
                }
            }

        }
        
        private void MovePiece()
        {
                        
            if(BoardArray[endx][endy].TeamColour != BoardArray[stx][sty].TeamColour)
            {
                if (BoardArray[stx][sty].PieceType == "Pawn" && BoardArray[endx][endy].SpecialMove == 3)
                {
                    ChessPiece Piece = BoardArray[stx][sty];
                    BoardArray[endx][endy] = Piece; //also resets SpecialMove to 0. Find if it would be necessary to reset if move is not taken.
                    BoardArray[stx][sty] = new ChessPiece();
                    BoardArray[endx][sty] = new ChessPiece();
                }
                else
                {
                    ChessPiece Piece = BoardArray[stx][sty];
                    BoardArray[endx][endy] = Piece;
                    BoardArray[stx][sty] = new ChessPiece();
                }
            }
            else
            {
                ChessPiece Piece = BoardArray[stx][sty];
                BoardArray[stx][sty] = BoardArray[endx][endy];
                BoardArray[endx][endy] = Piece;
            }
        }

        private async void Piece_Click(object sender, RoutedEventArgs e)
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
                            BoardArray[i][j].EnablePlayer(Turn, "");
                        }
                    }
                    BoardArray[posx][posy].EnablePlayer(Turn, showmove: true);
                    switch (PieceType)
                    {
                        case "Pawn":
                            {
                                if (Turn == "White")
                                {
                                    if (posy + direction >= 0)
                                    {
                                        BoardArray[posx][posy + direction].EnablePlayer("None", showmove: true);
                                        if (!BoardArray[posx][posy + direction].HasPiece)
                                            if (SpecialMove == 1 && posy + direction + direction >= 0)
                                            {
                                                BoardArray[posx][posy + direction + direction].EnablePlayer("None", showmove: true);
                                            }
                                        if (posx + 1 < 8)
                                        {
                                            BoardArray[posx + 1][posy + direction].EnablePlayer(enemy, Secondary: "", showmove: true);
                                            if (BoardArray[posx + 1][posy].SpecialMove == 2 && BoardArray[posx + 1][posy].TeamColour == enemy)
                                            {
                                                BoardArray[posx + 1][posy + direction].EnablePlayer("None", showmove: true);
                                                BoardArray[posx + 1][posy + direction].SpecialMove = 3;
                                            }
                                        }
                                        if (posx - 1 >= 0)
                                        {
                                            BoardArray[posx - 1][posy + direction].EnablePlayer(enemy, Secondary: "", showmove: true);
                                            if (BoardArray[posx - 1][posy].SpecialMove == 2 && BoardArray[posx - 1][posy].TeamColour == enemy)
                                            {
                                                BoardArray[posx - 1][posy + direction].EnablePlayer("None", showmove: true);
                                                BoardArray[posx - 1][posy + direction].SpecialMove = 3;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (posy + direction < 8)
                                    {
                                        BoardArray[posx][posy + direction].EnablePlayer("None", showmove: true);
                                        if (!BoardArray[posx][posy + direction].HasPiece)
                                            if (SpecialMove == 1 && posy + direction + direction < 8)
                                            {
                                                BoardArray[posx][posy + direction + direction].EnablePlayer("None", showmove: true);
                                            }
                                        if (posx + 1 < 8)
                                        {
                                            BoardArray[posx + 1][posy + direction].EnablePlayer(enemy, Secondary: "", showmove: true);
                                            if (BoardArray[posx + 1][posy].SpecialMove == 2 && BoardArray[posx + 1][posy].TeamColour == enemy)
                                            {
                                                BoardArray[posx + 1][posy + direction].EnablePlayer("None", showmove: true);
                                                BoardArray[posx + 1][posy + direction].SpecialMove = 3;
                                            }
                                        }
                                        if (posx - 1 >= 0)
                                        {
                                            BoardArray[posx - 1][posy + direction].EnablePlayer(enemy, Secondary: "", showmove: true);
                                            if (BoardArray[posx - 1][posy].SpecialMove == 2 && BoardArray[posx - 1][posy].TeamColour == enemy)
                                            {
                                                BoardArray[posx - 1][posy + direction].EnablePlayer("None", showmove: true);
                                                BoardArray[posx - 1][posy + direction].SpecialMove = 3;
                                            }
                                        }
                                    }
                                }
                                break;
                            }
                        case "Knight":
                            {
                                if (posx - 1 >= 0 & posy - 2 >= 0)
                                {
                                    BoardArray[posx - 1][posy - 2].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx + 1 < 8 & posy + 2 < 8)
                                {
                                    BoardArray[posx + 1][posy + 2].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx + 1 < 8 & posy - 2 >= 0)
                                {
                                    BoardArray[posx + 1][posy - 2].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx - 1 >= 0 & posy + 2 < 8)
                                {
                                    BoardArray[posx - 1][posy + 2].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }

                                if (posx - 2 >= 0 & posy - 1 >= 0)
                                {
                                    BoardArray[posx - 2][posy - 1].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx + 2 < 8 & posy + 1 < 8)
                                {
                                    BoardArray[posx + 2][posy + 1].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx + 2 < 8 & posy - 1 >= 0)
                                {
                                    BoardArray[posx + 2][posy - 1].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                if (posx - 2 >= 0 & posy + 1 < 8)
                                {
                                    BoardArray[posx - 2][posy + 1].EnablePlayer(enemy, Enemy: enemy, showmove: true);
                                }
                                break;
                            }
                        case "Bishop":
                            {
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx + i < 8 & posy + i < 8)
                                    {
                                        if (!BoardArray[posx + i][posy + i].HasPiece)
                                        {
                                            BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                break;
                            }
                        case "Rook":
                            {
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx + i < 8)
                                    {
                                        if (!BoardArray[posx + i][posy].HasPiece)
                                        {
                                            BoardArray[posx + i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx - i >= 0)
                                    {
                                        if (!BoardArray[posx - i][posy].HasPiece)
                                        {
                                            BoardArray[posx - i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posy + i < 8)
                                    {
                                        if (!BoardArray[posx][posy + i].HasPiece)
                                        {
                                            BoardArray[posx][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posy - i >= 0)
                                    {
                                        if (!BoardArray[posx][posy - i].HasPiece)
                                        {
                                            BoardArray[posx][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                break;
                            }
                        case "Queen":
                            {
                                // up,down
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx + i < 8)
                                    {
                                        if (!BoardArray[posx + i][posy].HasPiece)
                                        {
                                            BoardArray[posx + i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx - i >= 0)
                                    {
                                        if (!BoardArray[posx - i][posy].HasPiece)
                                        {
                                            BoardArray[posx - i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posy + i < 8)
                                    {
                                        if (!BoardArray[posx][posy + i].HasPiece)
                                        {
                                            BoardArray[posx][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posy - i >= 0)
                                    {
                                        if (!BoardArray[posx][posy - i].HasPiece)
                                        {
                                            BoardArray[posx][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                //diagonally
                                for (int i = 1; i < 8; i++)
                                {

                                    if (posx + i < 8 & posy + i < 8)
                                    {
                                        if (!BoardArray[posx + i][posy + i].HasPiece)
                                        {
                                            BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy + i].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy + i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx + i][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx + i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
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
                                            BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                        }
                                        else if (BoardArray[posx - i][posy - i].TeamColour == enemy)
                                        {
                                            BoardArray[posx - i][posy - i].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                            break;
                                        }
                                        else break;
                                    }
                                }
                                break;
                            }
                        case "King":
                            {
                                //up/down
                                if (posx + 1 < 8)
                                {
                                    if (!BoardArray[posx + 1][posy].HasPiece)
                                    {
                                        BoardArray[posx + 1][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx + 1][posy].TeamColour == enemy)
                                    {
                                        BoardArray[posx + 1][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }
                                }
                                if (posx - 1 >= 0)
                                {
                                    if (!BoardArray[posx - 1][posy].HasPiece)
                                    {
                                        BoardArray[posx - 1][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx - 1][posy].TeamColour == enemy)
                                    {
                                        BoardArray[posx - 1][posy].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                if (posy + 1 < 8)
                                {
                                    if (!BoardArray[posx][posy + 1].HasPiece)
                                    {
                                        BoardArray[posx][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx][posy + 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                if (posy - 1 >= 0)
                                {
                                    if (!BoardArray[posx][posy - 1].HasPiece)
                                    {
                                        BoardArray[posx][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx][posy - 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                //diagonally
                                if (posx - 1 >= 0 & posy - 1 >= 0)
                                {
                                    if (!BoardArray[posx - 1][posy - 1].HasPiece)
                                    {
                                        BoardArray[posx - 1][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx - 1][posy - 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx - 1][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                if (posx + 1 < 8 & posy + 1 < 8)
                                {
                                    if (!BoardArray[posx + 1][posy + 1].HasPiece)
                                    {
                                        BoardArray[posx + 1][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx + 1][posy + 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx + 1][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                if (posx - 1 >= 0 & posy + 1 < 8)
                                {
                                    if (!BoardArray[posx - 1][posy + 1].HasPiece)
                                    {
                                        BoardArray[posx - 1][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx - 1][posy + 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx - 1][posy + 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                if (posx + 1 < 8 & posy - 1 >= 0)
                                {
                                    if (!BoardArray[posx + 1][posy - 1].HasPiece)
                                    {
                                        BoardArray[posx + 1][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);
                                    }
                                    else if (BoardArray[posx + 1][posy - 1].TeamColour == enemy)
                                    {
                                        BoardArray[posx + 1][posy - 1].EnablePlayer(Turn, Enemy: enemy, showmove: true);

                                    }

                                }
                                break;
                            }
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
                if (BoardArray[stx][sty].PieceType == "Pawn")
                {
                    if (Math.Abs(sty - endy) == 2)
                        BoardArray[stx][sty].SpecialMove = 2;
                    else
                        BoardArray[stx][sty].SpecialMove = 0;

                }
                MovePiece();
                if (Turn == "White") { Turn = "Black"; } else { Turn = "White"; }
                
                await RefreshTable();

                if (Ai_Enabled == true && Turn == "Black")
                {

                    await Task.Run(() => Minmaxroot("Black", Ai_Level));
                    //Debug.WriteLine(BMove[0].ToString() + BMove[1].ToString() + BMove[2].ToString() + BMove[3].ToString());
                    stx = BMove[0];
                    sty = BMove[1];
                    endx = BMove[2];
                    endy = BMove[3];

                    if (BoardArray[endx][sty].PieceType == "Pawn" && BoardArray[endx][sty].TeamColour == "White") { BoardArray[endx][endy].SpecialMove = 3; }
                    MovePiece();
                    if (Turn == "White") { Turn = "Black"; } else { Turn = "White"; }
                    await RefreshTable();
                }
                

            }
            
        }
        
    }
}
