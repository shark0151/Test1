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
        public static List<string> PieceTypes = new List<string>(new string[] { "Pawn", "Knight", "Bishop", "Rook", "Queen", "King", "None" }); //0-p,1-k,2-b,3-r,4-q,5-k,6-na
        public static List<List<ChessPiece>> BoardArray = new List<List<ChessPiece>>();
        public static Grid PiecesGrid = new Grid();
        public static int stx = 0, sty = 0;
        public static int endx = 0, endy = 0;
        public static string Turn = "White";
        public static bool Selected = false;
       
        public Game()
        {
            this.InitializeComponent();
            InitTable();
            SetTableAI();
            SetTablePlayer();
            EnablePlayer();
            
            void InitTable()
            {
                Chessboard.Children.Add(PiecesGrid);
                for (int i = 0; i < 8; i++)
                {
                    BoardArray.Add(new List<ChessPiece>());
                    for (int j = 0; j < 8; j++)
                    {
                        ChessPiece x = new ChessPiece();
                        x.SetLocation(new Thickness(80 * i, 80 * j, 0, 0),i,j);
                        BoardArray[i].Add(x);
                        BoardArray[i][j].SetPieceType(PieceTypes[6]);
                        PiecesGrid.Children.Add(BoardArray[i][j].GetSquare());
                    }
                }
            }
 
            void SetTableAI()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        
                        if (j == 0)
                        {
                            BoardArray[i][j].SetPieceTeam("Black");                            
                            if (i == 0 || i == 7) { BoardArray[i][j].SetPieceType(PieceTypes[3]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_rook.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 1 || i == 6) { BoardArray[i][j].SetPieceType(PieceTypes[1]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_knight.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 2 || i == 5) { BoardArray[i][j].SetPieceType(PieceTypes[2]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_bishop.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 3) { BoardArray[i][j].SetPieceType(PieceTypes[4]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_queen.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 4) { BoardArray[i][j].SetPieceType(PieceTypes[5]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_king.png"))); BoardArray[i][j].SetHasPiece(true); }
                           
                        }
                        else if (j == 1)
                        {
                            BoardArray[i][j].SetPieceType(PieceTypes[0]);
                            BoardArray[i][j].SetPieceTeam("Black");
                            BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_pawn.png")));
                            BoardArray[i][j].SetHasPiece(true);
                        }

                    }
                }
            }

            void SetTablePlayer()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 6; j < 8; j++)
                    {

                        if (j == 7)
                        {
                            BoardArray[i][j].SetPieceTeam("White");
                            if (i == 0 || i == 7) { BoardArray[i][j].SetPieceType(PieceTypes[3]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_rook.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 1 || i == 6) { BoardArray[i][j].SetPieceType(PieceTypes[1]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_knight.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 2 || i == 5) { BoardArray[i][j].SetPieceType(PieceTypes[2]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_bishop.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 3) { BoardArray[i][j].SetPieceType(PieceTypes[4]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_queen.png"))); BoardArray[i][j].SetHasPiece(true); }
                            else if (i == 4) { BoardArray[i][j].SetPieceType(PieceTypes[5]); BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_king.png"))); BoardArray[i][j].SetHasPiece(true); }

                        }
                        else if (j == 6)
                        {
                            BoardArray[i][j].SetPieceType(PieceTypes[0]);
                            BoardArray[i][j].SetPieceTeam("White");
                            BoardArray[i][j].SetImage(new BitmapImage(new Uri("ms-appx:///Assets/pieces/w_pawn.png")));
                            BoardArray[i][j].SetHasPiece(true);
                        }

                    }
                }
            }

            void EnablePlayer()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                            BoardArray[i][j].EnablePlayer(Turn,"");
                    }
                }
            }

        }
        
    }

}
