// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using HelloWorldWithContainer.Services;
using Kona.Infrastructure;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Unity;
using Kona.Infrastructure.Interfaces;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace HelloWorldWithContainer
{
    sealed partial class App : MvvmAppBase
    {
        // New up the singleton container that will be used for type resolution in the app
        IUnityContainer _container = new UnityContainer();
        public App()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Required override. Generally you do your initial navigation to launch page, or 
        /// to the page approriate based on a search, sharing, or secondary tile launch of the app
        /// </summary>
        /// <param name="args">The launch arguments passed to the application</param>
        public override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            // Use the logical name for the view to navigate to. The default convention
            // in the NavigationService will be to append "Page" to the name and look 
            // for that page in a .Views child namespace in the project. IF you want another convention
            // for mapping view names to view types, you can override 
            // the KonaApplication.GetPageNameToTypeResolver method
            NavigationService.Navigate("Main", null);
        }

        /// <summary>
        /// This is the place you initialize your services and set default factory or default resolver for the view model locator
        /// </summary>
        /// <param name="args">The same launch arguments passed when the app starts.</param>
        public override void OnInitialize(IActivatedEventArgs args)
        {
            // Register the two KonaApplication services with the container so that view models can take dependencies on them
            _container.RegisterInstance<IRestorableStateService>(RestorableStateService);
            _container.RegisterInstance<INavigationService>(NavigationService);
            // Register any app specific types with the container
            _container.RegisterType<IDataRepository, DataRepository>();

            // Set a factory for the ViewModelLocator to use the container to construct view models so their 
            // dependencies get injected by the container
            ViewModelLocator.SetDefaultViewModelFactory((viewModelType) => _container.Resolve(viewModelType));
        }
    }
}
