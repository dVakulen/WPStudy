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
                var a = new CardInTeam
                {
                    PlaceInTeam = 2,
                    Attack = 2,
                    Attribute = Attribut.Light,
                    Name = "Asxda",
                    Image =
                        new ImageCard
                        {
                            Id = Guid.NewGuid(),
                            TimeStamp = DateTime.Now,
                            Title = "asd",
                            ImageSource = "Images/e1.png"
                        }
                };
                var b = new CardInTeam
                {PlaceInTeam = 1,
                    Attack = 7,
                    Attribute = Attribut.Light,
                    Name = "zxc",
                    Image =
                        new ImageCard
                        {
                            Id = Guid.NewGuid(),
                            TimeStamp = DateTime.Now,
                            Title = "dsaa",
                            ImageSource = "Images/e2.png"
                        }
                };
                var b1 = new CardInTeam
                {
                    PlaceInTeam = 3,
                    Attack = 14,
                    Attribute = Attribut.Earth,
                    Name = "asd",
                    Image =
                        new ImageCard
                        {
                            Id = Guid.NewGuid(),
                            TimeStamp = DateTime.Now,
                            Title = "fsd",
                            ImageSource = "Images/e3.png"
                        }
                };
              
                this.CardInTeam.InsertOnSubmit(a);
                var qqz = new Team { Number = 1, UserCardInTeams = new EntitySet<CardInTeam> { a, b, b1 } };
                a.Team = qqz;
                b.Team = qqz;
                b1.Team = qqz;
                var qqz1 = new Team { Number = 2, UserCardInTeams = new EntitySet<CardInTeam> { } };
              

                this.Team.InsertOnSubmit(qqz1);
                this.Team.InsertOnSubmit(qqz);
                var z = this.UserCards;
                z.InsertOnSubmit(
                    new Card
                        {
                            Id = Guid.NewGuid(),
                            Attack = 5,
                            Attribute = Attribut.Light,
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
                if (d.Count == 0)
                {
                 return;   
                }
                for (int i = 0; i < 15; i++)
                {
                    z.InsertOnSubmit(
                        new Card
                            {
                                Attack = i,
                                Attribute = Attribut.Fire,
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