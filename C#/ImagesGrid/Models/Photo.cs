#region Using Directives

using System.Windows.Media.Imaging;

#endregion
namespace ImagesGrid.Models
{
 

    public class Photo : ImageCard
    {
        #region Public Properties

        public BitmapImage Image { get; set; }

        #endregion
    }
}