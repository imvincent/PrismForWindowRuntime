// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;

namespace Kona.Infrastructure.Flyouts
{
    public class FlyoutView : Page
    {
        private Popup _popup;

        public string CommandId { get; private set; }

        public string CommandTitle { get; private set; }
        
        public int FlyoutSize { get; private set; }

        public FlyoutView(string commandId, string commandTitle, int flyoutSize)
        {
            this.CommandId = commandId;
            this.CommandTitle = commandTitle;
            this.FlyoutSize = flyoutSize;
        }

        public void Open()
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

            // Place the Flyout inside the Popup
            _popup.Child = this;
            _popup.IsOpen = true;

            var viewModel = this.DataContext as IFlyoutViewModel;
            if (viewModel != null)
            {
                viewModel.Open();
            }
        }

        public void Close()
        {
            _popup.IsOpen = false;
        }

        public void GoBack()
        {
            SettingsPane.Show();
            Close();
        }

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
    }
}
