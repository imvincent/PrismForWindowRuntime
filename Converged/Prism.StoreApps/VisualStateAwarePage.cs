// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    /// <summary>
    /// This is the base class that can be used for pages that need to be aware of layout changes and update the visual state accordingly.
    /// </summary>
    public class VisualStateAwarePage : Page, IView
    {
        /// <summary>
        /// Name of default visual state
        /// </summary>
        public static readonly string DefaultLayoutVisualState = "DefaultLayout";

        /// <summary>
        /// Name of portraite visual state
        /// </summary>
        public static readonly string PortraitLayoutVisualState = "PortraitLayout";

        /// <summary>
        /// Name of Minimal or narrow visual state
        /// </summary>
        public static readonly string MinimalLayoutVisualState = "MinimalLayout";

        /// <summary>
        /// Width of page that should trigger Minimal visual state
        /// </summary>
        public int MinimalLayoutWidth { get; set; }

        /// <summary>
        /// Gets or sets the get session state for Frame.
        /// </summary>
        /// <value>
        /// The session state for the Frame.
        /// </value>
        public static Func<IFrameFacade, IDictionary<string, object>> GetSessionStateForFrame { get; set; }
 
        private List<Control> _visualStateAwareControls;

        /// <summary>
        /// Initializes a new instance of the <see cref="VisualStateAwarePage"/> class.
        /// </summary>
        public VisualStateAwarePage()
        {
            //Default Minimal layout width value to 500
            MinimalLayoutWidth = 500;

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            // When this page is part of the visual tree make two changes:
            // 1) Map application view state to visual state for the page
            // 2) Handle keyboard and mouse navigation requests
            this.Loaded += (sender, e) =>
            {
                this.StartLayoutUpdates(sender, e);

                // Keyboard and mouse navigation only apply when occupying the entire window
                if (this.ActualHeight == Window.Current.Bounds.Height &&
                    this.ActualWidth == Window.Current.Bounds.Width)
                {
                    // Listen to the window directly so focus isn't required
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                        CoreDispatcher_AcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed +=
                        this.CoreWindow_PointerPressed;
                }
            };

            // Undo the same changes when the page is no longer visible
            this.Unloaded += (sender, e) =>
            {
                this.StopLayoutUpdates(sender, e);
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                    CoreDispatcher_AcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -=
                    this.CoreWindow_PointerPressed;
            };
        }


        #region Navigation support

        DelegateCommand _goBackCommand;

        /// <summary>
        /// <see cref="DelegateCommand"/> used to bind to the back Button's Command property
        /// for navigating to the most recent item in back navigation history, if a Frame
        /// manages its own navigation history.
        /// 
        /// The <see cref="DelegateCommand"/> is set up to use the virtual method <see cref="GoBack"/>
        /// as the Execute Action and <see cref="CanGoBack"/> for CanExecute.
        /// </summary>
        public DelegateCommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new DelegateCommand(
                        () => this.GoBack(this, null),
                        () => this.CanGoBack());
                }
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
            }
        }
        /// <summary>
        /// Invoked as an event handler to navigate backward in the page's associated
        /// <see cref="Frame"/> until it reaches the top of the navigation stack.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="eventArgs">Event data describing the conditions that led to the event.</param>
        protected virtual void GoHome(object sender, RoutedEventArgs eventArgs)
        {
            // Use the navigation frame to return to the topmost page
            if (this.Frame != null)
            {
                while (this.Frame.CanGoBack) this.Frame.GoBack();
            }
        }

        /// <summary>
        /// Virtual method used by the <see cref="GoBackCommand"/> property
        /// to determine if the <see cref="Frame"/> can go back.
        /// </summary>
        /// <returns>
        /// true if the <see cref="Frame"/> has at least one entry 
        /// in the back navigation history.
        /// </returns>
        public virtual bool CanGoBack()
        {
            return this.Frame != null && this.Frame.CanGoBack;
        }

        /// <summary>
        /// Invoked as an event handler to navigate backward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="eventArgs">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoBack(object sender, RoutedEventArgs eventArgs)
        {
            // Use the navigation frame to return to the previous page
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
        }

        /// <summary>
        /// Invoked as an event handler to navigate forward in the navigation stack
        /// associated with this page's <see cref="Frame"/>.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="eventArgs">Event data describing the conditions that led to the
        /// event.</param>
        protected virtual void GoForward(object sender, RoutedEventArgs eventArgs)
        {
            // Use the navigation frame to move to the next page
            if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
        }

        /// <summary>
        /// Invoked on every keystroke, including system keys such as Alt key combinations, when
        /// this page is active and occupies the entire window. Used to detect keyboard navigation
        /// between pages even when the page itself doesn't have focus.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender,
            AcceleratorKeyEventArgs args)
        {
            var virtualKey = args.VirtualKey;

            // Only investigate further when Left, Right, or the dedicated Previous or Next keys
            // are pressed
            if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                args.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
                (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                (int)virtualKey == 166 || (int)virtualKey == 167))
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) ||
                    (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    // When the previous key or Alt+Left are pressed navigate back
                    args.Handled = true;
                    this.GoBack(this, new RoutedEventArgs());
                }
                else if (((int)virtualKey == 167 && noModifiers) ||
                    (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    // When the next key or Alt+Right are pressed navigate forward
                    args.Handled = true;
                    this.GoForward(this, new RoutedEventArgs());
                }
            }
        }

        /// <summary>
        /// Invoked on every mouse click, touch screen tap, or equivalent interaction when this
        /// page is active and occupies the entire window. Used to detect browser-style next and
        /// previous mouse button clicks to navigate between pages.
        /// </summary>
        /// <param name="sender">Instance that triggered the event.</param>
        /// <param name="args">Event data describing the conditions that led to the event.</param>
        private void CoreWindow_PointerPressed(CoreWindow sender,
            PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            // Ignore button chords with the left, right, and middle buttons
            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed) return;

            // If back or foward are pressed (but not both) navigate appropriately
            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;
                if (backPressed) this.GoBack(this, new RoutedEventArgs());
                if (forwardPressed) this.GoForward(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region Visual state switching

        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Loaded"/>
        /// event of a <see cref="Control"/> within the page, to indicate that the sender should
        /// start receiving visual state management changes that correspond to application view
        /// state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="eventArgs">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested. A corresponding
        /// <see cref="FrameworkElement.Unloaded"/> event handler connected to
        /// <see cref="StopLayoutUpdates"/> is strongly encouraged. Instances of
        /// <see cref="VisualStateAwarePage"/> automatically invoke these handlers in their Loaded and
        /// Unloaded events.</remarks>
        /// <seealso cref="DetermineVisualState"/>
        /// <seealso cref="InvalidateVisualState"/>
        public void StartLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._visualStateAwareControls == null)
            {
                // Start listening to view state changes when there are controls interested in updates
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._visualStateAwareControls = new List<Control>();
            }
            this._visualStateAwareControls.Add(control);

            // Set the initial visual state of the control
            VisualStateManager.GoToState(control, DetermineVisualState(this.ActualWidth, this.ActualHeight), false);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState(e.Size.Width, e.Size.Height);
        }

      
        /// <summary>
        /// Invoked as an event handler, typically on the <see cref="FrameworkElement.Unloaded"/>
        /// event of a <see cref="Control"/>, to indicate that the sender should start receiving
        /// visual state management changes that correspond to application view state changes.
        /// </summary>
        /// <param name="sender">Instance of <see cref="Control"/> that supports visual state
        /// management corresponding to view states.</param>
        /// <param name="eventArgs">Event data that describes how the request was made.</param>
        /// <remarks>The current view state will immediately be used to set the corresponding
        /// visual state when layout updates are requested.</remarks>
        /// <seealso cref="StartLayoutUpdates"/>
        public void StopLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null || this._visualStateAwareControls == null) return;
            this._visualStateAwareControls.Remove(control);
            if (this._visualStateAwareControls.Count == 0)
            {
                // Stop listening to view state changes when no controls are interested in updates
                this._visualStateAwareControls = null;
                Window.Current.SizeChanged -= this.WindowSizeChanged;
            }
        }

        /// <summary>
        /// Translates the app width and height values into strings for visual state
        /// management within the page. The default implementation uses the names of enum values.
        /// Subclasses may override this method to control the mapping scheme used.
        /// </summary>
        /// <returns>Visual state name used to drive the
        /// <see cref="VisualStateManager"/></returns>
        /// <seealso cref="InvalidateVisualState"/>
        protected virtual string DetermineVisualState(double width, double height)
        {
            if (width <= MinimalLayoutWidth) 
            { 
                return MinimalLayoutVisualState; 
            } 
            
            if (width < height) 
            { 
                return PortraitLayoutVisualState; 
            } 
            
            return DefaultLayoutVisualState; 
        }

        /// <summary>
        /// Updates all controls that are listening for visual state changes with the correct
        /// visual state.
        /// </summary>
        /// <remarks>
        /// Typically used in conjunction with overriding <see cref="DetermineVisualState"/> to
        /// signal that a different value may be returned even though the view state has not
        /// changed.
        /// </remarks>
        public void InvalidateVisualState(double width, double height)
        {
            if (this._visualStateAwareControls != null)
            {
                string visualState = DetermineVisualState(width, height);
                foreach (var layoutAwareControl in this._visualStateAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }
        #endregion

        #region Process lifetime management

        private String _pageKey;

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached. The Parameter
        /// property provides the group to be displayed.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");

            // Returning to a cached page through navigation shouldn't trigger state loading
            if (this._pageKey != null) return;

            var frameFacade = new FrameFacadeAdapter(this.Frame);
            var frameState = GetSessionStateForFrame(frameFacade);
            this._pageKey = "Page-" + frameFacade.BackStackDepth;

            if (e.NavigationMode == NavigationMode.New)
            {
                // Clear existing state for forward navigation when adding a new page to the
                // navigation stack
                var nextPageKey = this._pageKey;
                int nextPageIndex = frameFacade.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }

                // Pass the navigation parameter to the new page
                this.LoadState(e.Parameter, null);
            }
            else
            {
                // Pass the navigation parameter and preserved page state to the page, using
                // the same strategy for loading suspended state and recreating pages discarded
                // from cache
                this.LoadState(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]);
            }
        }

        /// <summary>
        /// Invoked when this page will no longer be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached. The Parameter
        /// property provides the group to be displayed.</param>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var frameFacade = new FrameFacadeAdapter(this.Frame);
            var frameState = GetSessionStateForFrame(frameFacade);
            var pageState = new Dictionary<String, Object>();
            this.SaveState(pageState);
            frameState[_pageKey] = pageState;
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session. This will be null the first time a page is visited.</param>
        protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SessionStateService.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected virtual void SaveState(Dictionary<String, Object> pageState)
        {
        }

        #endregion
    }
}
