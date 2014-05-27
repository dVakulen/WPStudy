using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ImagesGrid
{
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ImagesGrid.Models;
    using ImagesGrid.Services;
    using ImagesGrid.ViewModels;

    using PhotoHubSample;

    public partial class TeamManagementPage : PhoneApplicationPage
    {
        private ImagesViewModel dataContext;

        private Dictionary<Attribut, int> attributeStatisticsDictionary = new Dictionary<Attribut, int>
                                                                              {
                                                                                 { Attribut.Earth, 0 },
                                                                                  { Attribut.Dark, 0 },
                                                                                   { Attribut.Fire, 0 },
                                                                                    { Attribut.Light, 0 },
                                                                                     { Attribut.Water, 0 },
                                                                              };

        private Dictionary<Attribut, SolidColorBrush> attributesColorsDictionary = new Dictionary<Attribut, SolidColorBrush>
                                                                              {
                                                                                 { Attribut.Earth,  new SolidColorBrush(Colors.Green) },
                                                                                  { Attribut.Dark,  new SolidColorBrush(Colors.Purple) },
                                                                                   { Attribut.Fire, new SolidColorBrush(Colors.Red) },
                                                                                    { Attribut.Light, new SolidColorBrush(Colors.Yellow)  },
                                                                                     { Attribut.Water, new SolidColorBrush(Colors.Blue)  },
                                                                              };
        public TeamManagementPage()
        {

            const string emptySlot = "EmptySlot";
            InitializeComponent();
            this.dataContext = App.ViewModel;
            this.DataContext = App.ViewModel;
            //  var b = DataService.GetImages().Result;
            Enum.GetValues(typeof(Attribut)).Cast<Attribut>();
            int i = 0;
            foreach (var card in this.dataContext.CurrentTeam.UserCardInTeams)
            {
                i++;
                var teamImage = new Image();
                var img = DataService.GetImage(card.Image.ImageSource);
                teamImage.Source = img.Image;
                teamImage.MaxHeight = 80;
                teamImage.MaxWidth = 80;
                teamImage.DataContext = card;
                teamImage.Tap += TeamImage_Tap;
                teamImage.Name = i.ToString();
                this.TeamsPanel.Children.Add(teamImage);
                this.attributeStatisticsDictionary[card.Attribute]+= card.Attack;
            }
            for (; i < 5; i++)
            {
                var emptyTeamImage = new Image();
                BitmapImage img = new BitmapImage();
                img.SetSource(
                    Application.GetResourceStream(
                        new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative)).Stream);
                emptyTeamImage.Source = img;
                emptyTeamImage.Name = emptySlot;
                emptyTeamImage.MaxHeight = 80;
                emptyTeamImage.MaxWidth = 80;
                emptyTeamImage.Tap += EmptyTeamImage_Tap;
                this.TeamsPanel.Children.Add(emptyTeamImage);
            }
            foreach (var attr in attributeStatisticsDictionary)
            {
               ListBoxItem attrItem= new ListBoxItem();
                attrItem.Content = string.Format("{0} : {1}", attr.Key, attr.Value);
                attrItem.Foreground = attributesColorsDictionary[attr.Key];
                attrItem.FontSize = 38;
                this.AttributeStatistics.Items.Add(attrItem);
            }
        }
        private void TeamImage_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("Team");
        }
        private void EmptyTeamImage_Tap(object sender, GestureEventArgs e)
        {
            MessageBox.Show("EmptyTeam");
        }
    }
}