// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.StoreApps.Infrastructure.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.StoreApps.Infrastructure.Tests
{
    [TestClass]
    public class FrameNavigationServiceFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        public async Task Navigate_To_Valid_Page()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                bool result = navigationService.Navigate("Mock", 1);

                Assert.IsTrue(result);
                Assert.IsNotNull(frame.Content);
                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
            });
        }

        [TestMethod]
        public async Task Navigate_To_Invalid_Page()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();

                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(string), sessionStateService);

                bool result = navigationService.Navigate("Mock", 1);

                Assert.IsFalse(result);
                Assert.IsNull(frame.Content);
            });
        }

        [TestMethod]
        public async Task NavigateToCurrentViewModel_Calls_VieModel_OnNavigatedTo()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());

                bool viewModelNavigatedToCalled = false;
                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (a, b) => Assert.IsTrue(true);
                viewModel.OnNavigatedToCommand = (parameter, navigationMode, frameState) =>
                {
                    Assert.AreEqual(NavigationMode.New, navigationMode);
                    viewModelNavigatedToCalled = true;
                };

                // Set up the viewModel to the Page we navigated
                frame.Navigated += (sender, e) =>
                {
                    var view = frame.Content as FrameworkElement;
                    view.DataContext = viewModel;
                };

                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();

                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                navigationService.Navigate("Mock", 1);

                Assert.IsTrue(viewModelNavigatedToCalled);
            });
        }

        [TestMethod]
        public async Task NavigateFromCurrentViewModel_Calls_VieModel_OnNavigatedFrom()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());

                bool viewModelNavigatedFromCalled = false;
                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (frameState, suspending) =>
                {
                    Assert.IsFalse(suspending);
                    viewModelNavigatedFromCalled = true;
                };

                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();

                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                // Initial navigatio
                navigationService.Navigate("page0", 0);

                // Set up the frame's content with a Page
                var view = new MockPage();
                view.DataContext = viewModel;
                frame.Content = view;

                // Navigate to fire the NavigatedToCurrentViewModel
                navigationService.Navigate("page1", 1);

                Assert.IsTrue(viewModelNavigatedFromCalled);
            });
        }

        [TestMethod]
        public async Task Suspending_Calls_VieModel_OnNavigatedFrom()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                navigationService.Navigate("Mock", 1);

                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedFromCommand = (frameState, suspending) =>
                {
                    Assert.IsTrue(suspending);
                };

                var page = (MockPage)frame.Content;
                page.DataContext = viewModel;

                navigationService.Suspending();
            });
        }

        [TestMethod]
        public async Task Resuming_Calls_ViewModel_OnNavigatedTo()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                navigationService.Navigate("Mock", 1);

                var viewModel = new MockPageViewModel();
                viewModel.OnNavigatedToCommand = (navigationParameter, navigationMode, frameState) =>
                {
                    Assert.AreEqual(NavigationMode.Refresh, navigationMode);
                };

                var page = (MockPage)frame.Content;
                page.DataContext = viewModel;

                navigationService.RestoreSavedNavigation();
            });
        }

        [TestMethod]
        public async Task GoBack_When_CanGoBack()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => new Dictionary<string, object>();
                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPage), sessionStateService);

                bool resultFirstNavigation = navigationService.Navigate("MockPage", 1);

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
                Assert.IsFalse(navigationService.CanGoBack());

                bool resultSecondNavigation = navigationService.Navigate("Mock", 2);

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(2, ((MockPage)frame.Content).PageParameter);
                Assert.IsTrue(navigationService.CanGoBack());

                navigationService.GoBack();

                Assert.IsInstanceOfType(frame.Content, typeof(MockPage));
                Assert.AreEqual(1, ((MockPage)frame.Content).PageParameter);
                Assert.IsFalse(navigationService.CanGoBack());
            });
        }

        [TestMethod]
        public async Task ViewModelOnNavigatedFromCalled_WithItsOwnStateDictionary()
        {
            await ExecuteOnUIThread(() =>
            {
                var frame = new FrameFacadeAdapter(new Frame());
                var sessionStateService = new MockSessionStateService();
                var frameSessionState = new Dictionary<string, object>();
                sessionStateService.GetSessionStateForFrameDelegate = (currentFrame) => frameSessionState;
                var navigationService = new FrameNavigationService(frame, (pageToken) => typeof(MockPageWithViewModel), sessionStateService);

                navigationService.Navigate("Page1", 1);
                Assert.AreEqual(1, frameSessionState.Count, "VM 1 state only");

                navigationService.Navigate("Page2", 2);
                Assert.AreEqual(2, frameSessionState.Count, "VM 1 and 2");

                navigationService.Navigate("Page1", 1);
                Assert.AreEqual(3, frameSessionState.Count, "VM 1, 2, and 1 again.");

                navigationService.Navigate("Page3", 3);
                Assert.AreEqual(4, frameSessionState.Count, "VM 1, 2, 1, and 3");
            });
        }
    }
}
