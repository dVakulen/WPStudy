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
    using System.Windows.Media.Imaging;

    using ImagesGrid.Models;
    using ImagesGrid.Services;
    using ImagesGrid.ViewModels;

    using PhotoHubSample;

    public partial class TeamManagementPage : PhoneApplicationPage
    {
        private ImagesViewModel dataContext;

        public TeamManagementPage()
        {
            InitializeComponent();
            this.dataContext = App.ViewModel;
            this.DataContext = App.ViewModel;
            //  var b = DataService.GetImages().Result;

            int i = 0;
            foreach (var team in this.dataContext.CurrentTeam.UserCardInTeams)
            {
                i++;
                var teamImage = new Image();
                var img = DataService.GetImage(team.Image.ImageSource);
                teamImage.Source = img.Image;
                teamImage.MaxHeight = 80;
                teamImage.MaxWidth = 80;
                teamImage.DataContext = team;
                teamImage.Name = i.ToString();
                this.TeamsPanel.Children.Add(teamImage);
            }
            for (; i < 5; i++)
            {
                var emptyTeamImage = new Image();
                BitmapImage img = new BitmapImage();
                img.SetSource(
                    Application.GetResourceStream(
                        new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative)).Stream);
                emptyTeamImage.Source = img;
                emptyTeamImage.Name = i.ToString();
                emptyTeamImage.MaxHeight = 80;
                emptyTeamImage.MaxWidth = 80; 
                this.TeamsPanel.Children.Add(emptyTeamImage);
            }
        }
    }
}