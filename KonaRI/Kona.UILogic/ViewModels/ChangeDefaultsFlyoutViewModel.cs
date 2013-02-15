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
using Kona.Infrastructure;
using Kona.Infrastructure.Flyouts;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;

namespace Kona.UILogic.ViewModels
{
    public class ChangeDefaultsFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private IResourceLoader _resourceLoader;
        private CheckoutDataViewModel _selectedPaymentMethod;
        private CheckoutDataViewModel _selectedShippingAddress;
        private CheckoutDataViewModel _selectedBillingAddress;

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
                var oldValue = _selectedShippingAddress;
                if (SetProperty(ref _selectedShippingAddress, value) && value != null && oldValue != null)
                {
                    _checkoutDataRepository.SetDefaultShippingAddress((Address)_selectedShippingAddress.Context);
                }
            }
        }

        public CheckoutDataViewModel SelectedBillingAddress
        {
            get { return _selectedBillingAddress; }
            set
            {
                var oldValue = _selectedBillingAddress;
                if (SetProperty(ref _selectedBillingAddress, value) && value != null && oldValue != null)
                {
                    _checkoutDataRepository.SetDefaultBillingAddress((Address)_selectedBillingAddress.Context);
                }
            }
        }

        public CheckoutDataViewModel SelectedPaymentMethod
        {
            get { return _selectedPaymentMethod; }
            set
            {
                var oldValue = _selectedPaymentMethod;
                if (SetProperty(ref _selectedPaymentMethod, value) && value != null && oldValue != null)
                {
                    _checkoutDataRepository.SetDefaultPaymentMethod((PaymentMethod)_selectedPaymentMethod.Context);
                }
            }
        }

        public void Open(object parameter, Action successAction)
        {
            // Populate ShippingAddress collection
            var shippingAddresses = _checkoutDataRepository.GetAllShippingAddresses().Select(address => CreateCheckoutData(address, Constants.ShippingAddress));
            ShippingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(shippingAddresses.ToList());

            if (ShippingAddresses != null)
            {
                var defaultShippingAddress = _checkoutDataRepository.GetDefaultShippingAddress();
                SelectedShippingAddress = defaultShippingAddress != null ? ShippingAddresses.FirstOrDefault(s => s.EntityId == defaultShippingAddress.Id) : null;
            }

            // Populate BillingAddress collection
            var billingAddresses = _checkoutDataRepository.GetAllBillingAddresses().Select(address => CreateCheckoutData(address, Constants.BillingAddress));
            BillingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(billingAddresses.ToList());

            if (BillingAddresses != null)
            {
                var defaultBillingAddress = _checkoutDataRepository.GetDefaultBillingAddress();
                SelectedBillingAddress =  defaultBillingAddress != null ? BillingAddresses.FirstOrDefault(s => s.EntityId == defaultBillingAddress.Id) : null;
            }

            // Populate PaymentMethod collection
            var paymentMethods = _checkoutDataRepository.GetAllPaymentMethods().Select(payment => CreateCheckoutData(payment));
            PaymentMethods = new ReadOnlyCollection<CheckoutDataViewModel>(paymentMethods.ToList());

            if (PaymentMethods != null)
            {
                var defaultPaymentMethod = _checkoutDataRepository.GetDefaultPaymentMethod();
                SelectedPaymentMethod = defaultPaymentMethod != null ? PaymentMethods.FirstOrDefault(s => s.EntityId == defaultPaymentMethod.Id) : null;
            }
        }

        private CheckoutDataViewModel CreateCheckoutData(Address address, string dataType)
        {
            return new CheckoutDataViewModel(address.Id,
                                            dataType == Constants.ShippingAddress ? _resourceLoader.GetString("ShippingAddress") : _resourceLoader.GetString("BillingAddress"),
                                            address.StreetAddress,
                                            string.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", address.City, address.State, address.ZipCode),
                                            string.Format(CultureInfo.CurrentUICulture, "{0} {1}", address.FirstName, address.LastName),
                                            dataType == Constants.ShippingAddress ? new Uri(Constants.ShippingAddressLogo, UriKind.Absolute) : new Uri(Constants.BillingAddressLogo, UriKind.Absolute),
                                            dataType,
                                            address);
        }

        private CheckoutDataViewModel CreateCheckoutData(PaymentMethod paymentMethod)
        {
            return new CheckoutDataViewModel(paymentMethod.Id,
                                            _resourceLoader.GetString("PaymentMethod"),
                                            string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardEndingIn"), paymentMethod.CardNumber.Substring(paymentMethod.CardNumber.Length - 4)),
                                            string.Format(CultureInfo.CurrentUICulture, _resourceLoader.GetString("CardExpiringOn"),
                                            string.Format(CultureInfo.CurrentCulture, "{0}/{1}", paymentMethod.ExpirationMonth, paymentMethod.ExpirationYear)),
                                            paymentMethod.CardholderName,
                                            new Uri(Constants.PaymentMethodLogo, UriKind.Absolute),
                                            Constants.PaymentMethod, 
                                            paymentMethod);
        }
    }
}