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
using Microsoft.Xna.Framework;
using System.Windows.Media.Imaging;

namespace DragDropPhoneApp
{
    public partial class MainPage : PhoneApplicationPage
    {

       

        FrameworkElement ElemToMove = null;
        double ElemVelX, ElemVelY;

        const double SPEED_FACTOR = 60;

        DispatcherTimer timer;



     

        public MainPage()
        {
            InitializeComponent();

            

            AddDragableItemOnCanvas();

        }


      

        void OnManipulationCompleted(object sender, ManipulationCompletedEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            ElemToMove = Elem;

            ElemVelX = args.FinalVelocities.LinearVelocity.X / SPEED_FACTOR;
            ElemVelY = args.FinalVelocities.LinearVelocity.Y / SPEED_FACTOR;

           // timer.Start();
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

    }
}