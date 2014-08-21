// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Prism.Mvvm;

// The Grid App template is documented at http://go.microsoft.com/fwlink/?LinkId=234226

namespace ExtendedSplashScreen
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        /// <summary>
        /// Initializes the singleton Application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            // Here we set the factory that will be used by Prism to create the extended splash screen.
            // During the initial setup, the MvvmAppBase class will check this property and if it was set, 
            // it will show the extended splash automatically.
            this.ExtendedSplashScreenFactory = (splashscreen) => new ExtendedSplashScreen(splashscreen);
        }

        protected override async Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                // Here we would load the application's resources.
                await this.LoadAppResources();
            }

            this.NavigationService.Navigate("GroupedItems", null);
        }
        /// <summary>
        /// We use this method to simulate the loading of resources from different sources asynchronously.
        /// </summary>
        /// <returns></returns>
        private async Task LoadAppResources()
        {
            await Task.Delay(7000);
        }
    }
}
