#region Using Directives

using System;
using System.Data.Linq;

using ImagesGrid.Models;
using ImagesGrid.Services;

#endregion
namespace ImagesGrid.Context
{



    public class CardsContext : DataContext
    {
        #region Constants

        private const string ConnectionString = "DataSource=isostore:/Test.sdf";

        #endregion

        #region Static Fields

        private static bool initialized;

        #endregion

        #region Fields

        public Table<ImageCard> ImageCards;

        public Table<Team> Team;
        public Table<Card> UserCards;
        public Table<CardInTeam> CardInTeam;

        #endregion

        #region Constructors and Destructors

        public CardsContext()
            : this(ConnectionString)
        {
        }

        public CardsContext(string connectionString)
            : base(connectionString)
        {
            if (this.DatabaseExists() && !initialized)
            {
                this.DeleteDatabase();
                initialized = true;
            }

            if (!this.DatabaseExists())
            {
                this.CreateDatabase();
                var z = this.UserCards;
                z.InsertOnSubmit(
                    new Card
                        {
                            Id = Guid.NewGuid(),
                            Attack = 5,
                            Attribute = Attribut.Black,
                            LargeImageName = "A",
                            Name = "Asd",
                            Image =
                                new ImageCard
                                    {
                                        Id = Guid.NewGuid(),
                                        TimeStamp = DateTime.Now,
                                        Title = "asd",
                                        ImageSource = "Images/e1.png"
                                    }
                        });

                var d = DataService.GetImagesNamesList(true);

                for (int i = 0; i < 15; i++)
                {
                    z.InsertOnSubmit(
                        new Card
                            {
                                Attack = i,
                                Attribute = Attribut.Green,
                                LargeImageName = "A",
                                Name = "Asd" + i,
                                Image =
                                    new ImageCard { TimeStamp = DateTime.Now, Title = "asd", ImageSource = d[i] }
                            });
                }

                this.SubmitChanges();
            }
        }

        #endregion

    }
}