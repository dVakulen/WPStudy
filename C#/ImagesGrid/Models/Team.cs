
#region Using Directives

using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

#endregion
namespace ImagesGrid.Models
{

    [Table]
    public class Team : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields

        private Guid id;

        private int number;

        private EntitySet<CardInTeam> userCardInTeams;

        #endregion

        #region Constructors and Destructors

        public Team()
        {
            if (this.userCardInTeams == null)
            {
                this.userCardInTeams = new EntitySet<CardInTeam>();
            }
        }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Public Properties

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

        [Column(CanBeNull = false)]
        public int Number
        {
            get
            {
                return this.number;
            }

            set
            {
                this.number = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        [Association(Name = "FK_UserCardInTeam_Team", Storage = "userCardInTeams", ThisKey = "Id", OtherKey = "teamId")]
        public EntitySet<CardInTeam> UserCardInTeams
        {
            get
            {
                return this.userCardInTeams;
            }

            set
            {
                this.userCardInTeams.Assign(value);
                this.NotifyPropertyChanged("UserCardInTeams");
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