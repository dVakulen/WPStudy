
#region Using Directives

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using ImagesGrid.Models;
using ImagesGrid.Repository;
using ImagesGrid.ViewModels;

using System.Collections.Generic;
using System.Linq;
using Microsoft.Phone.Controls;

using PhotoHubSample;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion
namespace ImagesGrid
{

    public partial class CardPage : PhoneApplicationPage
    {
        #region Fields

        private Repository<Card> cardRepository = new Repository<Card>();

        private ImagesViewModel dataContext;

        #endregion

        #region Constructors and Destructors

        public CardPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ViewModel;
            this.dataContext = App.ViewModel;
            this.dataContext.IsInSelectingCardToTeam = false;
            if (this.dataContext.IsCardNew)
            {
                this.CardAttr.ItemsSource = Enum.GetValues(typeof(Attribut)).Cast<Attribut>();

                this.Save.Visibility = Visibility.Visible;
                this.CardAttack.IsReadOnly = false;

                this.CardName.IsReadOnly = false;
                if (this.dataContext.CurrentCard.Image == null)
                {
                    BitmapImage img = new BitmapImage();
                    img.SetSource(
                        Application.GetResourceStream(
                            new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative)).Stream);
                    this.CardImg.Source = img;
                }
                this.CardAttr.SelectedItem = this.CardAttr.Items.Where(c => c.ToString() == this.dataContext.CurrentCard.Attribute.ToString()).FirstOrDefault();
            }
            else
            {
                var cardAttr = new List<string>();
                cardAttr.Add(dataContext.CurrentCard.Attribute.ToString());

                this.CardAttr.ItemsSource = cardAttr;
            }
        }

        #endregion

        #region Methods

        private void CardAttack_BindingValidationError(object sender, ValidationErrorEventArgs e)
        {
        }

        private void CardAttack_Tap(object sender, GestureEventArgs e)
        {
            this.CardAttack.SelectAll();
        }

        private void Image_Tap(object sender, GestureEventArgs e)
        {
            if (this.dataContext.IsCardNew)
            {
                this.NavigationService.Navigate(new Uri("/AllImagesPage.xaml", UriKind.Relative));
            }
        }

        private void Save_Tap(object sender, GestureEventArgs e)
        {
            if (this.dataContext.CurrentCard.Name == string.Empty || this.dataContext.CurrentCard.Image == null)
            {
                MessageBox.Show("Item cann't  contain empty fields");
                return;
            }
            Attribut res;
            bool success = Attribut.TryParse(CardAttr.SelectedItem.ToString(), out res);
            if (success)
            {
                this.dataContext.CurrentCard.Attribute = res;
            }
            if (this.dataContext.CurrentCard.Attack == 0)
            {
                this.dataContext.CurrentCard.Attack = Convert.ToInt32(this.CardAttack.Text);
            }
           
            this.dataContext.CurrentCard.Image = new ImageCard
                                                     {
                                                         ImageSource =
                                                             this.dataContext.CurrentCard.Image.ImageSource,
                                                         TimeStamp = DateTime.Now,
                                                         Title = this.dataContext.CurrentCard.Image.Title
                                                     };

            this.cardRepository.Insert(this.dataContext.CurrentCard);
            this.cardRepository.SubmitChanges();

            this.dataContext.CurrentCard = null;

            MessageBox.Show(" New card successfully created");

            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        #endregion

        private void CardAttr_Tap(object sender, GestureEventArgs e)
        {
        }
        protected override void OnNavigatingFrom(System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            Attribut res;
            bool success = Attribut.TryParse(CardAttr.SelectedItem.ToString(), out res);
            if (success && this.dataContext.CurrentCard != null)
            {
                this.dataContext.CurrentCard.Attribute = res;
            }
        }

        private void CardAttr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}