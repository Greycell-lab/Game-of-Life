using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Game_of_Life
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();

        const int CellsWidth = 50;
        const int CellsHeight = 50;
        Rectangle[,] field = new Rectangle[CellsHeight, CellsWidth];
        DispatcherTimer gameTime = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();
            gameTime.Interval = TimeSpan.FromSeconds(0.1);
            gameTime.Tick += GameTime_Tick;
            myCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            myCanvas.Arrange(new Rect(0.0,0.0,myCanvas.DesiredSize.Width, myCanvas.DesiredSize.Height));
            for (int i = 0; i < CellsHeight; i++)
            {
                for (int j = 0; j < CellsWidth; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Width = myCanvas.ActualWidth / CellsWidth - 2.0;
                    r.Height = myCanvas.ActualHeight / CellsHeight - 2.0;
                    r.Fill = Brushes.Cyan;
                    myCanvas.Children.Add(r);
                    Canvas.SetLeft(r, j * myCanvas.ActualWidth / CellsWidth);
                    Canvas.SetTop(r, i * myCanvas.ActualHeight / CellsHeight);
                    r.MouseDown += R_MouseDown;
                    field[i, j] = r;
                }
            }
        }     

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {            
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.Cyan) ? Brushes.Red : Brushes.Cyan;
        }

        private void GameTime_Tick(object sender, EventArgs e)
        {
            int cyanCounter = 0;
            int redCounter = 0;
            foreach(var x in field)
            {
                if (((Rectangle)x).Fill == Brushes.Cyan) cyanCounter++;
                if (((Rectangle)x).Fill == Brushes.Red) redCounter++;
            }
            cyan.Content = "Death: " + cyanCounter;
            red.Content = "Alive: " + redCounter;
            int[,] countNext = new int[CellsHeight, CellsWidth];
            
            for(int i=0;i<CellsHeight;i++)
            {
                for(int j=0;j<CellsWidth;j++)
                {
                    int neighbours = 0;
                    int iAbove = i - 1;
                    int iUnder = i + 1;
                    int jLeft = j - 1;
                    int jRight = j + 1;
                    if (iAbove < 0) iAbove = CellsHeight - 1;
                    if (iUnder >= CellsHeight) iUnder = 0;
                    if (jLeft < 0) jLeft = CellsWidth - 1;
                    if (jRight >= CellsWidth) jRight = 0;
                    if (field[iAbove, jLeft].Fill == Brushes.Red) neighbours++;
                    if (field[iAbove, j].Fill == Brushes.Red) neighbours++;
                    if (field[iAbove, jRight].Fill == Brushes.Red) neighbours++;
                    if (field[i, jLeft].Fill == Brushes.Red) neighbours++;                    
                    if (field[i, jRight].Fill == Brushes.Red) neighbours++;
                    if (field[iUnder, jLeft].Fill == Brushes.Red) neighbours++;
                    if (field[iUnder, j].Fill == Brushes.Red) neighbours++;
                    if (field[iUnder, jRight].Fill == Brushes.Red) neighbours++;
                    countNext[i, j] = neighbours;                    
                }
            }
            for(int i=0;i<CellsHeight;i++)
            {
                for(int j=0;j<CellsWidth;j++)
                {
                    if (countNext[i,j] < 2 || countNext[i,j] > 3)
                    {
                        field[i, j].Fill = Brushes.Cyan;
                    }
                    else if (countNext[i,j] == 3)
                    {
                        field[i, j].Fill = Brushes.Red;
                    }
                }
            }
        }

        private void ButtonTimer(object sender, RoutedEventArgs e)
        {
            if (sender == starte && gameTime.IsEnabled)
            {
                gameTime.Stop();
                ((Button)sender).Content = "Starte Animation";
            }
            else if (sender == starte && !gameTime.IsEnabled)
            {
                gameTime.Start();
                ((Button)sender).Content = "Stoppe Animation";
            }
            if(sender == random)
            {
                for (int i = 0; i < CellsHeight; i++)
                {
                    for (int j = 0; j < CellsWidth; j++)
                    {
                        Rectangle r = new Rectangle();
                        r.Width = myCanvas.ActualWidth / CellsWidth - 2.0;
                        r.Height = myCanvas.ActualHeight / CellsHeight - 2.0;
                        r.Fill = rnd.Next(0, 2) == 1 ? Brushes.Cyan : Brushes.Red;
                        myCanvas.Children.Add(r);
                        Canvas.SetLeft(r, j * myCanvas.ActualWidth / CellsWidth);
                        Canvas.SetTop(r, i * myCanvas.ActualHeight / CellsHeight);
                        r.MouseDown += R_MouseDown;
                        field[i, j] = r;
                    }
                }
                int cyanCounter = 0;
                int redCounter = 0;
                foreach (var x in field)
                {
                    if (((Rectangle)x).Fill == Brushes.Cyan) cyanCounter++;
                    if (((Rectangle)x).Fill == Brushes.Red) redCounter++;
                }
                cyan.Content = "Death: " + cyanCounter;
                red.Content = "Alive: " + redCounter;
            }    
            if(sender == reset)
            {
                gameTime.Stop();
                foreach(var x in field)
                {
                    ((Rectangle)x).Fill = Brushes.Cyan;
                }
            }
        }
    }
}
