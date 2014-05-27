#region Using Directives

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

using ImagesGrid.Models;
using ImagesGrid.Repository;
using ImagesGrid.Services;
using ImagesGrid.ViewModels;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using GestureEventArgs = System.Windows.Input.GestureEventArgs;

#endregion
namespace PhotoHubSample
{
    using System.Data.Linq;
    using System.Linq;

    public partial class MainPage : PhoneApplicationPage
    {
        #region Fields

        private BackgroundWorker bWorker = new BackgroundWorker();

        private Repository<Team> teamRepository = new Repository<Team>();
        private Stopwatch watch;

        #endregion

        #region Constructors and Destructors
        void TeamButton_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show((sender as Button).Content.ToString());
            if (sender is Button)
            {
                this.dataContext.CurrentTeam = ((sender as Button).DataContext as Team);
                this.NavigationService.Navigate(new Uri("/TeamManagementPage.xaml", UriKind.Relative));
            }
        }
        void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var lastTeam = teamRepository.GetAll().OrderByDescending(c => c.Number).FirstOrDefault();
            var team = new Team
                           {
                               Id = Guid.NewGuid(),
                               Number = ++lastTeam.Number,
                               UserCardInTeams = new EntitySet<CardInTeam>()
                           };
            teamRepository.Insert(team);
            this.TeamsStackPanel.Children.RemoveAt(this.TeamsStackPanel.Children.Count - 1);
            AddTeamButton((team));
            AddAddButton();
            //  MessageBox.Show(lastTeam.Number.ToString());
        }

        private void AddTeamButton(Team team)
        {
            var TeamBtn = new Button();
            TeamBtn.Click += TeamButton_Click;
            TeamBtn.Content = team.Number;
            TeamBtn.DataContext = team;
            this.TeamsStackPanel.Children.Add(TeamBtn);
        }

        private void AddAddButton()
        {
            var AddButton = new Button();
            AddButton.Click += AddButton_Click;
            AddButton.Content = "+";
            this.TeamsStackPanel.Children.Add(AddButton);
        }
        public MainPage()
        {
            this.InitializeComponent();
            this.dataContext = App.ViewModel;
            this.Loaded += this.MainPage_Loaded;
            this.LayoutUpdated += this.TestOne_LayoutUpdated;
            this.Loaded += this.TestOne_Loaded;
            this.bWorker.WorkerReportsProgress = false;
            this.bWorker.WorkerSupportsCancellation = false;
            this.bWorker.DoWork += this.bw_DoWork;
            this.bWorker.RunWorkerCompleted += this.bw_RunWorkerCompleted;
            var z = teamRepository.GetAll();
            foreach (var team in z)
            {
                AddTeamButton(team);
            }
            AddAddButton();
            this.pivot.IsLocked = dataContext.IsInSelectingCardToTeam;

        }

        #endregion

        #region Properties

        private ImagesViewModel dataContext { get; set; }

        #endregion

        #region Methods

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            this.RunWorker();
        }

        private void AddNew_KeyDown(object sender, KeyEventArgs e)
        {
            this.dataContext.CurrentCard = new Card();
            this.NavigationService.Navigate(new Uri("/CardPage.xaml", UriKind.Relative));
        }

        private void AddNew_Tap(object sender, GestureEventArgs e)
        {
            this.dataContext.IsCardNew = true;
            this.dataContext.CurrentCard = new Card();
            this.NavigationService.Navigate(new Uri("/CardPage.xaml", UriKind.Relative));
        }

        private void ImagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (sender is LongListSelector)
            {
                var sendr = sender as LongListSelector;

                if (sendr.SelectedItem == null)
                {
                    return;
                }
                if (!dataContext.IsInSelectingCardToTeam)
                {
                    this.dataContext.CurrentCard = sendr.SelectedItem as Card;
                   
                    this.dataContext.IsCardNew = false;
                    sendr.SelectedItem = null;
                    this.NavigationService.Navigate(new Uri("/CardPage.xaml", UriKind.Relative));
                }
                else
                {
                    var card = sendr.SelectedItem as Card;
                    if (card == null) return;
                    dataContext.CurrentTeam.UserCardInTeams.Add(new CardInTeam
                                                                    {
                                                                        Attack = card.Attack,
                                                                        Team =  dataContext.CurrentTeam,
                                                                        Image = card.Image,
                                                                        Id=  Guid.NewGuid(),
                                                                        Attribute = card.Attribute,
                                                                        Name = card.Name,
                                                                        LargeImageName =  card.LargeImageName
                                                                    });
                    dataContext.IsInSelectingCardToTeam = false;
                    this.NavigationService.Navigate(new Uri("/TeamManagementPage.xaml", UriKind.Relative));
                }

            }

        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = App.ViewModel;
            this.DataContext = viewModel;
            this.dataContext = viewModel;
            if (this.ImagesList.ItemsSource != null && this.ImagesList.ItemsSource.Count > 0)
            {
                this.ImagesList.ScrollTo(this.ImagesList.ItemsSource[this.ImagesList.ItemsSource.Count - 1]);
            }
        }

        private void OrderByChange(object sender, EventArgs e)
        {
            this.dataContext.IsCardNew = true;
            if (!(sender is ListPicker))
            {
                return;
            }

            if ((sender as ListPicker).SelectedIndex == 1)
            {
                this.dataContext.OrderByAttack = true;
            }
            else
            {
                this.dataContext.OrderByAttack = false;
            }
        }

        private void RunWorker()
        {
            if (this.bWorker.IsBusy != true)
            {
                this.bWorker.RunWorkerAsync();
            }
        }

        private void SortBy_GotFocus(object sender, RoutedEventArgs e)
        {
        }

        private void SortBy_Tap(object sender, GestureEventArgs e)
        {
        }

        private void TestOne_LayoutUpdated(object sender, EventArgs e)
        {

            ((App)Application.Current).OutputTimestamp("TestOne_LayoutUpdated");
        }

        private void TestOne_Loaded(object sender, RoutedEventArgs e)
        {
            this.setLoadingIndicator();
            ((App)Application.Current).OutputTimestamp("TestOne_Loaded");
        }

        private async void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    this.dataContext.IsLoading = true;
                    this.watch = Stopwatch.StartNew();
                    this.dataContext.Cards = DataService.GetCards().Result;
                    this.dataContext.photos = DataService.GetImages().Result;
                });
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(
                () =>
                {
                    this.watch.Stop();
                    this.dataContext.IsLoading = false;
                    this.StatisticsTextBox.Text = string.Format(
                        "Loaded {0} Iimages in {1} ms",
                        this.dataContext.CardsCount,
                        this.watch.Elapsed);
                });
        }


        private void setLoadingIndicator()
        {
            var progressIndicator = SystemTray.ProgressIndicator;
            if (progressIndicator != null)
            {
                return;
            }

            progressIndicator = new ProgressIndicator();

            SystemTray.SetProgressIndicator(this, progressIndicator);

            Binding binding = new Binding("IsLoading") { Source = this.DataContext };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsVisibleProperty, binding);

            binding = new Binding("IsLoading") { Source = this.DataContext };
            BindingOperations.SetBinding(progressIndicator, ProgressIndicator.IsIndeterminateProperty, binding);

            progressIndicator.Text = "Loading images...";
        }

        #endregion

        private void Image_Tap(object sender, GestureEventArgs e)
        {
        }
    }
}