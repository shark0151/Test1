using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Test1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Game : Page
    {
        private List<string> PieceTypes = new List<string>(new string[] { "Pawn", "Knight", "Bishop", "Rook", "Queen", "King" }); //0-p,1-k,2-b,3-r,4-q,5-k
        private List<ChessboardSquare> BlackPieces = new List<ChessboardSquare>();
        private List<List<ChessboardSquare>> BoardArray = new List<List<ChessboardSquare>>();
        public Game()
        {
            this.InitializeComponent();
            InitTable();
            SetTable();            

            void InitTable()
            {
                for (int i = 0; i < 8; i++)
                {
                    BoardArray.Add(new List<ChessboardSquare>());
                    for (int j = 0; j < 8; j++)
                    {
                        ChessboardSquare x = new ChessboardSquare();
                        x.SetLocation(new Thickness(80 * i, 80 * j, 0, 0), "" + i + j);
                        BoardArray[i].Add(x);
                        Chessboard.Children.Add(BoardArray[i][j].GetSquare());
                    }
                }
            }
            void SetTable()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        
                        if (j == 0)
                        {
                            BoardArray[i][j].SetPieceTeam(false);                            
                            if (i == 0 || i == 7) { BoardArray[i][j].SetPieceType(PieceTypes[3]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_rook.png"))); BoardArray[i][j].SetPieceActive(true); }
                            else if (i == 1 || i == 6) { BoardArray[i][j].SetPieceType(PieceTypes[2]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_knight.png"))); BoardArray[i][j].SetPieceActive(true); }
                            else if (i == 2 || i == 5) { BoardArray[i][j].SetPieceType(PieceTypes[1]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_bishop.png"))); BoardArray[i][j].SetPieceActive(true); }
                            else if (i == 3) { BoardArray[i][j].SetPieceType(PieceTypes[4]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_queen.png"))); BoardArray[i][j].SetPieceActive(true); }
                            else if (i == 4) { BoardArray[i][j].SetPieceType(PieceTypes[4]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_king.png"))); BoardArray[i][j].SetPieceActive(true); }
                           
                        }
                        else if (j == 1)
                        {
                            BoardArray[i][j].SetPieceType(PieceTypes[0]);
                            BoardArray[i][j].SetPieceTeam(false);
                            BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_pawn.png")));
                            BoardArray[i][j].SetPieceActive(true);
                        }

                        BlackPieces.Add(BoardArray[i][j]);
                    }
                }
            }
            
        }

        public class ChessboardSquare : ChessPiece
        {
            private Grid ChessSquare = new Grid();
            private Button SquareSelect = new Button();
            
            public ChessboardSquare()
            {                
                SquareSelect.Width = 80;
                SquareSelect.Height = 80;
                SquareSelect.HorizontalAlignment = HorizontalAlignment.Center;
                SquareSelect.VerticalAlignment = VerticalAlignment.Center;
                SquareSelect.Margin = new Thickness(0, 0, 0, 0);
                SquareSelect.Content = "";

                ChessSquare.Width = 80;
                ChessSquare.HorizontalAlignment = HorizontalAlignment.Left;
                ChessSquare.VerticalAlignment = VerticalAlignment.Top;
                ChessSquare.Height = 80;
                ChessSquare.Children.Add(GetImage());
                ChessSquare.Children.Add(SquareSelect);
            }
            public void SetLocation(Thickness x, string y)
            {
                ChessSquare.Margin = x;
                SquareSelect.Content = y;
            }
            public Grid GetSquare()
            {
                return ChessSquare;
            }
        }


    }
}
