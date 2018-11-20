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
        public static int Ai_Level = 4;
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

        public static int minmax(List<List<ChessPiece>> Ai_BoardArray, string Play_as, int depth, int Score_W, int Score_B)
        {
            /*
            List<List<ChessPiece>> Copy_BoardArray = new List<List<ChessPiece>>();
            for (int i = 0; i < 8; i++)
            {
                Copy_BoardArray.Add(new List<ChessPiece>());
                for (int j = 0; j < 8; j++)
                {
                    ChessPiece x = Ai_BoardArray[i][j];
                    Copy_BoardArray[i].Add(x);
                }
            }
            */

            void undo(List<List<ChessPiece>> virtual_BoardArrayx, ChessPiece Piece, List<int> move)
            {
                int astx, asty, aendx, aendy;
                astx = move[2];
                asty = move[3];
                aendx = move[0];
                aendy = move[1];


                virtual_BoardArrayx[astx][asty] = virtual_BoardArrayx[aendx][aendy];
                virtual_BoardArrayx[aendx][aendy] = Piece;

            }

            List<int> Move = new List<int> { 0, 0, 0, 0 };
                        
            int checkscore(int x, int y)
            {
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
            }

            List<List<ChessPiece>> virtual_MovePiece(List<List<ChessPiece>> virtual_BoardArrayx, List<int> Moveit)
            {
                
                int astx, asty, aendx, aendy;
                astx = Moveit[0];
                asty = Moveit[1];
                aendx = Moveit[2];
                aendy = Moveit[3];
                
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

            string enemy = "";
            List<List<int>> max;
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
                                        
                                        if (depth > 0)
                                        {
                                            if (y + 1 < 8)
                                            {
                                                Move = new List<int> { x, y, x, y + 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                                {
                                                    
                                                    if (checkscore(Move[2], Move[3]) >= Score_B)
                                                    {
                                                        ChessPiece bkup = new ChessPiece();
                                                        bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                        Score_B = checkscore(Move[2], Move[3]);
                                                        Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                        BMove = Move;
                                                        undo(Ai_BoardArray, bkup, Move);
                                                    }

                                                }
                                                if (x + 1 < 8)
                                                {
                                                    Move = new List<int> { x, y, x + 1, y + 1 };
                                                    if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                    {
                                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                        {

                                                            if (checkscore(Move[2], Move[3]) >= Score_B)
                                                            {
                                                                ChessPiece bkup = new ChessPiece();
                                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                                Score_B = checkscore(Move[2], Move[3]);
                                                                Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                                BMove = Move;
                                                                undo(Ai_BoardArray, bkup, Move);
                                                            }


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

                                                            if (checkscore(Move[2], Move[3]) >= Score_B)
                                                            {
                                                                ChessPiece bkup = new ChessPiece();
                                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                                Score_B = checkscore(Move[2], Move[3]);
                                                                Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                                BMove = Move;
                                                                undo(Ai_BoardArray, bkup, Move);
                                                            }


                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        else return Score_W;
                                    }
                                    else
                                    {
                                        if (depth > 0)
                                        {
                                            if (y - 1 >= 0)
                                            {
                                                Move = new List<int> { x, y, x, y - 1 };
                                                if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == false)
                                                {

                                                    if (checkscore(Move[2], Move[3]) >= Score_W)
                                                    {
                                                        ChessPiece bkup = new ChessPiece();
                                                        bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                        Score_W = checkscore(Move[2], Move[3]);
                                                        Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                        undo(Ai_BoardArray, bkup, Move);
                                                    }

                                                }

                                                if (x + 1 < 8)
                                                {
                                                    Move = new List<int> { x, y, x + 1, y - 1 };
                                                    if (Ai_BoardArray[Move[2]][Move[3]].HasPiece == true)
                                                    {
                                                        if (Ai_BoardArray[Move[2]][Move[3]].TeamColour == enemy)
                                                        {
                                                            if (checkscore(Move[2], Move[3]) >= Score_W)
                                                            {
                                                                ChessPiece bkup = new ChessPiece();
                                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                                Score_W = checkscore(Move[2], Move[3]);
                                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                                undo(Ai_BoardArray, bkup, Move);
                                                            }
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
                                                            if (checkscore(Move[2], Move[3]) >= Score_W)
                                                            {
                                                                ChessPiece bkup = new ChessPiece();
                                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                                Score_W = checkscore(Move[2], Move[3]);
                                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                                undo(Ai_BoardArray, bkup, Move);
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                        else return Score_B;
                                    }
                                    break;
                                }
                                
                            case "Knight":
                                {
                                    if (x - 1 >= 0 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 1, y - 2 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {

                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 1, y - 2");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 1, y - 2");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);

                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }

                                    }
                                    if (x + 1 < 8 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 1, y + 2 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 1, y + 2");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 1, y + 2");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);

                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }
                                    if (x + 1 < 8 & y - 2 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 1, y - 2 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 1, y - 2");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 1, y - 2");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }
                                    if (x - 1 >= 0 & y + 2 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 1, y + 2 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 1, y + 2");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 1, y + 2");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }

                                    if (x - 2 >= 0 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x - 2, y - 1 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 2, y - 1");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x - 2, y - 1");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }
                                    if (x + 2 < 8 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x + 2, y + 1 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 2, y + 1");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 2, y + 1");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }
                                    if (x + 2 < 8 & y - 1 >= 0)
                                    {
                                        Move = new List<int> { x, y, x + 2, y - 1 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 2, y - 1");
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + "x + 2, y - 1");
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
                                        }
                                    }
                                    if (x - 2 >= 0 & y + 1 < 8)
                                    {
                                        Move = new List<int> { x, y, x - 2, y + 1 };
                                        if (depth > 0)
                                        {
                                            if (Play_as == "Black")
                                            {
                                                if (checkscore(Move[2], Move[3]) >= Score_B & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                                {
                                                    ChessPiece bkup = new ChessPiece();
                                                    bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                    Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + " x - 2, y + 1 " + checkscore(x - 2, y + 1));
                                                    Score_B = checkscore(Move[2], Move[3]);
                                                    Score_W = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                    BMove = Move;
                                                    undo(Ai_BoardArray, bkup, Move);
                                                }
                                            }
                                            else if (checkscore(Move[2], Move[3]) >= Score_W & Ai_BoardArray[Move[2]][Move[3]].TeamColour != Play_as)
                                            {
                                                ChessPiece bkup = new ChessPiece();
                                                bkup = Ai_BoardArray[Move[2]][Move[3]];

                                                Debug.WriteLine(Move[2]+" "+Move[3]+" "+ Play_as + " x - 2, y + 1 " + checkscore(x - 2, y + 1));
                                                Score_W = checkscore(Move[2], Move[3]);
                                                Score_B = minmax(virtual_MovePiece(Ai_BoardArray, Move), enemy, depth - 1, Score_W, Score_B);
                                                undo(Ai_BoardArray, bkup, Move);
                                            }
                                        }
                                        else
                                        {
                                            Debug.WriteLine(Move[2] + " " + Move[3] + " " + Play_as + "x - 1, y - 2");
                                            if (Play_as == "Black")
                                                return Score_W;
                                            else
                                                return Score_B;
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

            if (depth == Ai_Level)
            {
                
            }
            if (Play_as == "Black")
            {
                return Score_W;
            }
            else return Score_B;
        }
    }

}
