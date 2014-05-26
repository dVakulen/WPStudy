
#region Using Directives

using System;
using System.Windows.Controls;

using ImagesGrid.Models;
using ImagesGrid.ViewModels;

using Microsoft.Phone.Controls;

using PhotoHubSample;

#endregion
namespace ImagesGrid
{

    public partial class AllImagesPage : PhoneApplicationPage
    {
        #region Fields

        private ImagesViewModel dataContext;

        #endregion

        #region Constructors and Destructors

        public AllImagesPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ViewModel;
            this.dataContext = App.ViewModel;
        }

        #endregion

        #region Methods

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is LongListSelector)
            {
                var sendr = sender as LongListSelector;
                if (sendr.SelectedItem == null)
                {
                    return;
                }

                if (sendr.SelectedItem is Photo)
                {
                    this.dataContext.CurrentCard.Image = sendr.SelectedItem as Photo;
                    this.dataContext.CurrentCard.Image.TimeStamp = DateTime.Now;

                    sendr.SelectedItem = null;
                    this.NavigationService.Navigate(new Uri("/CardPage.xaml", UriKind.Relative));
                }
            }
        }

        #endregion
    }
}