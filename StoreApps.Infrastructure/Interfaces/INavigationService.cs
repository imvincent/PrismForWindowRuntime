// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Microsoft.Practices.StoreApps.Infrastructure.Interfaces
{
    /// <summary>
    /// The INavigationService interface is used for creating a navigation services for your Windows Store application.
    /// The default implementation of INavigationService is the FrameNavigationService class, that uses a class that implements the IFrameFacade interface
    /// to provide page navigation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// Navigates to the page with the specified page token, passing the specified parameter.
        /// </summary>
        /// <param name="pageToken">The page token.</param>
        /// <param name="parameter">The parameter.</param>
        /// <returns>Returns <c>true</c> if navigation succeds; otherwise, <c>false</c></returns>
        bool Navigate(string pageToken, object parameter);

        /// <summary>
        /// Goes to the previous page in the navigation stack.
        /// </summary>
        void GoBack();

        /// <summary>
        /// Determines whether the navigation service can navigate to the previous page or not.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the navigation service can go back; otherwise, <c>false</c>.
        /// </returns>
        bool CanGoBack();

        /// <summary>
        /// Clears the navigation history.
        /// </summary>
        void ClearHistory();

        /// <summary>
        /// Restores the saved navigation.
        /// </summary>
        void RestoreSavedNavigation();

        /// <summary>
        /// Used for navigating away from the current view model due to a suspension event, in this way you can execute additional logic to handle suspensions.
        /// </summary>
        void Suspending();
    }
}
