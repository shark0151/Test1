using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
        public static int Ai_Aggression = 0;
        public static List<int> BMove;

        private class SChesspiece
        {
            public string TeamColour = "None";
            public string PieceType = "None";
            public bool HasPiece = false;
            public int SpecialMove = 0;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)

        {

            Ai_Enabled = (bool)e.Parameter;
            checkBox.IsChecked = (bool)e.Parameter;

        }

        //game over and stalemate events
        //special moves
        //promotions
        //settings and buttons
        //highlight ai move?
        //force ai to move forward/avoid repeating move

        

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
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            Ai_Enabled = true;
        }
        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Ai_Enabled = false;
        }

        private static List<List<SChesspiece>> virtual_MovePiece(List<List<SChesspiece>> virtual_BoardArrayx, List<int> Moveit)
        {

            int astx, asty, aendx, aendy;
            astx = Moveit[0];
            asty = Moveit[1];
            aendx = Moveit[2];
            aendy = Moveit[3];

            /*
            List<List<ChessPiece>> virtual_BoardArrayx = new List<List<S>>();
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
                if (virtual_BoardArrayx[astx][asty].PieceType == "Pawn" && virtual_BoardArrayx[aendx][aendy].SpecialMove == 3)
                {
                    SChesspiece Piece = virtual_BoardArrayx[astx][asty];
                    virtual_BoardArrayx[aendx][aendy] = Piece; //also resets SpecialMove to 0
                    virtual_BoardArrayx[astx][asty] = new SChesspiece();
                    virtual_BoardArrayx[aendx][asty] = new SChesspiece();
                }
                if (virtual_BoardArrayx[astx][asty].PieceType == "King" && virtual_BoardArrayx[aendx][aendy].SpecialMove == 5)
                {
                    SChesspiece Piece = new SChesspiece();
                    if (virtual_BoardArrayx[aendx][aendy].PieceType == "Rook" && aendx + 4 ==stx)
                    {
                        Piece = virtual_BoardArrayx[aendx][aendy]; //rook
                        virtual_BoardArrayx[aendx + 3][aendy] = Piece;//rook moves here
                        virtual_BoardArrayx[aendx][aendy] = new SChesspiece();
                        Piece = virtual_BoardArrayx[astx][asty]; //king
                        virtual_BoardArrayx[aendx+2][aendy] = Piece; //king moves here
                        virtual_BoardArrayx[astx][asty] = new SChesspiece();
                    }
                    if (virtual_BoardArrayx[aendx][aendy].PieceType == "Rook" && aendx - 3 == stx)
                    {
                        Piece = virtual_BoardArrayx[aendx][aendy]; //rook
                        virtual_BoardArrayx[aendx-2][aendy] = Piece;//rook moves here
                        virtual_BoardArrayx[aendx][aendy] = new SChesspiece();
                        Piece = virtual_BoardArrayx[astx][asty]; //king
                        virtual_BoardArrayx[aendx - 1][aendy] = Piece; //king moves here
                        virtual_BoardArrayx[astx][asty] = new SChesspiece();
                    }
                    


                }
                else
                {
                    SChesspiece Piece = virtual_BoardArrayx[astx][asty];
                    virtual_BoardArrayx[aendx][aendy] = Piece;
                    virtual_BoardArrayx[astx][asty] = new SChesspiece();
                }
            }
            else
            {
                SChesspiece Piece = virtual_BoardArrayx[astx][asty];
                virtual_BoardArrayx[astx][asty] = virtual_BoardArrayx[aendx][aendy];
                virtual_BoardArrayx[aendx][aendy] = Piece;
            }
            return virtual_BoardArrayx;
        }

        private static void undo(List<List<SChesspiece>> virtual_BoardArrayx, SChesspiece Piece, SChesspiece Piece2, List<int> move)
        {
            
            
            int astx, asty, aendx, aendy;
            astx = move[0];
            asty = move[1];
            aendx = move[2];
            aendy = move[3];


            virtual_BoardArrayx[astx][asty] = Piece2;
            virtual_BoardArrayx[aendx][aendy] = Piece;
            
        }

        public static async Task Minmaxroot(string Play_as, int depth)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                List<List<SChesspiece>> Ai_BoardArray = new List<List<SChesspiece>>();


                for (int i = 0; i < 8; i++)
                {
                    Ai_BoardArray.Add(new List<SChesspiece>());
                    for (int j = 0; j < 8; j++)
                    {
                        SChesspiece x = new SChesspiece();
                        x.HasPiece = BoardArray[i][j].GetHasPiece();
                        x.PieceType = BoardArray[i][j].GetPieceType();
                        x.TeamColour = BoardArray[i][j].GetPieceTeam();
                        x.SpecialMove = BoardArray[i][j].GetSpecial();
                        Ai_BoardArray[i].Add(x);
                    }
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
                                                    if (y + 2 < 8)
                                                    {
                                                        Move = new List<int> { x, y, x, y + 2 };
                                                        if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false && Ai_BoardArray[x][y].SpecialMove == 1)
                                                        {
                                                            MoveList.Add(Move);
                                                        }
                                                    }
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
                                                    else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                    {
                                                        Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                        MoveList.Add(Move);
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
                                                    else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                    {
                                                        Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                        MoveList.Add(Move);
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
                                                    if (y - 2 >= 0)
                                                    {
                                                        Move = new List<int> { x, y, x, y - 2 };
                                                        if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false && Ai_BoardArray[x][y].SpecialMove == 1)
                                                        {
                                                            MoveList.Add(Move);
                                                        }
                                                    }
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
                                                    else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                    {
                                                        Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                        MoveList.Add(Move);
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
                                                    else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                    {
                                                        Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                        MoveList.Add(Move);
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
                                                Move = new List<int> { x, y, x + i, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y + i < 8)
                                            {
                                                Move = new List<int> { x, y, x - i, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x + i, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x - i, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
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
                                                Move = new List<int> { x, y, x + i, y };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x - i, y };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y + i < 8)
                                            {
                                                Move = new List<int> { x, y, x, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
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
                                                Move = new List<int> { x, y, x + i, y };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x - i, y };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y + i < 8)
                                            {
                                                Move = new List<int> { x, y, x, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
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
                                                Move = new List<int> { x, y, x + i, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }
                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y + i < 8)
                                            {
                                                Move = new List<int> { x, y, x - i, y + i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x + i < 8 & y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x + i, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
                                                    break;
                                                }
                                                else break;
                                            }

                                        }
                                        for (int i = 1; i < 8; i++)
                                        {

                                            if (x - i >= 0 & y - i >= 0)
                                            {
                                                Move = new List<int> { x, y, x - i, y - i };

                                                if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                                {
                                                    MoveList.Add(Move);
                                                }
                                                else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                {
                                                    MoveList.Add(Move);
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
                                            Move = new List<int> { x, y, x + 1, y };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }
                                            if (x + 3 < 8)
                                            {
                                                Move = new List<int> { x, y, x + 3, y };
                                                if (!Ai_BoardArray[Move[2]-1][Move[3]].HasPiece && Ai_BoardArray[x][y].SpecialMove == 4 && Ai_BoardArray[Move[2]][Move[3]].SpecialMove == 5)
                                                {
                                                    MoveList.Add(Move);
                                                    
                                                }
                                            }
                                        }
                                        if (x - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x - 1, y };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }
                                            if (x - 4 >= 0)
                                            {
                                                Move = new List<int> { x, y, x - 4, y };
                                                if (!Ai_BoardArray[Move[2]+2][Move[3]].HasPiece && !Ai_BoardArray[Move[2]+1][Move[3]].HasPiece && Ai_BoardArray[x][y].SpecialMove == 4 && Ai_BoardArray[Move[2]][Move[3]].SpecialMove == 5)
                                                {
                                                    MoveList.Add(Move);
                                                    
                                                }
                                            }

                                        }
                                        if (y + 1 < 8)
                                        {
                                            Move = new List<int> { x, y, x, y + 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }

                                        }
                                        if (y - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x, y - 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }

                                        }
                                        //diagonally
                                        if (x - 1 >= 0 & y - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x - 1, y - 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }

                                        }
                                        if (x + 1 < 8 & y + 1 < 8)
                                        {
                                            Move = new List<int> { x, y, x + 1, y + 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }

                                        }
                                        if (x - 1 >= 0 & y + 1 < 8)
                                        {
                                            Move = new List<int> { x, y, x - 1, y + 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

                                            }

                                        }
                                        if (x + 1 < 8 & y - 1 >= 0)
                                        {
                                            Move = new List<int> { x, y, x + 1, y - 1 };
                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);

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
                    SChesspiece bkup = new SChesspiece();
                    bkup = Ai_BoardArray[Move[2]][Move[3]];
                    SChesspiece bkup1 = new SChesspiece();
                    bkup1 = Ai_BoardArray[Move[0]][Move[1]];

                    if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "Pawn")
                    {
                        if (Math.Abs(Move[1] - Move[3]) == 2)
                            Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 2;
                        else
                            Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;

                    }
                    if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "Rook")
                    {
                        Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;
                    }
                    if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "King")
                    {
                        Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;
                    }

                    var value = Minmax(virtual_MovePiece(Ai_BoardArray, MoveList[i]), enemy, depth - 1, 999, -999);
                    undo(Ai_BoardArray, bkup, bkup1, Move);
                    
                    if (value >= maxvalue)
                    {
                        maxvalue = value;
                        BMove = MoveList[i];
                    }


                }

                
            });
        }

        private static int Minmax(List<List<SChesspiece>> Ai_BoardArray, string Play_as, int depth,int Score_W,int Score_B)
        {
            
            List<int> Move = new List<int> { 0, 0, 0, 0 };
            List<List<int>> MoveList = new List<List<int>>();
           
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
                                                if (y + 2 < 8)
                                                {
                                                    Move = new List<int> { x, y, x, y + 2 };
                                                    if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false && Ai_BoardArray[x][y].SpecialMove == 1)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
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
                                                else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                {
                                                    Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                    MoveList.Add(Move);
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
                                                else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                {
                                                    Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                    MoveList.Add(Move);
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
                                                if (y - 2 >= 0)
                                                {
                                                    Move = new List<int> { x, y, x, y - 2 };
                                                    if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false && Ai_BoardArray[x][y].SpecialMove == 1)
                                                    {
                                                        MoveList.Add(Move);
                                                    }
                                                }
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
                                                else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                {
                                                    Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                    MoveList.Add(Move);
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
                                                else if (Ai_BoardArray[Move[2]][y].HasPiece == true && Ai_BoardArray[Move[2]][y].SpecialMove == 2)
                                                {
                                                    Ai_BoardArray[Move[2]][Move[3]].SpecialMove = 3;
                                                    MoveList.Add(Move);
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
                                            Move = new List<int> { x, y, x + i, y + i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0 & y + i < 8)
                                        {
                                            Move = new List<int> { x, y, x - i, y + i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }

                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8 & y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x + i, y - i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }

                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0 & y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x - i, y - i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
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
                                            Move = new List<int> { x, y, x + i, y };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x - i, y };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (y + i < 8)
                                        {
                                            Move = new List<int> { x, y, x, y + i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x, y - i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
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
                                            Move = new List<int> { x, y, x + i, y };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x - i, y };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (y + i < 8)
                                        {
                                            Move = new List<int> { x, y, x, y + i};

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x, y - i};

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
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
                                            Move = new List<int> { x, y, x + i, y + i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }
                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0 & y + i < 8)
                                        {
                                            Move = new List<int> { x, y, x - i, y + i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }

                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x + i < 8 & y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x + i, y - i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
                                                break;
                                            }
                                            else break;
                                        }

                                    }
                                    for (int i = 1; i < 8; i++)
                                    {

                                        if (x - i >= 0 & y - i >= 0)
                                        {
                                            Move = new List<int> { x, y, x - i, y - i };

                                            if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                            {
                                                MoveList.Add(Move);
                                            }
                                            else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                            {
                                                MoveList.Add(Move);
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
                                        Move = new List<int> { x, y, x + 1, y };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }
                                        if (x + 3 < 8)
                                        {
                                            Move = new List<int> { x, y, x + 3, y };
                                            if (!Ai_BoardArray[Move[2] - 1][Move[3]].HasPiece && Ai_BoardArray[x][y].SpecialMove == 4 && Ai_BoardArray[Move[2]][Move[3]].SpecialMove == 5)
                                            {
                                                MoveList.Add(Move);

                                            }
                                        }
                                    }
                                    if (x - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 1, y };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }
                                        if (x - 4 >= 0)
                                        {
                                            Move = new List<int> { x, y, x - 4, y };
                                            if (!Ai_BoardArray[Move[2] + 2][Move[3]].HasPiece && !Ai_BoardArray[Move[2] + 1][Move[3]].HasPiece && Ai_BoardArray[x][y].SpecialMove == 4 && Ai_BoardArray[Move[2]][Move[3]].SpecialMove == 5)
                                            {
                                                MoveList.Add(Move);

                                            }
                                        }

                                    }
                                    if (y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x, y + 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }

                                    }
                                    if (y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x, y - 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }

                                    }
                                    //diagonally
                                    if (x - 1 >= 0 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 1, y - 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }

                                    }
                                    if (x + 1 < 8 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 1, y + 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }

                                    }
                                    if (x - 1 >= 0 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 1, y + 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

                                        }

                                    }
                                    if (x + 1 < 8 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 1, y - 1 };
                                        if (!Ai_BoardArray[Move[2]][Move[3]].HasPiece)
                                        {
                                            MoveList.Add(Move);
                                        }
                                        else if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                        {
                                            MoveList.Add(Move);

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
                SChesspiece bkup = new SChesspiece();
                bkup = Ai_BoardArray[Move[2]][Move[3]];
                SChesspiece bkup1 = new SChesspiece();
                bkup1 = Ai_BoardArray[Move[0]][Move[1]];

                if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "Pawn")
                {
                    if (Math.Abs(Move[1] - Move[3]) == 2)
                        Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 2;
                    else
                        Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;

                }
                if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "Rook")
                {
                    Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;
                }
                if (Ai_BoardArray[Move[0]][Move[1]].PieceType == "King")
                {
                    Ai_BoardArray[Move[0]][Move[1]].SpecialMove = 0;
                }

                var value = Minmax(virtual_MovePiece(Ai_BoardArray, MoveList[i]), enemy, depth - 1, Score_W, Score_B);
                undo(Ai_BoardArray, bkup, bkup1, Move);
                
                if (Play_as == "Black")
                {
                    if (value > maxvalue)
                    {
                        maxvalue = value;

                    }
                    if (maxvalue > Score_B)
                    {
                        Score_B = maxvalue;

                    }
                    if (Score_W <= Score_B)
                    {
                        return maxvalue;

                    }
                }
                else
                {
                    if (value < maxvalue)
                    {
                        maxvalue = value;

                    }
                    if (maxvalue < Score_W)
                    {
                        Score_W = maxvalue;

                    }
                    if (Score_W <= Score_B)
                    {
                        return maxvalue;

                    }
                }
                
            }
            return maxvalue;
        }
    }

}
