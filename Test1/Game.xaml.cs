using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string Update = "";
        public static bool Selected = false;
        public static bool Check = false;
        public static bool Ai_Enabled = false;
        public static int Ai_Level = 3;
        public static List<int> BMove;




        protected override void OnNavigatedTo(NavigationEventArgs e)

        {

            Ai_Enabled = (bool)e.Parameter;

        }
        //events for game over
        //special moves

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
                        x.SetLocation(new Thickness(80 * i, 80 * j, 0, 0), i, j);
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
                        BoardArray[i][j].EnablePlayer(Turn, "");
                    }
                }
            }


        }

        private static List<List<ChessPiece>> virtual_MovePiece(List<List<ChessPiece>> virtual_BoardArrayx, List<int> Moveit)
        {

            int astx, asty, aendx, aendy;
            astx = Moveit[0];
            asty = Moveit[1];
            aendx = Moveit[2];
            aendy = Moveit[3];

            /*
            List<List<ChessPiece>> virtual_BoardArrayx = new List<List<ChessPiece>>();
            for (int i = 0; i < 8; i++)
            {
                virtual_BoardArrayx.Add(new List<ChessPiece>());
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece x = Copy_BoardArray[i][j];
                    virtual_BoardArrayx[i].Add(x);
                }
            }
            */


            if (virtual_BoardArrayx[aendx][aendy].TeamColour != virtual_BoardArrayx[astx][asty].TeamColour)
            {
                ChessPiece Piece = virtual_BoardArrayx[astx][asty];
                virtual_BoardArrayx[aendx][aendy] = Piece;
                virtual_BoardArrayx[astx][asty] = new ChessPiece();
            }
            else
            {
                ChessPiece Piece = virtual_BoardArrayx[astx][asty];
                virtual_BoardArrayx[astx][asty] = virtual_BoardArrayx[aendx][aendy];
                virtual_BoardArrayx[aendx][aendy] = Piece;
            }
            return virtual_BoardArrayx;
        }

        private static void undo(List<List<ChessPiece>> virtual_BoardArrayx, ChessPiece Piece, ChessPiece Piece2, List<int> move)
        {
            
            
            int astx, asty, aendx, aendy;
            astx = move[0];
            asty = move[1];
            aendx = move[2];
            aendy = move[3];


            virtual_BoardArrayx[astx][asty] = Piece2;
            virtual_BoardArrayx[aendx][aendy] = Piece;
            
        }


        public static int minmaxroot(List<List<ChessPiece>> Ai_BoardArray, string Play_as, int depth)
        {
            void checkscore(int x,int y)
            {

            }
            List<int> Move = new List<int> { 0, 0, 0, 0 };
            List<List<int>> MoveList = new List<List<int>>();
            int maxvalue = -999;
            string enemy = "";
            if (Play_as == "White") { enemy = "Black"; } else { enemy = "White"; }
            
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (Ai_BoardArray[x][y].TeamColour == Play_as)
                    {
                        switch (Ai_BoardArray[x][y].PieceType)
                        {
                            case "Pawn":
                                {
                                    if (Play_as == "Black")
                                    {


                                        if (y + 1 < 8)
                                        {
                                            Move = new List<int> { x, y, x, y + 1 };
                                            if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            if (x + 1 < 8)
                                            {
                                                Move = new List<int> { x, y, x + 1, y + 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }

                                                }
                                            }
                                            if (x - 1 >= 0)
                                            {
                                                Move = new List<int> { x, y, x - 1, y + 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    else //as white
                                    {

                                        if (y - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x, y - 1 };
                                            if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                            {
                                                MoveList.Add(Move);
                                            }

                                            if (x + 1 < 8)
                                            {
                                                Move = new List<int> { x, y, x + 1, y - 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
                                            }
                                            if (x - 1 >= 0)
                                            {
                                                Move = new List<int> { x, y, x - 1, y - 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    break;
                                }

                            case "Knight":
                                {
                                    if (x - 1 >= 0 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 1, y - 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }



                                    if (x + 1 < 8 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 1, y + 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }
                                    if (x + 1 < 8 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 1, y - 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }
                                    if (x - 1 >= 0 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 1, y + 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }



                                    if (x - 2 >= 0 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 2, y - 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }


                                    if (x + 2 < 8 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 2, y + 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }

                                    if (x + 2 < 8 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 2, y - 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }

                                    if (x - 2 >= 0 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 2, y + 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }


                                    }
                                    break;
                                }

                            case "Bishop":
                                {
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8 & y + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y + i].HasPiece)
                                            {
                                                checkscore(x + i, y + 1);
                                            }
                                            else if (Ai_BoardArray[x + i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y + i].HasPiece)
                                            {
                                                checkscore(x - i, y + i);
                                            }
                                            else if (Ai_BoardArray[x - i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y - i].HasPiece)
                                            {
                                                checkscore(x + i, y - i);
                                            }
                                            else if (Ai_BoardArray[x + i][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y - i].HasPiece)
                                            {
                                                checkscore(x - i, y - i);
                                            }
                                            else if (Ai_BoardArray[x - i][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x - i, y - i);
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

                                        if (x + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y].HasPiece)
                                            {
                                                checkscore(x + i, y);
                                            }
                                            else if (Ai_BoardArray[x + i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y].HasPiece)
                                            {
                                                checkscore(x - i, y);
                                            }
                                            else if (Ai_BoardArray[x - i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y + i].HasPiece)
                                            {
                                                checkscore(x, y + i);
                                            }
                                            else if (Ai_BoardArray[x][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y - i].HasPiece)
                                            {
                                                checkscore(x, y - i);
                                            }
                                            else if (Ai_BoardArray[x][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x, y - i);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    break;
                                }
                            case "Queen":
                                { // up,down
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y].HasPiece)
                                            {
                                                checkscore(x + i, y);
                                            }
                                            else if (Ai_BoardArray[x + i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y].HasPiece)
                                            {
                                                checkscore(x - i, y);
                                            }
                                            else if (Ai_BoardArray[x - i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y + i].HasPiece)
                                            {
                                                checkscore(x, y + i);
                                            }
                                            else if (Ai_BoardArray[x][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y - i].HasPiece)
                                            {
                                                checkscore(x, y - i);
                                            }
                                            else if (Ai_BoardArray[x][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y + i].HasPiece)
                                            {
                                                checkscore(x + i, y + i);
                                            }
                                            else if (Ai_BoardArray[x + i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y + i].HasPiece)
                                            {
                                                checkscore(x - i, y + i);
                                            }
                                            else if (Ai_BoardArray[x - i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y - i].HasPiece)
                                            {
                                                checkscore(x + i, y - i);
                                            }
                                            else if (Ai_BoardArray[x + i][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y - i].HasPiece)
                                            {
                                                checkscore(x - i, y - i);
                                            }
                                            else if (Ai_BoardArray[x - i][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x - i, y - i);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    break;
                                }
                            case "King":
                                {//up/down
                                    if (x + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x + 1][y].HasPiece)
                                        {
                                            checkscore(x + 1, y);
                                        }
                                        else if (Ai_BoardArray[x + 1][y].TeamColour == enemy)
                                        {
                                            checkscore(x + 1, y);

                                        }
                                    }
                                    if (x - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x - 1][y].HasPiece)
                                        {
                                            checkscore(x - 1, y);
                                        }
                                        else if (Ai_BoardArray[x - 1][y].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y);

                                        }

                                    }
                                    if (y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x][y + 1].HasPiece)
                                        {
                                            checkscore(x, y + 1);
                                        }
                                        else if (Ai_BoardArray[x][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x, y + 1);
                                        }

                                    }
                                    if (y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x][y - 1].HasPiece)
                                        {
                                            checkscore(x, y - 1);
                                        }
                                        else if (Ai_BoardArray[x][y - 1].TeamColour == enemy)
                                        {
                                            checkscore(x, y - 1);

                                        }

                                    }
                                    //diagonally
                                    if (x - 1 >= 0 & y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x - 1][y - 1].HasPiece)
                                        {
                                            checkscore(x - 1, y - 1);
                                        }
                                        else if (Ai_BoardArray[x - 1][y - 1].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y - 1);

                                        }

                                    }
                                    if (x + 1 < 8 & y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x + 1][y + 1].HasPiece)
                                        {
                                            checkscore(x + 1, y + 1);
                                        }
                                        else if (Ai_BoardArray[x + 1][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x + 1, y + 1);

                                        }

                                    }
                                    if (x - 1 >= 0 & y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x - 1][y + 1].HasPiece)
                                        {
                                            checkscore(x - 1, y + 1);
                                        }
                                        else if (Ai_BoardArray[x - 1][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y + 1);

                                        }

                                    }
                                    if (x + 1 < 8 & y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x + 1][y - 1].HasPiece)
                                        {
                                            checkscore(x + 1, y - 1);
                                        }
                                        else if (Ai_BoardArray[x + 1][y - 1].TeamColour == enemy)
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

            for (int i = 0; i < MoveList.Count; i++)
            {
                Move = MoveList[i];
                ChessPiece bkup = new ChessPiece();
                bkup = Ai_BoardArray[Move[2]][Move[3]];
                ChessPiece bkup1 = new ChessPiece();
                bkup1 = Ai_BoardArray[Move[0]][Move[1]];

                var value =  Minmax(virtual_MovePiece(Ai_BoardArray, MoveList[i]), enemy, depth - 1);
                undo(Ai_BoardArray, bkup, bkup1, Move);
                if (value >= maxvalue)
                {
                    maxvalue = value;
                    BMove = MoveList[i];
                }


            }
            return 0;
        }



        public static int Minmax(List<List<ChessPiece>> Ai_BoardArray, string Play_as, int depth)
        {
            
            
            //maybe reduce arraycopy to only necessary info 
            
            
            List<int> Move = new List<int> { 0, 0, 0, 0 };
            List<List<int>> MoveList = new List<List<int>>();


            void checkscore(int x, int y)
            {
                /*
                int Score = 0;
                if (Ai_BoardArray[x][y].TeamColour != Play_as)
                {
                    switch (Ai_BoardArray[x][y].PieceType)
                    {
                        case "None":
                            Score = 0;
                            break;
                        case "Pawn":
                            Score = 10;
                            break;
                        case "Knight":
                            Score = 20;
                            break;
                        case "Bishop":
                            Score = 30;
                            break;
                        case "Rook":
                            Score = 40;
                            break;
                        case "Queen":
                            Score = 50;
                            break;
                        case "King":
                            Score = 100;
                            break;
                    }

                }

                else
                {
                    Score = -100;

                }

                return Score;
                */
            }
            int checktotalscore()
            {
                int Score = 0;
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        if (Ai_BoardArray[x][y].TeamColour == "Black")
                        {
                            switch (Ai_BoardArray[x][y].PieceType)
                            {
                                case "None":
                                    //
                                    break;
                                case "Pawn":
                                    Score = Score + 10;
                                    break;
                                case "Knight":
                                    Score = Score + 20;
                                    break;
                                case "Bishop":
                                    Score = Score + 30;
                                    break;
                                case "Rook":
                                    Score = Score + 40;
                                    break;
                                case "Queen":
                                    Score = Score + 50;
                                    break;
                                case "King":
                                    Score = Score + 100;
                                    break;
                            }

                        }
                        else switch (Ai_BoardArray[x][y].PieceType)
                            {
                                case "None":
                                    //
                                    break;
                                case "Pawn":
                                    Score = Score - 10;
                                    break;
                                case "Knight":
                                    Score = Score - 20;
                                    break;
                                case "Bishop":
                                    Score = Score - 30;
                                    break;
                                case "Rook":
                                    Score = Score - 40;
                                    break;
                                case "Queen":
                                    Score = Score - 50;
                                    break;
                                case "King":
                                    Score = Score - 100;
                                    break;
                            }

                    }
                }
                return Score;
            }

            if (depth == 0)
            {
                return checktotalscore();
            }

            string enemy = "";
            int maxvalue = 0;
            
            if (Play_as == "White") { enemy = "Black"; maxvalue = 999; } else { enemy = "White"; maxvalue = -999; }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {

                    if (Ai_BoardArray[x][y].TeamColour == Play_as)
                    {
                        switch (Ai_BoardArray[x][y].PieceType)
                        {
                            case "Pawn":
                                {
                                    if (Play_as == "Black")
                                    {


                                        if (y + 1 < 8)
                                        {
                                            Move = new List<int> { x, y, x, y + 1 };
                                            if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            if (x + 1 < 8)
                                            {
                                                Move = new List<int> { x, y, x + 1, y + 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }

                                                }
                                            }
                                            if (x - 1 >= 0)
                                            {
                                                Move = new List<int> { x, y, x - 1, y + 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }

                                                }
                                            }
                                        }

                                    }
                                    else //as white
                                    {

                                        if (y - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x, y - 1 };
                                            if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                            {
                                                MoveList.Add(Move);
                                            }

                                            if (x + 1 < 8)
                                            {
                                                Move = new List<int> { x, y, x + 1, y - 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
                                            }
                                            if (x - 1 >= 0)
                                            {
                                                Move = new List<int> { x, y, x - 1, y - 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                {
                                                    if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
                                            }

                                        }

                                    }
                                    break;
                                }

                            case "Knight":
                                {
                                    if (x - 1 >= 0 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 1, y - 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }



                                    if (x + 1 < 8 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 1, y + 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }
                                    if (x + 1 < 8 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 1, y - 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }
                                    if (x - 1 >= 0 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 1, y + 2 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }



                                    if (x - 2 >= 0 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 2, y - 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }
                                    }


                                    if (x + 2 < 8 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 2, y + 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }

                                    if (x + 2 < 8 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 2, y - 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }

                                    }

                                    if (x - 2 >= 0 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 2, y + 1 };

                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                        {
                                            MoveList.Add(Move);
                                        }


                                    }
                                    break;
                                }

                            case "Bishop":
                                {
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8 & y + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y + i].HasPiece)
                                            {
                                                checkscore(x + i, y + 1);
                                            }
                                            else if (Ai_BoardArray[x + i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y + i].HasPiece)
                                            {
                                                checkscore(x - i, y + i);
                                            }
                                            else if (Ai_BoardArray[x - i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y - i].HasPiece)
                                            {
                                                checkscore(x + i, y - i);
                                            }
                                            else if (Ai_BoardArray[x + i][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y - i].HasPiece)
                                            {
                                                checkscore(x - i, y - i);
                                            }
                                            else if (Ai_BoardArray[x - i][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x - i, y - i);
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

                                        if (x + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y].HasPiece)
                                            {
                                                checkscore(x + i, y);
                                            }
                                            else if (Ai_BoardArray[x + i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y].HasPiece)
                                            {
                                                checkscore(x - i, y);
                                            }
                                            else if (Ai_BoardArray[x - i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y + i].HasPiece)
                                            {
                                                checkscore(x, y + i);
                                            }
                                            else if (Ai_BoardArray[x][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y - i].HasPiece)
                                            {
                                                checkscore(x, y - i);
                                            }
                                            else if (Ai_BoardArray[x][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x, y - i);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    break;
                                }
                            case "Queen":
                                { // up,down
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8)
                                        {
                                            if (!Ai_BoardArray[x + i][y].HasPiece)
                                            {
                                                checkscore(x + i, y);
                                            }
                                            else if (Ai_BoardArray[x + i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y].HasPiece)
                                            {
                                                checkscore(x - i, y);
                                            }
                                            else if (Ai_BoardArray[x - i][y].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y + i].HasPiece)
                                            {
                                                checkscore(x, y + i);
                                            }
                                            else if (Ai_BoardArray[x][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x][y - i].HasPiece)
                                            {
                                                checkscore(x, y - i);
                                            }
                                            else if (Ai_BoardArray[x][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y + i].HasPiece)
                                            {
                                                checkscore(x + i, y + i);
                                            }
                                            else if (Ai_BoardArray[x + i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y + i].HasPiece)
                                            {
                                                checkscore(x - i, y + i);
                                            }
                                            else if (Ai_BoardArray[x - i][y + i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x + i][y - i].HasPiece)
                                            {
                                                checkscore(x + i, y - i);
                                            }
                                            else if (Ai_BoardArray[x + i][y - i].TeamColour == enemy)
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
                                            if (!Ai_BoardArray[x - i][y - i].HasPiece)
                                            {
                                                checkscore(x - i, y - i);
                                            }
                                            else if (Ai_BoardArray[x - i][y - i].TeamColour == enemy)
                                            {
                                                checkscore(x - i, y - i);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    break;
                                }
                            case "King":
                                {//up/down
                                    if (x + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x + 1][y].HasPiece)
                                        {
                                            checkscore(x + 1, y);
                                        }
                                        else if (Ai_BoardArray[x + 1][y].TeamColour == enemy)
                                        {
                                            checkscore(x + 1, y);

                                        }
                                    }
                                    if (x - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x - 1][y].HasPiece)
                                        {
                                            checkscore(x - 1, y);
                                        }
                                        else if (Ai_BoardArray[x - 1][y].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y);

                                        }

                                    }
                                    if (y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x][y + 1].HasPiece)
                                        {
                                            checkscore(x, y + 1);
                                        }
                                        else if (Ai_BoardArray[x][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x, y + 1);
                                        }

                                    }
                                    if (y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x][y - 1].HasPiece)
                                        {
                                            checkscore(x, y - 1);
                                        }
                                        else if (Ai_BoardArray[x][y - 1].TeamColour == enemy)
                                        {
                                            checkscore(x, y - 1);

                                        }

                                    }
                                    //diagonally
                                    if (x - 1 >= 0 & y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x - 1][y - 1].HasPiece)
                                        {
                                            checkscore(x - 1, y - 1);
                                        }
                                        else if (Ai_BoardArray[x - 1][y - 1].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y - 1);

                                        }

                                    }
                                    if (x + 1 < 8 & y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x + 1][y + 1].HasPiece)
                                        {
                                            checkscore(x + 1, y + 1);
                                        }
                                        else if (Ai_BoardArray[x + 1][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x + 1, y + 1);

                                        }

                                    }
                                    if (x - 1 >= 0 & y + 1 < 8)
                                    {
                                        if (!Ai_BoardArray[x - 1][y + 1].HasPiece)
                                        {
                                            checkscore(x - 1, y + 1);
                                        }
                                        else if (Ai_BoardArray[x - 1][y + 1].TeamColour == enemy)
                                        {
                                            checkscore(x - 1, y + 1);

                                        }

                                    }
                                    if (x + 1 < 8 & y - 1 >= 0)
                                    {
                                        if (!Ai_BoardArray[x + 1][y - 1].HasPiece)
                                        {
                                            checkscore(x + 1, y - 1);
                                        }
                                        else if (Ai_BoardArray[x + 1][y - 1].TeamColour == enemy)
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

            for( int i = 0;i<MoveList.Count;i++)
            {
                Move = MoveList[i];
                ChessPiece bkup = new ChessPiece();
                bkup = Ai_BoardArray[Move[2]][Move[3]];
                ChessPiece bkup1 = new ChessPiece();
                bkup1 = Ai_BoardArray[Move[0]][Move[1]];

                var value = Minmax(virtual_MovePiece(Ai_BoardArray, MoveList[i]), enemy, depth - 1);
                undo(Ai_BoardArray, bkup, bkup1, Move);
                if (Play_as == "Black")
                {
                    if (value >= maxvalue)
                    {
                        maxvalue = value;

                    }
                }
                else
                {
                    if (value <= maxvalue)
                    {
                        maxvalue = value;

                    }
                }
                
            }
            return maxvalue;
        }
    }

}
