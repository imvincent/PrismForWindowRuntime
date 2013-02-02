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
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Globalization.DateTimeFormatting;

namespace Kona.UILogic.ViewModels
{
    public class ChangeDefaultsFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private CheckoutDataViewModel _selectedPaymentMethod;
        private CheckoutDataViewModel _selectedShippingAddress;
        private CheckoutDataViewModel _selectedBillingAddress;
        private IResourceLoader _resourceLoader;

        public ChangeDefaultsFlyoutViewModel(ICheckoutDataRepository checkoutDataRepository, IResourceLoader resourceLoader)
        {
            _checkoutDataRepository = checkoutDataRepository;
            _resourceLoader = resourceLoader;
            SaveCommand = new DelegateCommand(SaveDefaults);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public ICollection<CheckoutDataViewModel> PaymentMethods { get; private set; }

        public ICollection<CheckoutDataViewModel> ShippingAddresses { get; private set; }

        public ICollection<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public Action CloseFlyout { get; set; }

        public Action GoBack { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand GoBackCommand { get; private set; }

        public CheckoutDataViewModel SelectedPaymentMethod
        {
            get { return _selectedPaymentMethod; }
            set { SetProperty(ref _selectedPaymentMethod, value); }
        }

        public CheckoutDataViewModel SelectedShippingAddress
        {
            get { return _selectedShippingAddress; }
            set { SetProperty(ref _selectedShippingAddress, value); }
        }

        public CheckoutDataViewModel SelectedBillingAddress
        {
            get { return _selectedBillingAddress; }
            set { SetProperty(ref _selectedBillingAddress, value); }
        }

        public void Open(object parameter, Action successAction)
        {
            PopulateCollections();
            var defaultPaymentMethod = _checkoutDataRepository.GetDefaultPaymentMethodValue();
            var defaultShippingAddress = _checkoutDataRepository.GetDefaultShippingAddressValue();
            var defaultBillingAddress = _checkoutDataRepository.GetDefaultBillingAddressValue();
            
            if (ShippingAddresses != null && defaultShippingAddress != null)
            {
                SelectedShippingAddress = ShippingAddresses.FirstOrDefault(s => s.EntityId == defaultShippingAddress.Id);
            }
            if (BillingAddresses != null && defaultBillingAddress != null)
            {
                SelectedBillingAddress = BillingAddresses.FirstOrDefault(b => b.EntityId == defaultBillingAddress.Id);
            }
            if (PaymentMethods != null && defaultPaymentMethod != null)
            {
                SelectedPaymentMethod = PaymentMethods.FirstOrDefault(p => p.EntityId == defaultPaymentMethod.Id);
            }
        }

        private void PopulateCollections()
        {
            var shippingAddresses = _checkoutDataRepository.GetAllShippingAddresses()
                                                           .Select(address => CreateCheckoutData(address, Constants.ShippingAddress));

            var billingAddresses = _checkoutDataRepository.GetAllBillingAddresses()
                                                          .Select(address => CreateCheckoutData(address, Constants.BillingAddress));


            var paymentMethods = _checkoutDataRepository.GetAllPaymentMethods()
                                                        .Select(paymentMethod => CreateCheckoutData(paymentMethod));

            ShippingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(shippingAddresses.ToList());
            BillingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(billingAddresses.ToList());
            PaymentMethods = new ReadOnlyCollection<CheckoutDataViewModel>(paymentMethods.ToList());
        }

        private void SaveDefaults()
        {
            _checkoutDataRepository.SetAsDefaultShippingAddress(SelectedShippingAddress != null ? SelectedShippingAddress.EntityId : null);
            _checkoutDataRepository.SetAsDefaultBillingAddress(SelectedBillingAddress != null ? SelectedBillingAddress.EntityId : null);
            _checkoutDataRepository.SetAsDefaultPaymentMethod(SelectedPaymentMethod != null ? SelectedPaymentMethod.EntityId : null);
            
            CloseFlyout();
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