// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Kona.AWShopper.Common;
using Kona.Infrastructure;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Kona.AWShopper
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private IAccountService _accountService;
        private ISettingsCharmService _settingsCharmService;
        private FrameNavigationService _navigationService;
        private IShoppingCartRepository _shoppingCartRepository;
        private IRestorableStateService _stateService;
        private ICredentialStore _credentialStore;

        public bool IsSuspending { get; private set; }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                //Associate the frame with a SuspensionManager key                                
                SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            _settingsCharmService = CreateSettingsCharmService();
            _stateService = new RestorableStateService();
            _credentialStore = new RoamingCredentialStore();
            _accountService = CreateAccountService(_settingsCharmService, _stateService, _credentialStore);
            _navigationService = CreateNavigationService(rootFrame);
            _shoppingCartRepository = new ShoppingCartRepository(new ShoppingCartServiceProxy(), _accountService);

            BootstrapApplication(_navigationService);

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state and navigate to the last page visited
                try
                {
                    await SuspensionManager.RestoreAsync();
                    _navigationService.RestoreSavedNavigation();
                    _stateService.SetFrameState(SuspensionManager.SessionStateForFrame(rootFrame));
                    _stateService.RaiseAppRestored();
                }
                catch (SuspensionManagerException)
                {
                    // Something went wrong restoring state.
                    // Assume there is no state and continue
                }
            }
            else
                _stateService.SetFrameState(SuspensionManager.SessionStateForFrame(rootFrame));

            // <snippet404>
            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!_navigationService.Navigate("Hub", args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // </snippet404>

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            IsSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                _navigationService.Suspending();

                // Save application state
                await SuspensionManager.SaveAsync();

                //TODO: Stop any background activity
                deferral.Complete();
            }
            finally
            {
                IsSuspending = false;
            }

        }
    }
}
