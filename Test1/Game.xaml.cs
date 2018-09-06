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
        private List<ChessPieceUnit> BlackPieces = new List<ChessPieceUnit>();
        public Game()
        {
            this.InitializeComponent();
            SetTable();
            void SetTable()
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        ChessPieceUnit x = new ChessPieceUnit();
                        if (j == 0)
                        {
                            x.SetPieceTeam(false);
                            Image PieceImage = new Image();
                            if (i == 0 || i == 7) { x.SetPieceType(PieceTypes[3]); PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_rook.png")); }
                            else if (i == 1 || i == 6) { x.SetPieceType(PieceTypes[2]); PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_knight.png")); }
                            else if (i == 2 || i == 5) { x.SetPieceType(PieceTypes[1]); PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_bishop.png")); }
                            else if (i == 3) { x.SetPieceType(PieceTypes[4]); PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_queen.png")); }
                            else if (i == 4) { x.SetPieceType(PieceTypes[4]); PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_king.png")); }
                            PieceImage.Width = 80;
                            PieceImage.Height = 80;
                            PieceImage.HorizontalAlignment = HorizontalAlignment.Left;
                            PieceImage.VerticalAlignment = VerticalAlignment.Top;
                            PieceImage.Margin = new Thickness(80 * i, 0, 0, 0);
                            Chessboard.Children.Add(PieceImage);

                        }
                        else if (j == 1)
                        {
                            x.SetPieceType(PieceTypes[0]);
                            x.SetPieceTeam(false);
                            Image PieceImage = new Image();
                            PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_pawn.png"));
                            PieceImage.Width = 80;
                            PieceImage.Height = 80;
                            PieceImage.HorizontalAlignment = HorizontalAlignment.Left;
                            PieceImage.VerticalAlignment = VerticalAlignment.Top;
                            PieceImage.Margin = new Thickness(80 * i, 80, 0, 0);
                            Chessboard.Children.Add(PieceImage);

                        }

                        BlackPieces.Add(x);
                        
                    }
                }
            }
            
        }

        public class ChessPieceUnit : ChessPiece
        {
            public ChessPieceUnit()
            {
                Image PieceImage = new Image();
                PieceImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pieces/b_bishop.png"));
                PieceImage.Width = 80;
                PieceImage.Height = 80;
                
            }


        }
        
    }
}
