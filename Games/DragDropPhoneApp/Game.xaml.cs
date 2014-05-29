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
    using System.Windows.Media;
    using System.Windows.Shapes;

    public partial class Game : PhoneApplicationPage
    {
        public Game()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 15; i++)
            {
                
            }
          /*  ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();

            ColumnDefinition colDef4 = new ColumnDefinition();
            ContentPanel.ColumnDefinitions.Add(colDef1);
            ContentPanel.ColumnDefinitions.Add(colDef2);
            ContentPanel.ColumnDefinitions.Add(colDef3);

            ContentPanel.ColumnDefinitions.Add(colDef4);
            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            ContentPanel.RowDefinitions.Add(rowDef1);
            ContentPanel.RowDefinitions.Add(rowDef2);
            ContentPanel.RowDefinitions.Add(rowDef3);
            ContentPanel.RowDefinitions.Add(rowDef4);

            // Add the first text cell to the Grid
            TextBlock txt1 = new TextBlock();
            
            txt1.Text = "2005 Products Shipped";
            txt1.FontSize = 12;
            txt1.FontWeight = FontWeights.Bold;
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, 0);


           // Rectangle rect = new Rectangle();
          
          //  rect.
           

            // Add the second text cell to the Grid
            TextBlock txt2 = new TextBlock();
            txt2.Text = "Quarter 1";
            txt2.FontSize = 12;
            txt2.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt2, 1);
            Grid.SetColumn(txt2, 0);

            // Add the third text cell to the Grid
            TextBlock txt3 = new TextBlock();
            txt3.Text = "Quarter 2";
            txt3.FontSize = 12;
            txt3.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt3, 1);
            Grid.SetColumn(txt3, 1);

            // Add the fourth text cell to the Grid
            TextBlock txt4 = new TextBlock();
            txt4.Text = "Quarter 3";
            txt4.FontSize = 12;
            txt4.FontWeight = FontWeights.Bold;
            Grid.SetRow(txt4, 1);
            Grid.SetColumn(txt4, 2);

            // Add the sixth text cell to the Grid
            TextBlock txt5 = new TextBlock();
            Double db1 = new Double();
            db1 = 50000;
            txt5.Text = db1.ToString();
            Grid.SetRow(txt5, 2);
            Grid.SetColumn(txt5, 0);

            // Add the seventh text cell to the Grid
            TextBlock txt6 = new TextBlock();
            Double db2 = new Double();
            db2 = 100000;
            txt6.Text = db2.ToString();
            Grid.SetRow(txt6, 2);
            Grid.SetColumn(txt6, 1);

            // Add the final text cell to the Grid
            TextBlock txt7 = new TextBlock();
            Double db3 = new Double();
            db3 = 150000;
            txt7.Text = db3.ToString();
            Grid.SetRow(txt7, 2);
            Grid.SetColumn(txt7, 2);

            // Total all Data and Span Three Columns
            TextBlock txt8 = new TextBlock();
            txt8.FontSize = 16;
            txt8.FontWeight = FontWeights.Bold;
            txt8.Text = "Total Units: " + (db1 + db2 + db3).ToString();
            Grid.SetRow(txt8, 3);
            Grid.SetColumnSpan(txt8, 3);





            Button b = new Button();
            b.Content = 4;
            Grid.SetRow(b, 1);
            Grid.SetColumn(b, 4);

            Button b1 = new Button();
            b1.Content = 3;
            Grid.SetRow(b1,2);
            Grid.SetColumn(b1, 4);
            Button b2 = new Button();
            b2.Content = 2;
            Grid.SetRow(b2, 3);
            Grid.SetColumn(b2, 4);
            Button b0= new Button();
            b0.Content = 2;
            Grid.SetRow(b0, 0);
            Grid.SetColumn(b0, 4);
            b0.Background = new SolidColorBrush(Color.FromArgb(255, 100, 10, 10)); 
            // Add the TextBlock elements to the Grid Children collection
            ContentPanel.Children.Add(b);
            ContentPanel.Children.Add(b1);
            ContentPanel.Children.Add(b0);
            ContentPanel.Children.Add(b2);
            ContentPanel.Children.Add(txt1);
            ContentPanel.Children.Add(txt2);
            ContentPanel.Children.Add(txt3);
            ContentPanel.Children.Add(txt4);
            ContentPanel.Children.Add(txt5);
            ContentPanel.Children.Add(txt6);
            ContentPanel.Children.Add(txt7);
            ContentPanel.Children.Add(txt8);*/
        }
        private int[] usedNumbers = new int[15];

        private int usedNumbersCount = 0;
        private int GetNextRandomNumber()
        {
            Random r = new Random();
            int num;
            bool unique = false;
            do
            {

                 num = r.Next(1, 15);
                unique = usedNumbers.Any(v => v == num);

            }
            while (!unique);
            usedNumbers[usedNumbersCount] = num;
            usedNumbersCount++;
            return num;


        }
        public void CreateButtonsWithNumbers()
        {

            Button btn = new Button();
            btn.Content = 4;
            Grid.SetRow(btn, 1);
            Grid.SetColumn(btn, 4);
        }
    }
}