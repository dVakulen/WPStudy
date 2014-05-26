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
    public class Team : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public Team()
        {
            if (userCardInTeams == null)
            {
                userCardInTeams = new EntitySet<CardInTeam>();
            }
        }
        private int number;
        private Guid id;

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

        private EntitySet<CardInTeam> userCardInTeams;

        [Association(Name = "FK_UserCardInTeam_Team", Storage = "userCardInTeams", ThisKey = "Id", OtherKey = "teamId")]
        public EntitySet<CardInTeam> UserCardInTeams
        {
            get { return this.userCardInTeams; }
            set { this.userCardInTeams.Assign(value); }
        }







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
    }

}
