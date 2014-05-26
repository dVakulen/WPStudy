using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagesGrid.Models
{
    using System.ComponentModel;
    using System.Data.Linq;
    using System.Data.Linq.Mapping;

    [Table]
    public class CardInTeam : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields

        [Column(CanBeNull = false)]
        public Guid imageId;

        private int attack;
        [Column(CanBeNull = true)]
        private Guid? teamId;
        private Attribut attribute;

        private Guid id;

        private EntityRef<ImageCard> image;

        private string largeImageName;

        private EntityRef<Team> team;
        private string name;

        #endregion

        #region Constructors and Destructors

        public CardInTeam()
        {
            this.largeImageName = "A";
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Public Properties

        [Column(CanBeNull = false)]
        public int Attack
        {
            get
            {
                return this.attack;
            }

            set
            {
                this.NotifyPropertyChanging("Attack");
                this.attack = value;
                this.NotifyPropertyChanged("Attack");
            }
        }
        [Association(Name = "FK_Team_UserCardInTeam", Storage = "team", ThisKey = "teamId", OtherKey = "Id", IsForeignKey = true)]
        public Team Team
        {
            get { return this.team.Entity; }
            set
            {
                this.NotifyPropertyChanging("Team");
                this.team.Entity = value;
                if (value != null)
                    this.teamId = value.Id;
                this.NotifyPropertyChanged("Team");
            }
        }


        [Column(CanBeNull = false)]
        public Attribut Attribute
        {
            get
            {
                return this.attribute;
            }

            set
            {
                this.NotifyPropertyChanging("Attribute");
                this.attribute = value;
                this.NotifyPropertyChanged("Attribute");
            }
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public Guid Id
        {
            get
            {
                return this.id;
            }

            set
            {
                this.NotifyPropertyChanging("Id");
                this.id = value;
                this.NotifyPropertyChanged("Id");
            }
        }

        [Association(Name = "ImageCard1", Storage = "image", ThisKey = "imageId", OtherKey = "Id", IsForeignKey = true)]
        public ImageCard Image
        {
            get
            {
                return this.image.Entity;
            }

            set
            {

                this.NotifyPropertyChanging("Image");

                this.image.Entity = value;
                if (value != null)
                {
                    this.imageId = value.Id;
                }

                this.NotifyPropertyChanged("Image");

            }
        }

        [Column(CanBeNull = false)]
        public string LargeImageName
        {
            get
            {
                return this.largeImageName;
            }

            set
            {
                this.NotifyPropertyChanging("LargeImageName");
                this.largeImageName = value;
                this.NotifyPropertyChanged("LargeImageName");
            }
        }

        [Column(CanBeNull = false)]
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                this.NotifyPropertyChanging("Name");
                this.name = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        #endregion

        #region Methods

        private void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void NotifyPropertyChanging(string propertyName)
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
