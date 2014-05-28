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
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace GameAndCircles
{
    using Windows.Storage.Provider;
    using Windows.UI;
    using Windows.UI.Input;
    using Windows.UI.Popups;
    using Windows.UI.Xaml.Media.Imaging;
    using Windows.UI.Xaml.Shapes;

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Circles : Page
    {
        public Circles()
        {
            this.InitializeComponent();
       
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ContentPanel_Tapped(object sender, TappedRoutedEventArgs e)
        {

          /*  Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.Red);
           
            Canvas.SetLeft(el, e.GetPosition(ContentPanel).X);
            Canvas.SetTop(el, e.GetPosition(ContentPanel).Y);
            ContentPanel.Children.Add(el);*/
        }

        private void ContentPanel_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {

            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.Blue);
          
            Canvas.SetLeft(el, e.Position.X);
            Canvas.SetTop(el, e.Position.Y);
            ContentPanel.Children.Add(el);
        }

        private void ContentPanel_GotFocus(object sender, RoutedEventArgs e)
        {

            Ellipse el = new Ellipse();
            el.Width = 10;
            el.Height = 10;
            el.Fill = new SolidColorBrush(Colors.Blue);

            Canvas.SetLeft(el, 100);
            Canvas.SetTop(el, 100);
            ContentPanel.Children.Add(el);
        }

   
        private void Circle_Tapped(object sender, TappedRoutedEventArgs e)
        {
         
        }
        private void Circle_Holded(object sender, HoldingRoutedEventArgs e)
        {
            Info.Text = e.GetPosition(ContentPanel).X.ToString();
            if (sender is Ellipse)
            {
                var ellipse = sender as Ellipse;

                Canvas.SetLeft(ellipse, e.GetPosition(ContentPanel).X-35);
                Canvas.SetTop(ellipse, e.GetPosition(ContentPanel).Y-35);
            }
          
            //MessageDialog qwe = new MessageDialog("Rly?", "xx");
            //   qwe.ShowAsync();
        }
        FrameworkElement ElemToMove = null;
        double ElemVelX, ElemVelY;
        const double SPEED_FACTOR = 60;
        void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            ElemToMove = Elem;
            
            ElemVelX = args.Velocities.Linear.X / SPEED_FACTOR;
            ElemVelY = args.Velocities.Linear.Y / SPEED_FACTOR;

        //    timer.Start();
        }

        void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs args)
        {
            FrameworkElement Elem = sender as FrameworkElement;

            double Left = Canvas.GetLeft(Elem);
            double Top = Canvas.GetTop(Elem);

            Left += args.Cumulative .Translation.X;
            Top += args.Cumulative.Translation.Y;

            //check for bounds
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
        void image_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var image = sender as Ellipse;
            CompositeTransform ct = image.RenderTransform as CompositeTransform;
            if (ct == null) image.RenderTransform = ct = new CompositeTransform();
            ct.TranslateX += e.Delta.Translation.X;
            ct.TranslateY += e.Delta.Translation.Y;
        }
        void AddDragableItemOnCanvas(TappedRoutedEventArgs e)
        {
            Ellipse el = new Ellipse();
        //    el.Holding += Circle_Holded;
            el.ManipulationStarted += (sender, args) =>
            {
                MessageDialog qwe = new MessageDialog("Rly?", "xx");
                qwe.ShowAsync();
                    
                };
            el.ManipulationDelta += image_ManipulationDelta;
            el.ManipulationCompleted += OnManipulationCompleted;
            
            el.Width = 70;
            el.Height = 70;
            el.Fill = new SolidColorBrush(Colors.White);
        //    el.Tapped += Circle_Tapped;
            Canvas.SetLeft(el, e.GetPosition(ContentPanel).X - 35);
            Canvas.SetTop(el, e.GetPosition(ContentPanel).Y - 35);
            ContentPanel.Children.Add(el);

            //subscribe to events
          
        }

        private void Page_Tapped(object sender, TappedRoutedEventArgs e)
        {
            AddDragableItemOnCanvas(e);
            /*  Ellipse el = new Ellipse();
            el.Holding += Circle_Holded;
            el.
            el.Width = 70;
            el.Height = 70;
            el.Fill = new SolidColorBrush(Colors.White);
            el.Tapped += Circle_Tapped;
            Canvas.SetLeft(el, e.GetPosition(ContentPanel).X-35);
            Canvas.SetTop(el, e.GetPosition(ContentPanel).Y-35);
            ContentPanel.Children.Add(el);*/
        }
    }
}
