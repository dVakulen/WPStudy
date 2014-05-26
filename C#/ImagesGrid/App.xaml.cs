#region Using Directives

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

using ImagesGrid.ViewModels;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

#endregion
namespace PhotoHubSample
{
   

    public partial class App : Application
    {
        #region Static Fields

        private static ImagesViewModel viewModel;

        #endregion

        #region Fields

        private bool phoneApplicationInitialized;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            this.UnhandledException += this.Application_UnhandledException;

            // Standard Silverlight initialization
            this.InitializeComponent();

            // Phone-specific initialization
            this.InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (Debugger.IsAttached)
            {
                // Display the current frame rate counters
                Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                // Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                // Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

            this.DbInitialize();
            this.StartTime = Environment.TickCount;
        }

        #endregion

        #region Public Properties

        public static ImagesViewModel ViewModel
        {
            get
            {
                if (viewModel == null)
                {
                    viewModel = new ImagesViewModel();
                }

                return viewModel;
            }
        }

        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        public long StartTime { get; set; }

        #endregion

        #region Public Methods and Operators

        public void OutputTimestamp(string location)
        {
            {
                // if (location == "TestOne_Loaded")
                long end = Environment.TickCount;
                Debug.WriteLine("{0}{1}", location, end - this.StartTime);
            }
        }

        #endregion

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        #region Methods

        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // Ensure that required application state is persisted here.
        }

        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // Code to execute if a navigation fails

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        // Avoid double-initialization

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (this.RootVisual != this.RootFrame)
            {
                this.RootVisual = this.RootFrame;
            }

            // Remove this handler since it is no longer needed
            this.RootFrame.Navigated -= this.CompleteInitializePhoneApplication;
        }

        private void DbInitialize()
        {
            // z.SubmitChanges();
        }

        private void InitializePhoneApplication()
        {
            if (this.phoneApplicationInitialized)
            {
                return;
            }

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            this.RootFrame = new PhoneApplicationFrame();
            this.RootFrame.Navigated += this.CompleteInitializePhoneApplication;

            // Handle navigation failures
            this.RootFrame.NavigationFailed += this.RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            this.phoneApplicationInitialized = true;
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                Debugger.Break();
            }
        }

        #endregion
    }
}