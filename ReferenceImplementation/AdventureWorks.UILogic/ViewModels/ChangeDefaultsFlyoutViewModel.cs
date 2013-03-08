// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ChangeDefaultsFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly IResourceLoader _resourceLoader;
        private CheckoutDataViewModel _selectedShippingAddress;
        private CheckoutDataViewModel _selectedBillingAddress;
        private CheckoutDataViewModel _selectedPaymentMethod;

        public ChangeDefaultsFlyoutViewModel(ICheckoutDataRepository checkoutDataRepository, IResourceLoader resourceLoader)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;

            GoBackCommand = new DelegateCommand(() => GoBack());
        }

        public IReadOnlyCollection<CheckoutDataViewModel> PaymentMethods { get; private set; }

        public IReadOnlyCollection<CheckoutDataViewModel> ShippingAddresses { get; private set; }

        public IReadOnlyCollection<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public Action CloseFlyout { get; set; }

        public Action GoBack { get; set; }

        public ICommand GoBackCommand { get; private set; }

        public CheckoutDataViewModel SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set
            {
                if (SetProperty(ref _selectedShippingAddress, value))
                {
                    if (_selectedShippingAddress != null)
                    {
                        _checkoutDataRepository.SetDefaultShippingAddress(_selectedShippingAddress.EntityId);
                    }
                    else
                    {
                        _checkoutDataRepository.RemoveDefaultShippingAddress();
                    }
                }
            }
        }

        public CheckoutDataViewModel SelectedBillingAddress
        {
            get { return _selectedBillingAddress; }
            set
            {
                if (SetProperty(ref _selectedBillingAddress, value))
                {
                    if (_selectedBillingAddress != null)
                    {
                        _checkoutDataRepository.SetDefaultBillingAddress(_selectedBillingAddress.EntityId);
                    }
                    else
                    {
                        _checkoutDataRepository.RemoveDefaultBillingAddress();
                    }
                }
            }
        }

        public CheckoutDataViewModel SelectedPaymentMethod
        {
            get { return _selectedPaymentMethod; }
            set
            {
                if (SetProperty(ref _selectedPaymentMethod, value))
                {
                    if (_selectedPaymentMethod != null)
                    {
                        _checkoutDataRepository.SetDefaultPaymentMethod(_selectedPaymentMethod.EntityId);
                    }
                    else
                    {
                        _checkoutDataRepository.RemoveDefaultPaymentMethod();
                    }
                }
            }
        }

        public async void Open(object parameter, Action successAction)
        {
            // Populate ShippingAddress collection
            var shippingAddresses = _checkoutDataRepository.GetAllShippingAddresses().Select(address => CreateCheckoutData(address, Constants.ShippingAddress));
            ShippingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(shippingAddresses.ToList());

            if (ShippingAddresses != null)
            {
                var defaultShippingAddress = _checkoutDataRepository.GetDefaultShippingAddress();
                var selectedShippingAddress = defaultShippingAddress != null ? ShippingAddresses.FirstOrDefault(s => s.EntityId == defaultShippingAddress.Id) : null;
                SetProperty(ref _selectedShippingAddress, selectedShippingAddress, "SelectedShippingAddress");
            }

            // Populate BillingAddress collection
            var billingAddresses = _checkoutDataRepository.GetAllBillingAddresses().Select(address => CreateCheckoutData(address, Constants.BillingAddress));
            BillingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(billingAddresses.ToList());

            if (BillingAddresses != null)
            {
                var defaultBillingAddress = _checkoutDataRepository.GetDefaultBillingAddress();
                var selectedBillingAddress = defaultBillingAddress != null ? BillingAddresses.FirstOrDefault(s => s.EntityId == defaultBillingAddress.Id) : null;
                SetProperty(ref _selectedBillingAddress, selectedBillingAddress, "SelectedBillingAddress");
            }

            // Populate PaymentMethod collection
            var paymentMethods = (await _checkoutDataRepository.GetAllPaymentMethodsAsync()).Select(CreateCheckoutData);
            PaymentMethods = new ReadOnlyCollection<CheckoutDataViewModel>(paymentMethods.ToList());

            if (PaymentMethods != null)
            {
                var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();
                var selectedPaymentMethod = defaultPaymentMethod != null ? PaymentMethods.FirstOrDefault(s => s.EntityId == defaultPaymentMethod.Id) : null;
                SetProperty(ref _selectedPaymentMethod, selectedPaymentMethod, "SelectedPaymentMethod");
            }
        }

        private CheckoutDataViewModel CreateCheckoutData(Address address, string dataType)
        {
            return new CheckoutDataViewModel()
                {
                    EntityId = address.Id,
                    DataType = dataType,
                    Title = dataType == Constants.ShippingAddress ? _resourceLoader.GetString("ShippingAddress") : _resourceLoader.GetString("BillingAddress"),
                    FirstLine = address.StreetAddress,
                    SecondLine =  string.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", address.City, address.State, address.ZipCode),
                    BottomLine = string.Format(CultureInfo.CurrentUICulture, "{0} {1}", address.FirstName, address.LastName),
                    LogoUri = dataType == Constants.ShippingAddress ? new Uri(Constants.ShippingAddressLogo, UriKind.Absolute) : new Uri(Constants.BillingAddressLogo, UriKind.Absolute)
                };
        }

        private CheckoutDataViewModel CreateCheckoutData(PaymentMethod paymentMethod)
        {
            return new CheckoutDataViewModel()
                {
                    EntityId = paymentMethod.Id,
                    DataType = Constants.PaymentMethod,
                    Title = _resourceLoader.GetString("PaymentMethod"),
                    FirstLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardEndingIn"), paymentMethod.CardNumber.Substring(paymentMethod.CardNumber.Length - 4)),
                    SecondLine = string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardExpiringOn"),
                                                string.Format(CultureInfo.CurrentCulture, "{0}/{1}", paymentMethod.ExpirationMonth, paymentMethod.ExpirationYear)),
                    BottomLine = paymentMethod.CardholderName,
                    LogoUri = new Uri(Constants.PaymentMethodLogo, UriKind.Absolute)
                };
        }
    }
}