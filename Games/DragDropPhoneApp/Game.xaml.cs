using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace DragDropPhoneApp
{
    using System.Windows.Ink;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using System.Windows.Shapes;


    public partial class Game : PhoneApplicationPage
    {
        private double cWidth = 480;
        public Game()
        {
            InitializeComponent();
          
                {
                   // this.MainCanvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                  //  this.MainCanvas.Arrange(new Rect(0, 0, this.MainCanvas.ActualHeight, this.MainCanvas.ActualWidth));
                     
                    this.Initialize();
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            CreateButtonsWithNumbers(this.cells[i][j]);
                        }
                    }
                    MainCanvas.Children.Remove(cells[3][3].Element);
                    cells[3][3].IsEmptySpace = true;
                }
    
        }

      
        private Storyboard CreateStoryboard(UIElement element,Point pTo )
        {
            double to = pTo.Y;
            double toLeft =pTo.X;
            Storyboard result = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation();
            animation.To = to;
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Top)"));
            Storyboard.SetTarget(animation, element);
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(900));
            DoubleAnimation animationLeft = new DoubleAnimation();
            animationLeft.To = toLeft;
            Storyboard.SetTargetProperty(animationLeft, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(animationLeft, element);

            result.Children.Add(animation);
            result.Children.Add(animationLeft);

            return result;
        }
        void AddCircleOnCanvas()
        {
            var el = new Ellipse { Width = 70, Height = 70, Fill = new SolidColorBrush(Colors.White) };

            //     el.ManipulationDelta += (OnManipulationDelta);
            //    el.ManipulationCompleted += OnManipulationCompleted;
            Canvas.SetLeft(el, 150);
            Canvas.SetTop(el, 155);
            MainCanvas.Children.Add(el);

        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private int[] usedNumbers = new int[16];
        private Cell[][] cells = new Cell[4][];

        private double cellWidth = 100;
        private int usedNumbersCount = 0;

        private void Initialize()
        {

            cellWidth = cWidth / 4;
            for (int i1 = 0; i1 < 4; i1++)
            {
                cells[i1] = new Cell[4];
            }
            int j = 0;
            int k = 0;
            for (double i = 0; i < cWidth; i += cellWidth)
            {
                for (double w = 0; w < cWidth; w += cellWidth) //MainCanvas.ActualWidth
                {
                    cells[j][k] = new Cell { PositionOnCanvas = new Point(i, w), PositionInMatrix = new Point(j,k)};
                    k++;
                }
                k = 0;
                j++;
            }
        }

        private bool Move(Cell to, Cell from)
        {
            bool moved = false;
            if (to.IsEmptySpace)
            {
                var story = this.CreateStoryboard(from.Element, to.PositionOnCanvas);

                var temp = to.Element;
                to.Element = from.Element;
                from.Element = temp;
              //  cells[(int)from.PositionInMatrix.X][(int)from.PositionInMatrix.Y].IsEmptySpace = true;
                from.IsEmptySpace = true;
                 to.IsEmptySpace = false;
           //   var temp = from.PositionInMatrix;
            //  from.PositionInMatrix = to.PositionInMatrix;
              //  to.PositionInMatrix = temp;
                    story.Begin();

                    moved = true;
                //  var story = this.CreateStoryboard(cell.Element, CellToMove.PositionOnCanvas);

                //   cells[cellPosX][cellPosY].Element = CellToMove.Element;
                //  cells[cellPosX][cellPosY].IsEmptySpace = true;
                //    CellToMove.IsEmptySpace = false;

                //    story.Begin();
            }
            return moved;
        }
        private void cellClick(object sender, GestureEventArgs e)
        {

            var btn = sender as Button;
            if(btn == null) return;
            var cell = btn.DataContext as Cell;
            if (cell == null) return;

            var cellPosY =( int)cell.PositionInMatrix.Y ;
            var cellPosX = (int)cell.PositionInMatrix.X;
          
            var left = (int)cell.PositionInMatrix.X - 1;
            var right = (int)cell.PositionInMatrix.X + 1;
            var top = (int)cell.PositionInMatrix.Y + 1;
            var bottom = (int)cell.PositionInMatrix.Y - 1;
          
            if (left > 0)
            {

                var CellToMove = cells[left][cellPosY];
                //CellToMove.PositionInMatrix = new Point(left, cellPosY);
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    return;
                }

            }
            if (right < 4)
            {
                var CellToMove = cells[right][cellPosY];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    return;
                }
              
            }
            if (bottom >0)
            {
                var CellToMove = cells[cellPosX][bottom];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    return;
                }

            }
            if (top <4)
            {
                var CellToMove = cells[cellPosX][top];
                if (this.Move(CellToMove, cell))
                {
                    btn.DataContext = CellToMove;
                    return;
                }

            }
           
            //  if()
        }
        private int GetNextRandomNumber()
        {
            Random r = new Random();
            int num=0;
            bool unique;
            do
            {

                num = r.Next(0, 16);

                unique = usedNumbers.Contains(num);
                if(usedNumbersCount == 15) break;
            }
            while (unique);
            usedNumbers[usedNumbersCount] = num;
            usedNumbersCount++;
            return num;


        }
        public void CreateButtonsWithNumbers(Cell cell)
        {


            Button btn = new Button
                             {
                                 Width = this.cellWidth,
                                 DataContext = cell,
                                 Height = this.cellWidth,
                                 Content = this.GetNextRandomNumber()
                                
                             };
            btn.Tap += cellClick;
            Canvas.SetLeft(btn, cell.PositionOnCanvas.X);
            Canvas.SetTop(btn, cell.PositionOnCanvas.Y);
          
            cell.Element = btn;
            MainCanvas.Children.Add(btn);
        }
    }

    public class Cell
    {
        public bool IsEmptySpace = false;
        public Point PositionOnCanvas;

        public UIElement Element;

        public Point PositionInMatrix;
    }
}