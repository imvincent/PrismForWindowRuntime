// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using HelloWorld.Services;
using HelloWorld.ViewModels;
using HelloWorld.Views;
using Microsoft.Practices.Prism.Mvvm;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

// This QuickStart is documented at http://go.microsoft.com/fwlink/?LinkID=288829&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        // Declare any app services that you want to hold on to as singletons
        IDataRepository _dataRepository;
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Required override. Generally you do your initial navigation to launch page, or 
        /// to the page approriate based on a search, sharing, or secondary tile launch of the app
        /// </summary>
        /// <param name="args">The launch arguments passed to the application</param>
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            // Use the logical name for the view to navigate to. The default convention
            // in the NavigationService will be to append "Page" to the name and look 
            // for that page in a .Views child namespace in the project. IF you want another convention
            // for mapping view names to view types, you can override 
            // the MvvmAppBase.GetPageNameToTypeResolver method
            NavigationService.Navigate("Main", null);
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// This is the place you initialize your services and set default factory or default resolver for the view model locator
        /// </summary>
        /// <param name="args">The same launch arguments passed when the app starts.</param>
        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            // New up the singleton data repository, and pass it the state service it depends on from the base class
            _dataRepository = new DataRepository(SessionStateService);

            // Register factory methods for the ViewModelLocator for each view model that takes dependencies so that you can pass in the
            // dependent services from the factory method here.
            ViewModelLocationProvider.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(_dataRepository, NavigationService));
            ViewModelLocationProvider.Register(typeof(UserInputPage).ToString(), () => new UserInputPageViewModel(_dataRepository, NavigationService));
            
            return Task.FromResult<object>(null);
        }
    }
}
