// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.PubSubEvents;
using System;
using System.Globalization;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace EventAggregatorQuickstart
{
    public class Bootstrapper
    {
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Make sure this is called after the UI thread is established
        /// </summary>
        /// <param name="navService"></param>
        public void Bootstrap(INavigationService navService)
        {
            // Create the singleton EventAggregator so it can be dependency injected down to the view models who need it
            _eventAggregator  = new EventAggregator();
            ViewModelLocator.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(_eventAggregator));
        }

        public INavigationService CreateNavigationService(IFrameFacade rootFrame, ISessionStateService sessionStateService)
        {
            Func<string, Type> navigationResolver = (string pageToken) =>
            {
                // We set a custom namespace for the View
                var viewNamespace = "EventAggregatorQuickstart";

                var viewFullName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}Page", viewNamespace, pageToken);
                var viewType = Type.GetType(viewFullName);

                return viewType;
            };

            var navigationService = new FrameNavigationService(rootFrame, navigationResolver, sessionStateService);
            return navigationService;
        }
    }
}
