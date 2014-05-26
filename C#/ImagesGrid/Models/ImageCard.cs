#region Using Directives

using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

#endregion

namespace ImagesGrid.Models
{

    [Table]
    public class ImageCard
    {
        #region Fields

        private Guid id;

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
                this.id = value;
            }
        }

        [Column(CanBeNull = false)]
        public string ImageSource { get; set; }

        [Column]
        public DateTime TimeStamp { get; set; }

        [Column]
        public string Title { get; set; }

        #endregion
    }
}