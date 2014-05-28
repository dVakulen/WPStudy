using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Threading;
using System.Windows.Media.Imaging;

namespace DragDropPhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {


        private List<FrameworkElement> circlesToDrop = new List<FrameworkElement>();
        FrameworkElement elemToMove = null;
        double ElemVelX, ElemVelY;

        const double SPEED_FACTOR = 60;

        private DispatcherTimer timer;


        const float Ratio = 0.15f;


        private static bool firstLoad = true;
        public MainPage()
        {
            InitializeComponent();
          
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(35);
            timer.Tick += this.OnTimerTick;



            AddDragableItemOnCanvas();

        }
        void OnTimerTick(object sender, EventArgs e)
        {
            Point target = new Point(240, 0);
            List<FrameworkElement> elemToRemove = new List<FrameworkElement>();
            foreach (var circle in circlesToDrop)
            {

                double Left, Top;
                Left = Canvas.GetLeft(circle);
                Top = Canvas.GetTop(circle);
                var yDelta = 1000 / Top;
                Point delta = new Point(target.X, yDelta - target.Y);
                if (delta.Y < 0.05f)
                {
                    elemToRemove.Add(circle);
                    continue;
                }
                if (yDelta - 1000 / MainCanvas.ActualHeight < 0.15f && yDelta - 1000 / MainCanvas.ActualHeight > -0.15f)
                {
                    elemToRemove.Add(circle);
                    Canvas.SetTop(circle, MainCanvas.ActualHeight - 75);
                    continue;
                }
                if (false&&Top - MainCanvas.ActualHeight < 20 && Top - MainCanvas.ActualHeight > -30)
                {
                    elemToRemove.Add(circle);
                    Canvas.SetTop(circle, MainCanvas.ActualHeight - 75);
                    continue;
                }
                Top += delta.Y;
                Canvas.SetTop(circle, Top);
            }

            foreach (var element in elemToRemove)
            {
                circlesToDrop.Remove(element);
            }
            /*
            if (this.elemToMove != null)
            {
                double Left, Top;
                Left = Canvas.GetLeft(this.elemToMove);
                Top = Canvas.GetTop(this.elemToMove);
                Vector2 currentPosition = new Vector2((float)Left, (float)Top);

                Microsoft.Xna.Framework.Rectangle movingRectangle = new Microsoft.Xna.Framework.Rectangle();
                movingRectangle.X = (int)Left;
                movingRectangle.Y = (int)Top;
                movingRectangle.Width = 100;
                movingRectangle.Height = 100;

                Vector2 Target = new Vector2(240, 0);

                if (movingRectangle.Intersects(rectangleA))
                {
                    Target.X = rectangleA.X;
                    Target.Y = rectangleA.Y;
                }

                Vector2 Delta = (Target - currentPosition) * RATIO;
                if (Delta.Length() < 0.05f)
                {
                    currentPosition = Target;
                    timer.Stop();
                }
                else
                {
                    currentPosition += Delta;
                }

                Canvas.SetLeft(this.elemToMove, currentPosition.X);
                Canvas.SetTop(this.elemToMove, currentPosition.Y);
            }*/
        }




        void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            //    this.elemToMove = Elem;
            circlesToDrop.Add(Elem);
            ElemVelX = args.FinalVelocities.LinearVelocity.X / SPEED_FACTOR;
            ElemVelY = args.FinalVelocities.LinearVelocity.Y / SPEED_FACTOR;
            timer.Start();
        }

        void OnManipulationDelta(object sender, ManipulationDeltaEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            double Left = Canvas.GetLeft(Elem);
            double Top = Canvas.GetTop(Elem);

            Left += args.DeltaManipulation.Translation.X;
            Top += args.DeltaManipulation.Translation.Y;
            if (Left < 0)
            {
                Left = 0;
            }
            else if (Left > (LayoutRoot.ActualWidth - Elem.ActualWidth))
            {
                Left = LayoutRoot.ActualWidth - Elem.ActualWidth;
            }

            if (Top < 0)
            {
                Top = 0;
            }
            else if (Top > (LayoutRoot.ActualHeight - Elem.ActualHeight))
            {
                Top = LayoutRoot.ActualHeight - Elem.ActualHeight;
            }

            Canvas.SetLeft(Elem, Left);
            Canvas.SetTop(Elem, Top);
        }



        void AddDragableItemOnCanvas()
        {
            return;
            Image appleImage = new Image { Source = new BitmapImage(new Uri("/images/Apple.png", UriKind.Relative)) };

            Canvas.SetTop(appleImage, 0);
            Canvas.SetLeft(appleImage, 240);

            MainCanvas.Children.Add(appleImage);

            appleImage.ManipulationDelta += OnManipulationDelta;
            appleImage.ManipulationCompleted += OnManipulationCompleted;
        }
        void AddCircleOnCanvas(GestureEventArgs e)
        {
            var el = new Ellipse { Width = 70, Height = 70, Fill = new SolidColorBrush(Colors.White) };

            el.ManipulationDelta += (OnManipulationDelta);
            el.ManipulationCompleted += OnManipulationCompleted;
            Canvas.SetLeft(el, e.GetPosition(MainCanvas).X - 35);
            Canvas.SetTop(el, e.GetPosition(MainCanvas).Y - 35);
            MainCanvas.Children.Add(el);

        }



        private void MainCanvas_Tap(object sender, GestureEventArgs e)
        {
            AddCircleOnCanvas(e);

        }

        private void PhoneApplicationPage_Tap(object sender, GestureEventArgs e)
        {

        }

        private void MainCanvas_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (firstLoad)
            {
                firstLoad = false;
                this.NavigationService.Navigate(new Uri("/Menu.xaml", UriKind.Relative));
            }
        }

    }
}