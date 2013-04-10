// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Search;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using AdventureWorks.Shopper.Services;

namespace AdventureWorks.Shopper
{
    /// <summary>
    /// This class uses the MvvmAppBase class to bootstrap this Windows Store App with Mvvm support
    /// http://go.microsoft.com/fwlink/?LinkID=288809&clcid=0x409
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        // Create the singleton container that will be used for type resolution in the app
        private readonly IUnityContainer _container = new UnityContainer();

        //Bootstrap: App singleton service declarations
        private IEventAggregator _eventAggregator;
        private TileUpdater _tileUpdater;

        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Dark;
        }

        // Documentation on navigation between pages is at http://go.microsoft.com/fwlink/?LinkID=288815&clcid=0x409
        protected override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            if (args != null && !string.IsNullOrEmpty(args.Arguments))
            {
                // The app was launched from a Secondary Tile
                // Navigate to the item's page
                NavigationService.Navigate("ItemDetail", args.Arguments);
            }
            else
            {
                // Navigate to the initial page
                NavigationService.Navigate("Hub", null);
            }
        }

        // Documentation on using search can be found at http://go.microsoft.com/fwlink/?LinkID=288822&clcid=0x409
        protected override void OnSearchApplication(SearchQueryArguments args)
        {
            if (args != null && !string.IsNullOrEmpty(args.QueryText))
            {
                NavigationService.Navigate("SearchResults", args.QueryText);
            }
            else
            {
                NavigationService.Navigate("Hub", null);
            }
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            // Set up the list of known types for the SuspensionManager
            SessionStateService.RegisterKnownType(typeof(Address));
            SessionStateService.RegisterKnownType(typeof(PaymentMethod));
            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(CheckoutDataViewModel));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<CheckoutDataViewModel>));
            SessionStateService.RegisterKnownType(typeof(ShippingMethod));
            SessionStateService.RegisterKnownType(typeof(ReadOnlyDictionary<string, ReadOnlyCollection<string>>));
            SessionStateService.RegisterKnownType(typeof(Order));
            SessionStateService.RegisterKnownType(typeof(Product));
            SessionStateService.RegisterKnownType(typeof(ReadOnlyCollection<Product>));
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _eventAggregator = new EventAggregator();

            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<IFlyoutService>(FlyoutService);
            _container.RegisterInstance<IEventAggregator>(_eventAggregator);
            _container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheService, TemporaryFolderCacheService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISecondaryTileService, SecondaryTileService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISearchPaneService, SearchPaneService>(new ContainerControlledLifetimeManager());

            // Register repositories
            _container.RegisterType<IProductCatalogRepository, ProductCatalogRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartRepository, ShoppingCartRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICheckoutDataRepository, CheckoutDataRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager());

            // Register web service proxies
            _container.RegisterType<IProductCatalogService, ProductCatalogServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderService, OrderServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartService, ShoppingCartServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShippingMethodService, ShippingMethodServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocationService, LocationServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddressService, AddressServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPaymentMethodService, PaymentMethodServiceProxy>(new ContainerControlledLifetimeManager());

            // Register child view models
            _container.RegisterType<IShippingAddressUserControlViewModel, ShippingAddressUserControlViewModel>();
            _container.RegisterType<IBillingAddressUserControlViewModel, BillingAddressUserControlViewModel>();
            _container.RegisterType<IPaymentMethodUserControlViewModel, PaymentMethodUserControlViewModel>();
            _container.RegisterType<ISignInUserControlViewModel, SignInUserControlViewModel>();

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "AdventureWorks.UILogic.ViewModels.{0}ViewModel, AdventureWorks.UILogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=634ac3171ee5190a", viewType.Name);
                    var viewModelType = Type.GetType(viewModelTypeName);
                    return viewModelType;
                });

            // Documentation on working with tiles can be found at http://go.microsoft.com/fwlink/?LinkID=288821&clcid=0x409
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            _tileUpdater.StartPeriodicUpdate(new Uri(Constants.ServerAddress + "/api/TileNotification"), PeriodicUpdateRecurrence.HalfHour);

            var resourceLoader = _container.Resolve<IResourceLoader>();
            SearchPane.GetForCurrentView().PlaceholderText = resourceLoader.GetString("SearchPanePlaceHolderText");
        }

        protected override object Resolve(Type type)
        {
            // Use the container to resolve types (e.g. ViewModels and Flyouts)
            // so their dependencies get injected
            return _container.Resolve(type);
        }

        protected override IList<SettingsCharmActionItem> GetSettingsCharmActionItems()
        {
            var settingsCharmItems = new List<SettingsCharmActionItem>();
            var accountService = _container.Resolve<IAccountService>();
            var resourceLoader = _container.Resolve<IResourceLoader>();

            if (accountService.SignedInUser == null)
            {
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("LoginText"), () => FlyoutService.ShowFlyout("SignIn")));
            }
            else
            {
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("LogoutText"), () => FlyoutService.ShowFlyout("SignOut")));
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("AddShippingAddressTitle"), () => NavigationService.Navigate("ShippingAddress", null)));
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("AddBillingAddressTitle"), () => NavigationService.Navigate("BillingAddress", null)));
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("AddPaymentMethodTitle"), () => NavigationService.Navigate("PaymentMethod", null)));
                settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("ChangeDefaults"), () => FlyoutService.ShowFlyout("ChangeDefaults")));
            }
            settingsCharmItems.Add(new SettingsCharmActionItem(resourceLoader.GetString("PrivacyPolicy"), async () => await Launcher.LaunchUriAsync(new Uri(resourceLoader.GetString("PrivacyPolicyUrl")))));

            return settingsCharmItems;
        }
    }
}
