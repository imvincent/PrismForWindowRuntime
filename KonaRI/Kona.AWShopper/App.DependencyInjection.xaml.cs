// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Kona.AWShopper.Common;
using Kona.AWShopper.Views;
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Services;
using Kona.UILogic.Repositories;
using Kona.UILogic.ViewModels;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace Kona.AWShopper
{
    sealed partial class App : Application
    {
        // <snippet403>
        private static FrameNavigationService CreateNavigationService(Frame frame)
        {
            var sessionStateWrapper = new FrameSessionStateWrapper();

            Func<string, Type> navigationResolver = (string pageToken) =>
            {
                // We set a custom namespace for the View
                var viewNamespace = "Kona.AWShopper.Views";

                var viewFullName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}Page", viewNamespace, pageToken);
                var viewType = Type.GetType(viewFullName);

                return viewType;
            };

            var navigationService = new FrameNavigationService(frame, sessionStateWrapper, navigationResolver);
            return navigationService;
        }
        // </snippet403>

        private static ISettingsCharmService CreateSettingsCharmService()
        {
            Func<IEnumerable<FlyoutView>> flyoutsFactory = () => new List<FlyoutView>()
                        {
                            new SignInFlyout("signIn", "Login"),
                            new SignOutFlyout("signOut", "Logout"),
                            new PaymentMethodFlyout("newPaymentMethod", "Add Payment Information"),
                            new BillingAddressFlyout("newBillingAddress", "Add Billing Address"),
                            new ShippingAddressFlyout("newShippingAddress", "Add Shipping Address"),
                            new EditPaymentMethodFlyout("editPaymentMethod", "Edit Payment Information") { ExcludeFromSettingsPane = true },
                            new EditShippingAddressFlyout("editShippingAddress", "Edit Shipping Address") { ExcludeFromSettingsPane = true },
                            new EditBillingAddressFlyout("editBillingAddress", "Edit Billing Address") { ExcludeFromSettingsPane = true },
                            new ChangeDefaultsFlyout("changeDefaults", "Change Defaults"),
                        };

            var settingsCharmService = new SettingsCharmService(flyoutsFactory);
            SettingsPane.GetForCurrentView().CommandsRequested += settingsCharmService.OnCommandsRequested;

            return settingsCharmService;
        }

        // <snippet506>
        private static AccountService CreateAccountService(IRestorableStateService stateService, ICredentialStore credentialStore)
        {
            var identityService = new IdentityServiceProxy();
            return new AccountService(identityService, stateService, credentialStore);
        }
        // </snippet506>

        // Bootstrap prior to building HubPage
        // This class is intended to be a central bootstrapping class that ties together views and view models.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling")]
        private void BootstrapApplication(INavigationService navService)
        {
            // Initialize service and repositories
            var checkoutDataRepository = new CheckoutDataRepository(new SettingsStoreService());
            var orderServiceProxy = new OrderServiceProxy();
            var shippingMethodServiceProxy = new ShippingMethodServiceProxy();

            // Set up the list of known types for the SuspensionManager
            SuspensionManager.KnownTypes.Add(typeof(Address));
            SuspensionManager.KnownTypes.Add(typeof(PaymentInfo));
            SuspensionManager.KnownTypes.Add(typeof(UserInfo));
            SuspensionManager.KnownTypes.Add(typeof(ObservableCollection<ShoppingCartItemViewModel>));
            SuspensionManager.KnownTypes.Add(typeof(ReadOnlyDictionary<string, ReadOnlyCollection<string>>));
           

            // Create Repositories
            // <snippet510>
            // </snippet510>

            // Update resolver because the ViewModels are located in a separate assembly than the Views. The ViewModel Types are in the Kona.UILogic assembly.
            // <snippet301>
            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                    {
                        var viewName = viewType.Name;
                        var viewModelName = String.Format(CultureInfo.InvariantCulture, "Kona.UILogic.ViewModels.{0}ViewModel, Kona.UILogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", viewName);
                        return Type.GetType(viewModelName);
                    });
            // </snippet301>

            // <snippet302>
            ViewModelLocator.Register(typeof(HubPage), () => new HubPageViewModel(_productCatalogRepository, navService));
            ViewModelLocator.Register(typeof(GroupDetailPage), () => new GroupDetailPageViewModel(_productCatalogRepository, navService));
            ViewModelLocator.Register(typeof(ItemDetailPage), () => new ItemDetailPageViewModel(_productCatalogRepository, navService, _shoppingCartRepository));
            ViewModelLocator.Register(typeof(SignInFlyout), () => new SignInFlyoutViewModel(_accountService, _credentialStore));
            ViewModelLocator.Register(typeof(SignOutFlyout), () => new SignOutFlyoutViewModel(_accountService, _credentialStore));
            ViewModelLocator.Register(typeof(ShoppingCartPage), () => new ShoppingCartPageViewModel(_shoppingCartRepository, navService, _accountService, _settingsCharmService));
            ViewModelLocator.Register(typeof(CheckoutSummaryPage), () => new CheckoutSummaryPageViewModel(navService, orderServiceProxy, shippingMethodServiceProxy, checkoutDataRepository, _shoppingCartRepository, _accountService, CreateSettingsCharmService(), _resourceLoader));
            ViewModelLocator.Register(typeof(CheckoutHubPage), () => new CheckoutHubPageViewModel(navService, _accountService, orderServiceProxy, shippingMethodServiceProxy, _shoppingCartRepository,
                                                                                                              new ShippingAddressUserControlViewModel(checkoutDataRepository, new LocationServiceProxy()),
                                                                                                              new BillingAddressUserControlViewModel(checkoutDataRepository, new LocationServiceProxy()), 
                                                                                                              new PaymentMethodUserControlViewModel(checkoutDataRepository)));
            ViewModelLocator.Register(typeof(PaymentMethodFlyout), () => new PaymentMethodFlyoutViewModel(new PaymentMethodUserControlViewModel(null), checkoutDataRepository));
            ViewModelLocator.Register(typeof(ShippingAddressFlyout), () => new ShippingAddressFlyoutViewModel(new ShippingAddressUserControlViewModel(null, new LocationServiceProxy()), checkoutDataRepository));
            ViewModelLocator.Register(typeof(BillingAddressFlyout), () => new BillingAddressFlyoutViewModel(new BillingAddressUserControlViewModel(null, new LocationServiceProxy()), checkoutDataRepository));
            ViewModelLocator.Register(typeof(ShoppingCartTabUserControl), () => new ShoppingCartTabUserControlViewModel(_shoppingCartRepository, _eventAggregator, _navigationService));
            ViewModelLocator.Register(typeof(EditPaymentMethodFlyout), () => new EditPaymentMethodFlyoutViewModel(new PaymentMethodUserControlViewModel(null), checkoutDataRepository));
            ViewModelLocator.Register(typeof(EditShippingAddressFlyout), () => new EditShippingAddressFlyoutViewModel(new ShippingAddressUserControlViewModel(null, new LocationServiceProxy()), checkoutDataRepository));
            ViewModelLocator.Register(typeof(EditBillingAddressFlyout), () => new EditBillingAddressFlyoutViewModel(new BillingAddressUserControlViewModel(null, new LocationServiceProxy()), checkoutDataRepository));
            ViewModelLocator.Register(typeof(TopAppBarUserControl), () => new TopAppBarUserControlViewModel(_navigationService));
            ViewModelLocator.Register(typeof(ChangeDefaultsFlyout), () => new ChangeDefaultsFlyoutViewModel(checkoutDataRepository));
            //</snippet302>
        }
    }
}
