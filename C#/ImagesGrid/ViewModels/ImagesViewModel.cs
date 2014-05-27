#region Using Directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

using ImagesGrid.Helpers;
using ImagesGrid.Models;
using ImagesGrid.Services;

#endregion
namespace ImagesGrid.ViewModels
{


    public class ImagesViewModel : INotifyPropertyChanged
    {
        #region Fields

        public int CardsCount;

       // public List<string> Names = DataService.GetImagesNamesList(false);

        public List<Card> cards;

        private bool isLoading;

        private bool orderBy;

        private List<Photo> phots;

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public List<Card> Cards
        {
            get
            {
                return this.cards;
            }

            set
            {
                this.cards = value;

                Deployment.Current.Dispatcher.BeginInvoke(
                    () =>
                        {
                            this.CardsCount = value.Count;
                            this.NotifyPropertyChanged("GroupedCardsList");
                            this.NotifyPropertyChanged("GroupedCards");
                        });
            }
        }

        public Card CurrentCard { get; set; }

        public List<AlphaKeyGroup<Card>> GroupedCards
        {
            get
            {
                var cards = this.Cards;
                return AlphaKeyGroup<Card>.CreateGroups(cards, s => s.Name, true);
            }
        }

        public bool IsInSelectingCardToTeam;
        public Team CurrentTeam { get; set; }
        public List<KeyedList<string, Card>> GroupedCardsList
        {
            get
            {
                IEnumerable<KeyedList<string, Card>> groupedPhotos;
                if (this.Cards == null)
                {
                    return null;
                }

                if (!this.OrderByAttack)
                {
                    groupedPhotos = from photo in this.Cards
                                    orderby photo.Attack
                                    group photo by photo.LargeImageName
                                    into images
                                    select new KeyedList<string, Card>(images);
                }
                else
                {
                    groupedPhotos = from photo in this.Cards
                                    orderby photo.Attribute
                                    group photo by photo.LargeImageName
                                    into images
                                    select new KeyedList<string, Card>(images);
                }

                return new List<KeyedList<string, Card>>(groupedPhotos);
            }
        }

        public List<KeyedList<string, Photo>> GroupedPhotos
        {
            get
            {
                if (this.photos == null)
                {
                    return null;
                }

                var groupedPhotos = from photo in this.photos
                                    orderby photo.TimeStamp
                                    group photo by photo.TimeStamp.ToString("y")
                                    into images
                                    select new KeyedList<string, Photo>(images);

                return new List<KeyedList<string, Photo>>(groupedPhotos);
            }
        }

        public bool IsCardNew { get; set; }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                this.NotifyPropertyChanged();
            }
        }

        public bool OrderByAttack
        {
            get
            {
                return this.orderBy;
            }

            set
            {
                if (this.orderBy != value)
                {
                    this.orderBy = value;
                    this.NotifyPropertyChanged("GroupedCardsList");
                }
            }
        }

        public List<Photo> photos
        {
            get
            {
                return this.phots;
            }

            set
            {
                this.phots = value;
                this.NotifyPropertyChanged("GroupedPhotos");
            }
        }

        #endregion

        #region Public Methods and Operators

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}