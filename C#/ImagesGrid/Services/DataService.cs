namespace ImagesGrid.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Media.Imaging;

    using ImagesGrid.Models;
    using ImagesGrid.Repository;
    using ImagesGrid.StaticMembers;

    #endregion

    public static class DataService
    {
        #region Static Fields

        private static Repository<Card> cardRepository = new Repository<Card>();

        #endregion

        #region Public Methods and Operators

        public static BitmapImage FetchImage(Photo photo)
        {
            BitmapImage image = null;

            // AutoResetEvent bitmapInitializationEvt = new AutoResetEvent(false);
            image = new BitmapImage();

            using (var imageStream = LoadImage(photo.ImageSource))
            {
                if (imageStream != null)
                {
                    image.SetSource(imageStream);
                }
            }

            // bitmapInitializationEvt.WaitOne();
            return image;
        }

        public static async Task<List<Card>> GetCards()
        {
            var cards = cardRepository.GetAll().ToList();
            foreach (var card in cards)
            {
                card.Image = GetImage(card.Image.ImageSource);
            }

            return cards;
        }

        public static Photo GetImage(string imgName)
        {
            DateTime start = new DateTime(2010, 1, 1);

            Photo imageData = new Photo { ImageSource = imgName, Title = imgName, TimeStamp = start };

            imageData.Image = FetchImage(imageData);
            return imageData;
        }

        public static async Task<List<Photo>> GetImages()
        {
            List<Photo> imageList = new List<Photo>();
            DateTime start = new DateTime(2010, 1, 1);
            foreach (var imgName in GetImagesNamesList(true))
            {
                Photo imageData = new Photo { ImageSource = imgName, Title = imgName, TimeStamp = start };
                imageData.Image = FetchImage(imageData);
                imageList.Add(imageData);
            }

            return imageList;
        }

        public static List<string> GetImagesNamesList(bool withPath)
        {
            IsolatedStorageFile storeFile = IsolatedStorageFile.GetUserStoreForApplication();
            string imageFolder = Consts.ImageFolder;
            if (!storeFile.DirectoryExists(Consts.ImageFolderSlash))
            {
                storeFile.CreateDirectory(Consts.ImageFolderSlash);
            }

            List<string> fileList = new List<string>(storeFile.GetFileNames(Consts.ImageFolderSlash));
            List<string> imgNameList = new List<string>();
            if (withPath)
            {
                foreach (string file in fileList)
                {
                    imgNameList.Add(Path.Combine(imageFolder, file));
                }
            }
            else
            {
                return fileList;
            }

            return imgNameList;
        }


        private static Stream LoadImage(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentException("one of parameters is null");
            }

            Stream stream = null;

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists(filename))
                {
                    stream = isoStore.OpenFile(filename, FileMode.Open, FileAccess.Read);
                }
            }

            return stream;
        }

        #endregion
    }
}