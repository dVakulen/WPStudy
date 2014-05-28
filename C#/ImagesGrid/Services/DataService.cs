namespace ImagesGrid.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using ImagesGrid.Models;
    using ImagesGrid.Repository;
    using ImagesGrid.StaticMembers;

    #endregion

    public static class DataService
    {
        #region Static Fields

        private static Repository<Card> cardRepository = new Repository<Card>();

        private static Repository<CardInTeam> teamCardsRepository = new Repository<CardInTeam>();
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

        public static bool isCardInTeam(Card card)
        {
            return true;
            return
                teamCardsRepository.Where(
                    v => v.Name.Equals(card.Name) || v.Attribute == card.Attribute).Any();//v.Attack == card.Attack && v.Attribute == card.Attribute &&
        }
    

        private static BitmapImage AspectScale(BitmapImage img, int maxWidth, int maxHeigh)
        {
            double scaleRatio;

            if (img.PixelWidth > img.PixelHeight)
                scaleRatio = (maxWidth / (double)img.PixelWidth);
            else
                scaleRatio = (maxHeigh / (double)img.PixelHeight);

            var scaledWidth = img.PixelWidth * scaleRatio;
            var scaledHeight = img.PixelHeight * scaleRatio;

            using (var mem = new MemoryStream())
            {
                var wb = new WriteableBitmap(img);
                wb.SaveJpeg(mem, (int)scaledWidth, (int)scaledHeight, 0, 100);
                mem.Seek(0, SeekOrigin.Begin);
                var bn = new BitmapImage();
                bn.SetSource(mem);
                return bn;
            }
        }
        private static void DrawTeamMark(Photo photo)
        {
            Image img = new Image();
            img.Source = photo.Image;
            WriteableBitmap wb = new WriteableBitmap(img, null);

           

            BitmapImage img11 = new BitmapImage();
            img11.SetSource(
               Application.GetResourceStream(new Uri(@"Assets/Tiles/FlipCycleTileSmall.png", UriKind.Relative))
                   .Stream);
            img11 = AspectScale(img11, 50, 50);
            Image displayImg1 = new Image { Source = img11 };
              wb.Render(displayImg1, new TranslateTransform() { X = 115, Y = 5 });
            wb.Invalidate();
           
            
            using (MemoryStream ms = new MemoryStream())
            {
                wb.SaveJpeg(ms, 100, 100, 0, 100);

                photo.Image.SetSource(ms);
            }

        }
        public static async Task<List<Card>> GetCards()
        {
            var cards = cardRepository.GetAll().ToList();
            foreach (var card in cards)
            {
              
                if (isCardInTeam(card) && false)
                {
                    DateTime start = new DateTime(2010, 1, 1);
                    BitmapImage img = new BitmapImage();
                    img.SetSource(
                        Application.GetResourceStream(new Uri(@"Assets/Tiles/FlipCycleTileMedium.png", UriKind.Relative))
                            .Stream);
                    Photo imageData = new Photo { Image = img, ImageSource = @"Assets/Tiles/FlipCycleTileMedium.png", Title = "aaa", TimeStamp = start };
                    card.Image = imageData;
                }
                else
                {

                    card.Image = GetImage(card.Image.ImageSource);
                    if (isCardInTeam(card))
                    {
                    //    DrawTeamMark((Photo)card.Image);
                    }
                }
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