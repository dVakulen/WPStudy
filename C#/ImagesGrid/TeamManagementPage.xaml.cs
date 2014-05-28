#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using ImagesGrid.Models;
using ImagesGrid.Repository;
using ImagesGrid.Services;
using ImagesGrid.ViewModels;

using Microsoft.Phone.Controls;

using PhotoHubSample;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion
namespace ImagesGrid
{


    public partial class TeamManagementPage : PhoneApplicationPage
    {
        #region Constants

        private const string emptySlot = "EmptySlot";

        #endregion

        #region Fields

        private Dictionary<Attribut, int> attributeStatisticsDictionary = new Dictionary<Attribut, int>
                                                                              {
                                                                                  {
                                                                                      Attribut
                                                                                      .Earth, 
                                                                                      0
                                                                                  }, 
                                                                                  {
                                                                                      Attribut
                                                                                      .Dark, 
                                                                                      0
                                                                                  }, 
                                                                                  {
                                                                                      Attribut
                                                                                      .Fire, 
                                                                                      0
                                                                                  }, 
                                                                                  {
                                                                                      Attribut
                                                                                      .Light, 
                                                                                      0
                                                                                  }, 
                                                                                  {
                                                                                      Attribut
                                                                                      .Water, 
                                                                                      0
                                                                                  }, 
                                                                              };

        private static Dictionary<Attribut, SolidColorBrush> attributesColorsDictionary =
            new Dictionary<Attribut, SolidColorBrush>
                {
                    { Attribut.Earth, new SolidColorBrush(Colors.Green) }, 
                    { Attribut.Dark, new SolidColorBrush(Colors.Purple) }, 
                    { Attribut.Fire, new SolidColorBrush(Colors.Red) }, 
                    { Attribut.Light, new SolidColorBrush(Colors.Yellow) }, 
                    { Attribut.Water, new SolidColorBrush(Colors.Blue) }, 
                };

        private ImagesViewModel dataContext;

        private Repository<CardInTeam> teamCardsRepository = new Repository<CardInTeam>();

        private Repository<Team> teamRepository = new Repository<Team>();

        #endregion

        #region Constructors and Destructors

        public TeamManagementPage()
        {
            this.InitializeComponent();
            this.dataContext = App.ViewModel;
            this.DataContext = App.ViewModel;

            this.Title.Text = this.Title.Text + " " + this.dataContext.CurrentTeam.Number;
            int i = 0;
            bool found = false;
            var cardInTeam = this.dataContext.CurrentTeam.UserCardInTeams;
            for (int j = 0; j < 5; j++)
            {
                if (i < cardInTeam.Count)
                {

                    for (int h = 0; h < cardInTeam.Count; h++)
                    {
                        if (cardInTeam[h].PlaceInTeam == j)
                        {
                            var card = cardInTeam[i];
                            var img = DataService.GetImage(card.Image.ImageSource);
                            var teamImage = new Image
                                                {
                                                    Source = img.Image,
                                                    MaxHeight = 80,
                                                    MaxWidth = 80,
                                                    DataContext = card,
                                                    Name = i.ToString()
                                                };
                            teamImage.Tap += this.TeamImage_Tap;
                            this.TeamsPanel.Children.Add(teamImage);
                            this.attributeStatisticsDictionary[card.Attribute] += card.Attack;
                            i++;
                            found = true;
                        }
                    }
                }
                if (!found)
                {
                    BitmapImage img = new BitmapImage();
                    img.SetSource(
                        Application.GetResourceStream(new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative))
                            .Stream);
                    var emptyTeamImage = new Image { Source = img, Name = emptySlot, MaxHeight = 80, MaxWidth = 80, Tag = j };
                    emptyTeamImage.Tap += this.EmptyTeamImage_Tap;
                    this.TeamsPanel.Children.Add(emptyTeamImage);
                }
                found = false;
            }


            foreach (var attr in this.attributeStatisticsDictionary)
            {
                ListBoxItem attrItem = new ListBoxItem();
                attrItem.Content = string.Format("{0} : {1}", attr.Key, attr.Value);
                attrItem.Foreground = attributesColorsDictionary[attr.Key];
                attrItem.FontSize = 38;
                this.AttributeStatistics.Items.Add(attrItem);
            }
        }

        #endregion

        #region Methods

        private void EmptyTeamImage_Tap(object sender, GestureEventArgs e)
        {
            this.dataContext.IsInSelectingCardToTeam = true;
            this.dataContext.SelectedCardPlace = Convert.ToInt32((sender as Image).Tag);
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            this.dataContext.CurrentTeam.UserCardInTeams.Clear();
            this.NavigationService.Navigate(new Uri("/TeamManagementPage.xaml?Refresh=true", UriKind.Relative));
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var team = this.teamRepository.GetAll().Where(v => v.Id == this.dataContext.CurrentTeam.Id).FirstOrDefault();
            foreach (var userCardInTeam in this.dataContext.CurrentTeam.UserCardInTeams)
            {
                if (!userCardInTeam.IsNew)
                    continue;

                userCardInTeam.IsNew = false;
                var image = new ImageCard
                                {
                                    ImageSource = userCardInTeam.Image.ImageSource,
                                    TimeStamp = DateTime.Now,
                                    Title = userCardInTeam.Image.Title
                                };
                userCardInTeam.Image = image;
                userCardInTeam.teamId = team.Id;
                team.UserCardInTeams.Add(userCardInTeam);
                this.teamCardsRepository.Insert(userCardInTeam);

            }

            this.teamCardsRepository.SubmitChanges();
            MessageBox.Show("Changes saved");
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        private void TeamImage_Tap(object sender, GestureEventArgs e)
        {
            var card = (sender as Image).DataContext as CardInTeam;
            MessageBox.Show(
                "Card " + card.Name + Environment.NewLine + "Attribute: " + card.Attribute + Environment.NewLine
                + "Attack: " + card.Attack);
        }

        #endregion
    }
}