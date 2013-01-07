// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

namespace Kona.Infrastructure
{
    public class FrameNavigationService : INavigationService
    {
        private readonly Frame frame;
        private IFrameSessionState frameSessionState;
        private Func<string, Type> navigationResolver;

        public FrameNavigationService(Frame frame, IFrameSessionState frameSessionState, Func<string, Type> navigationResolver)
        {
            this.frame = frame;
            this.frameSessionState = frameSessionState;
            this.navigationResolver = navigationResolver;

            this.frame.Navigating += frame_Navigating;
            this.frame.Navigated += frame_Navigated;
        }

        private void frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            NavigateFromCurrentViewModel(false);
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {
            NavigateToCurrentViewModel(e.NavigationMode, e.Parameter);
        }

        public void Suspending()
        {
            NavigateFromCurrentViewModel(true);
        }

        public void RestoreSavedNavigation()
        {
            NavigateToCurrentViewModel(NavigationMode.Refresh, null);
        }

        private void NavigateToCurrentViewModel(NavigationMode navigationMode, object parameter)
        {
            var newView = frame.Content as FrameworkElement;
            if (newView == null) return;
            var frameState = this.frameSessionState.GetSessionStateForFrame(frame);
            var newViewModel = newView.DataContext as INavigationAware;
            if (newViewModel != null)
                newViewModel.OnNavigatedTo(parameter, navigationMode, frameState);
        }

        private void NavigateFromCurrentViewModel(bool suspending)
        {
            var departingView = frame.Content as FrameworkElement;
            if (departingView == null) return;
            var frameState = this.frameSessionState.GetSessionStateForFrame(frame);
            var departingViewModel = departingView.DataContext as INavigationAware;
            if (departingViewModel != null)
                departingViewModel.OnNavigatedFrom(frameState, suspending);
        }

        public bool Navigate(string pageToken, object parameter)
        {
            Type pageType = this.navigationResolver(pageToken);
            return frame.Navigate(pageType, parameter);
        }

        public void GoBack()
        {
            frame.GoBack();
        }

        public bool CanGoBack()
        {
            return frame.CanGoBack;
        }
    }
}
