// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.BillingAddressFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.ChangeDefaultsFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "AdventureWorks.UILogic.ViewModels.CheckoutHubPageViewModel.#.ctor(Microsoft.Practices.StoreApps.Infrastructure.Interfaces.INavigationService,AdventureWorks.UILogic.Services.IAccountService,AdventureWorks.UILogic.Repositories.IOrderRepository,AdventureWorks.UILogic.Repositories.IShoppingCartRepository,AdventureWorks.UILogic.ViewModels.IShippingAddressUserControlViewModel,AdventureWorks.UILogic.ViewModels.IBillingAddressUserControlViewModel,AdventureWorks.UILogic.ViewModels.IPaymentMethodUserControlViewModel,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IFlyoutService,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IResourceLoader,AdventureWorks.UILogic.Services.IAlertMessageService)", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "AdventureWorks.UILogic.ViewModels.CheckoutSummaryPageViewModel.#.ctor(Microsoft.Practices.StoreApps.Infrastructure.Interfaces.INavigationService,AdventureWorks.UILogic.Services.IOrderService,AdventureWorks.UILogic.Repositories.IOrderRepository,AdventureWorks.UILogic.Services.IShippingMethodService,AdventureWorks.UILogic.Repositories.ICheckoutDataRepository,AdventureWorks.UILogic.Repositories.IShoppingCartRepository,AdventureWorks.UILogic.Services.IAccountService,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IFlyoutService,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IResourceLoader,AdventureWorks.UILogic.Services.IAlertMessageService)", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.ShippingAddressFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "flyout", Scope = "member", Target = "AdventureWorks.UILogic.ViewModels.ShoppingCartPageViewModel.#.ctor(AdventureWorks.UILogic.Repositories.IShoppingCartRepository,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.INavigationService,AdventureWorks.UILogic.Services.IAccountService,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IFlyoutService,Microsoft.Practices.StoreApps.Infrastructure.Interfaces.IResourceLoader,AdventureWorks.UILogic.Services.IAlertMessageService,AdventureWorks.UILogic.Repositories.ICheckoutDataRepository,AdventureWorks.UILogic.Repositories.IOrderRepository,Microsoft.Practices.PubSubEvents.IEventAggregator)", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.SignInFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.SignOutFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Signing provided by app package.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetDefaultShippingAddress()", Justification = "Using method instead of property for consistency with async get and indexed methods.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetDefaultBillingAddress()", Justification = "Using method instead of property for consistency with async get and indexed methods.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetDefaultPaymentMethodAsync()", Justification = "Get method is async. New value returned with every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllShippingAddresses()", Justification = "Using method instead of property for consistency with async get and indexed methods.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllBillingAddresses()", Justification = "Using method instead of property for consistency with async get and indexed methods.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllPaymentMethodsAsync()", Justification = "Get method is async. New value returned with every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllPaymentMethodsAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.ILocationService.#GetStatesAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.IProductCatalogRepository.#GetRootCategoriesAsync(System.Int32)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.IProductCatalogRepository.#GetSubcategoriesAsync(System.Int32,System.Int32)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.IProductCatalogRepository.#GetFilteredProductsAsync(System.String)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.IProductCatalogRepository.#GetProductsAsync(System.Int32)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IProductCatalogService.#GetCategoriesAsync(System.Int32,System.Int32)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IProductCatalogService.#GetFilteredProductsAsync(System.String)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IProductCatalogService.#GetProductsAsync(System.Int32)", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IShippingMethodService.#GetShippingMethodsAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Flyout", Scope = "type", Target = "AdventureWorks.UILogic.ViewModels.PaymentMethodFlyoutViewModel", Justification = "Flyout is a standard Windows Store term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Services.ILocationService.#GetStatesAsync()", Justification = "This method returns a new value for every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Services.IShippingMethodService.#GetShippingMethodsAsync()", Justification = "This method returns a new value for every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Services.IShippingMethodService.#GetBasicShippingMethodAsync()", Justification = "This method returns a new value for every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.IShoppingCartRepository.#GetShoppingCartAsync()", Justification = "This method returns a new value for every call.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "AdventureWorks.UILogic.Services.RoamingCredentialStore.#RemoveAllCredentialsByResource(System.String,Windows.Security.Credentials.PasswordVault)", Justification = "PasswordVault throws System.Exception.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "AdventureWorks.UILogic.Services.RoamingCredentialStore.#GetSavedCredentials(System.String)", Justification = "PasswordVault throws System.Exception.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Models.ModelValidationResult.#ModelState", Justification = "ModelState is populated by calling HttpContentExtensions.ReadAsAsync. This model class needs to expose the same shape as the data being transfered.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "AdventureWorks.UILogic.ViewModels.CheckoutSummaryPageViewModel.#UpdateOrderCheckoutData(System.Object,System.String)", Justification = "This cast is only done once per option in switch statement. The switch case blocks are mutually exclusive using break statements.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Services.IAddressService.#GetAddressesAsync()", Justification = "This method can make an async call to fetch data.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IAddressService.#GetAddressesAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllShippingAddressesAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllBillingAddressesAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Scope = "member", Target = "AdventureWorks.UILogic.Services.IPaymentMethodService.#GetPaymentMethodsAsync()", Justification = "Required by Task<T> data type.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetDefaultShippingAddressAsync()", Justification = "This method can make an async call to fetch data.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetDefaultBillingAddressAsync()", Justification = "This method can make an async call to fetch data.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllShippingAddressesAsync()", Justification = "This method can make an async call to fetch data.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Repositories.ICheckoutDataRepository.#GetAllBillingAddressesAsync()", Justification = "This method can make an async call to fetch data.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.UILogic.Services.IPaymentMethodService.#GetPaymentMethodsAsync()", Justification = "This method can make an async call to fetch data.")]
