// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.System;
#if WINDOWS_APP
using Windows.UI.ApplicationSettings;
#endif
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Mvvm.Interfaces;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using AdventureWorks.Shopper.Services;
using AdventureWorks.Shopper.Views;
using AdventureWorks.UILogic;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.ViewModels;

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
        private TileUpdater _tileUpdater;

        public IEventAggregator EventAggregator { get; set; }

        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Dark;
        }

        // Documentation on navigation between pages is at http://go.microsoft.com/fwlink/?LinkID=288815&clcid=0x409
        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
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

            Window.Current.Activate();
            return Task.FromResult<object>(null);
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
            SessionStateService.RegisterKnownType(typeof(Dictionary<string, Collection<string>>));
            SessionStateService.RegisterKnownType(typeof(Order));
            SessionStateService.RegisterKnownType(typeof(Product));
            SessionStateService.RegisterKnownType(typeof(Collection<Product>));
        }

        protected override Task OnInitializeAsync(IActivatedEventArgs args)
        {
            EventAggregator = new EventAggregator();

            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<IEventAggregator>(EventAggregator);
            _container.RegisterInstance<IResourceLoader>(new Microsoft.Practices.Prism.StoreApps.ResourceLoaderAdapter(new ResourceLoader()));
            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheService, TemporaryFolderCacheService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISecondaryTileService, SecondaryTileService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());

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

            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "AdventureWorks.UILogic.ViewModels.{0}ViewModel, AdventureWorks.UILogic, Version=1.1.0.0, Culture=neutral", viewType.Name);
                    var viewModelType = Type.GetType(viewModelTypeName);
                    if (viewModelType == null)
                    {
                        viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "AdventureWorks.UILogic.ViewModels.{0}ViewModel, AdventureWorks.UILogic.Windows, Version=1.0.0.0, Culture=neutral", viewType.Name);
                        viewModelType = Type.GetType(viewModelTypeName);
                    }

                    return viewModelType;
                });
            // Documentation on working with tiles can be found at http://go.microsoft.com/fwlink/?LinkID=288821&clcid=0x409
            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            _tileUpdater.StartPeriodicUpdate(new Uri(Constants.ServerAddress + "/api/TileNotification"), PeriodicUpdateRecurrence.HalfHour);
            var resourceLoader = _container.Resolve<IResourceLoader>();

            return base.OnInitializeAsync(args);
        }

        protected override object Resolve(Type type)
        {
            // Use the container to resolve types (e.g. ViewModels and Flyouts)
            // so their dependencies get injected
            return _container.Resolve(type);
        }
#if WINDOWS_APP
        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var accountService = _container.Resolve<IAccountService>();
            var resourceLoader = _container.Resolve<IResourceLoader>();
            var eventAggregator = _container.Resolve<IEventAggregator>();

            if (accountService.SignedInUser == null)
            {
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("LoginText"), (c) => new SignInFlyout(eventAggregator).Show()));
            }
            else
            {
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("LogoutText"), (c) => new SignOutFlyout().Show()));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddShippingAddressTitle"), (c) => NavigationService.Navigate("ShippingAddress", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddBillingAddressTitle"), (c) => NavigationService.Navigate("BillingAddress", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddPaymentMethodTitle"), (c) => NavigationService.Navigate("PaymentMethod", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("ChangeDefaults"), (c) => new ChangeDefaultsFlyout().Show()));
            }
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("PrivacyPolicy"), async (c) => await Launcher.LaunchUriAsync(new Uri(resourceLoader.GetString("PrivacyPolicyUrl")))));
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("Help"), async (c) => await Launcher.LaunchUriAsync(new Uri(resourceLoader.GetString("HelpUrl")))));

            return settingsCommands;
        }
#endif
    }
}
