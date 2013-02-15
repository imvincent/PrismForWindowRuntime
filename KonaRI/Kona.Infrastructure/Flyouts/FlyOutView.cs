// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Kona.Infrastructure.Flyouts
{
    public class FlyoutView : Page
    {
        #region Fields
        private Popup _popup;
        #endregion

        #region Construction
        public FlyoutView(string commandId, string commandTitle, int flyoutSize)
        {
            CommandId = commandId;
            CommandTitle = commandTitle;
            FlyoutSize = flyoutSize;
        }

        #endregion

        #region Properties
        /// <summary>
        /// The command identifier used to invoke the flyout panel.
        /// </summary>
        public string CommandId
        {
            get { return (string)GetValue(CommandIdProperty); }
            set { SetValue(CommandIdProperty, value); }
        }

        /// <summary>
        /// The title presented inside of the flyout.
        /// </summary>
        public string CommandTitle
        {
            get { return (string)GetValue(CommandTitleProperty); }
            set { SetValue(CommandTitleProperty, value); }
        }

        /// <summary>
        /// The width of the flyout.
        /// </summary>
        public int FlyoutSize
        {
            get { return (int)GetValue(FlyoutSizeProperty); }
            set { SetValue(FlyoutSizeProperty, value); }
        }

        /// <summary>
        /// A flag to indicate whether to present a link in the settings flyout for presenting the flyout.
        /// </summary>
        public bool ExcludeFromSettingsPane
        {
            get { return (bool)GetValue(ExcludeFromSettingsPaneProperty); }
            set { SetValue(ExcludeFromSettingsPaneProperty, value); }
        }
        #endregion

        #region Dependency Properties
        /// <summary>
        /// DependencyProperty for CommandId.
        /// </summary>
        public static readonly DependencyProperty CommandIdProperty =
            DependencyProperty.Register("CommandId", typeof(string), typeof(FlyoutView), new PropertyMetadata(null));

        /// <summary>
        /// DependencyProperty for CommandTitle.
        /// </summary>
        public static readonly DependencyProperty CommandTitleProperty =
            DependencyProperty.Register("CommandTitle", typeof(string), typeof(FlyoutView), new PropertyMetadata(null));

        /// <summary>
        /// DependencyProperty for FlyoutSize.
        /// </summary>
        public static readonly DependencyProperty FlyoutSizeProperty =
            DependencyProperty.Register("FlyoutSize", typeof(int), typeof(FlyoutView), new PropertyMetadata(0));

        /// <summary>
        /// DependencyProperty for ExcludeFromSettingsPane.
        /// </summary>
        public static readonly DependencyProperty ExcludeFromSettingsPaneProperty =
            DependencyProperty.Register("ExcludeFromSettingsPane", typeof(bool), typeof(FlyoutView), new PropertyMetadata(false));
        #endregion

        #region Public Methods
        /// <summary>
        /// Called to present the flyout view.
        /// </summary>
        /// <param name="parameter">Optional parameter for the caller to pass into the view.</param>
        /// <param name="successAction">Method to be invoked on successful completion of the user task in the flyout.</param>
        public void Open(object parameter, Action successAction)
        {
            // Create a new Popup to display the Flyout
            _popup = new Popup();
            _popup.IsLightDismissEnabled = true;
            _popup.SetValue(Canvas.LeftProperty, Window.Current.Bounds.Width - FlyoutSize);
            _popup.SetValue(Canvas.TopProperty, 0);

            // Handle the Closed & Activated events of the Popup
            _popup.Closed += OnPopupClosed;
            Window.Current.Activated += OnWindowActivated;

            // Update the Flyout dimensions
            Width = FlyoutSize;
            Height = Window.Current.Bounds.Height;

            // Add animations for the panel.
            _popup.ChildTransitions = new TransitionCollection();
            _popup.ChildTransitions.Add(new PaneThemeTransition()
            {
                Edge = (SettingsPane.Edge == SettingsEdgeLocation.Right) ? EdgeTransitionLocation.Right : EdgeTransitionLocation.Left
            });

            // Place the Flyout inside the Popup and open it
            _popup.Child = this;
            _popup.IsOpen = true;

            // If the Flyout has a DataContext, call the viewModel.Open method and set the set the Close and GoBack actions for future use
            var viewModel = DataContext as IFlyoutViewModel;
            if (viewModel != null)
            {
                viewModel.CloseFlyout = Close;
                viewModel.GoBack = GoBack;
                viewModel.Open(parameter, successAction);
            }
        }

        /// <summary>
        /// Closes the flyout.
        /// </summary>
        public void Close()
        {
            _popup.IsOpen = false;
        }

        /// <summary>
        /// Handler for the GoBack button in the flyout to go back to the settings flyout if that is how the user got to this flyout.
        /// </summary>
        public void GoBack()
        {
            SettingsPane.Show();
            Close();
        }
        #endregion

        #region Private Methods
        // <snippet520>
        private void OnPopupClosed(object sender, object e)
        {
            _popup.Child = null;
            Window.Current.Activated -= OnWindowActivated;
        }

        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                Close();
            }
        }
        // </snippet520>
        #endregion
    }
}
