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
using Kona.UILogic.Repositories;
using Windows.ApplicationModel.Resources;
using Windows.Globalization;
using Windows.Globalization.DateTimeFormatting;

namespace Kona.UILogic.ViewModels
{
    public class ChangeDefaultsFlyoutViewModel : ViewModel, IFlyoutViewModel
    {
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private CheckoutDataViewModel _selectedPaymentInfo;
        private CheckoutDataViewModel _selectedShippingAddress;
        private CheckoutDataViewModel _selectedBillingAddress;

        public ChangeDefaultsFlyoutViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _checkoutDataRepository = checkoutDataRepository;
            SaveDefaultsCommand = new DelegateCommand(SaveDefaults);
            GoBackCommand = new DelegateCommand(() => GoBack(), () => true);
        }

        public ICommand SaveDefaultsCommand { get; set; }
        public Action CloseFlyout { get; set; }
        public Action GoBack { get; set; }
        public ICommand GoBackCommand { get; private set; }
        public ICollection<CheckoutDataViewModel> PaymentInfos { get; private set; }
        public ICollection<CheckoutDataViewModel> ShippingAddresses { get; private set; }
        public ICollection<CheckoutDataViewModel> BillingAddresses { get; private set; }

        public CheckoutDataViewModel SelectedPaymentInfo
        {
            get { return _selectedPaymentInfo; }
            set { SetProperty(ref _selectedPaymentInfo, value); }
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
            var defaultPaymentMethod = _checkoutDataRepository.GetDefaultPaymentInfoValue();
            var defaultShippingAddress = _checkoutDataRepository.GetDefaultShippingAddressValue();
            var defaultBillingAddress = _checkoutDataRepository.GetDefaultBillingAddressValue();
            SelectedShippingAddress = ShippingAddresses.FirstOrDefault(s => s.EntityId == defaultShippingAddress.Id);
            SelectedBillingAddress = BillingAddresses.FirstOrDefault(b => b.EntityId == defaultBillingAddress.Id);
            SelectedPaymentInfo = PaymentInfos.FirstOrDefault(p => p.EntityId == defaultPaymentMethod.Id);
        }
         
        private void PopulateCollections()
        {
            var resourceLoader = new ResourceLoader();
            var shippingAddressesViewModels = new List<CheckoutDataViewModel>();

            var shippingAddresses = _checkoutDataRepository.RetrieveAllShippingAddresses();
            foreach (var address in shippingAddresses)
            {
                shippingAddressesViewModels.Add(
                    new CheckoutDataViewModel(
                        new
                            {
                                EntityId = address.Id,
                                FirstLine = address.StreetAddress,
                                SecondLine =
                            String.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", address.City, address.State,
                                          address.ZipCode),
                                Name =
                            String.Format(CultureInfo.CurrentUICulture, "{0} {1} {2}", address.FirstName, address.MiddleInitial,
                                          address.LastName),
                                DataType = resourceLoader.GetString("ShippingAddress"),
                                Content = address
                            }, null));
            }

            var billingAddressesViewModels = new List<CheckoutDataViewModel>();
            var billingAddresses = _checkoutDataRepository.RetrieveAllBillingAddresses();
            foreach (var billingAddress in billingAddresses)
            {
                billingAddressesViewModels.Add(
                    new CheckoutDataViewModel(
                        new
                            {
                                EntityId = billingAddress.Id,
                                FirstLine = billingAddress.StreetAddress,
                                SecondLine =
                            String.Format(CultureInfo.CurrentUICulture, "{0}, {1} {2}", billingAddress.City,
                                          billingAddress.State, billingAddress.ZipCode),
                                Name =
                            String.Format(CultureInfo.CurrentUICulture, "{0} {1} {2}", billingAddress.FirstName,
                                          billingAddress.MiddleInitial, billingAddress.LastName),
                                DataType = resourceLoader.GetString("BillingAddress"),
                                Content = billingAddress
                            }, null));
            }

            var paymentInfosViewModels = new List<CheckoutDataViewModel>();
            var tempDateTimeFormatter = new DateTimeFormatter("shortdate");
            var yearMonthDateTimeFormatter = new DateTimeFormatter(YearFormat.Full, MonthFormat.Abbreviated,
                                                                   DayFormat.None, DayOfWeekFormat.None, HourFormat.None,
                                                                   MinuteFormat.None, SecondFormat.None,
                                                                   tempDateTimeFormatter.Languages,
                                                                   tempDateTimeFormatter.GeographicRegion,
                                                                   CalendarIdentifiers.Gregorian,
                                                                   tempDateTimeFormatter.Clock);
            var paymentInfos = _checkoutDataRepository.RetrieveAllPaymentInformation();
            foreach (var paymentInfo in paymentInfos)
            {
                var creditCardExpirationDateOffset =
                    new DateTimeOffset(new DateTime(int.Parse(paymentInfo.ExpirationYear, CultureInfo.CurrentUICulture),
                                                    int.Parse(paymentInfo.ExpirationMonth, CultureInfo.CurrentUICulture), 2));
                paymentInfosViewModels.Add(
                    new CheckoutDataViewModel(
                        new
                            {
                                EntityId = paymentInfo.Id,
                                FirstLine =
                            string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardEndingIn"),
                                          paymentInfo.CardNumber.Substring(paymentInfo.CardNumber.Length - 4)),
                                SecondLine =
                            string.Format(CultureInfo.CurrentUICulture, resourceLoader.GetString("CardExpiringOn"),
                                          yearMonthDateTimeFormatter.Format(creditCardExpirationDateOffset)),
                                Name = paymentInfo.CardholderName,
                                DataType = resourceLoader.GetString("Payment"),
                                Content = paymentInfo
                            }, null));
                PaymentInfos = new ReadOnlyCollection<CheckoutDataViewModel>(paymentInfosViewModels);
                ShippingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(shippingAddressesViewModels);
                BillingAddresses = new ReadOnlyCollection<CheckoutDataViewModel>(billingAddressesViewModels);
            }
        }

        private void SaveDefaults()
        {
            _checkoutDataRepository.SetAsDefaultShippingAddress(SelectedShippingAddress != null
                                                                    ? SelectedShippingAddress.EntityId
                                                                    : null);
            _checkoutDataRepository.SetAsDefaultBillingAddress(SelectedBillingAddress != null
                                                                   ? SelectedBillingAddress.EntityId
                                                                   : null);
            _checkoutDataRepository.SetAsDefaultPaymentInfo(SelectedPaymentInfo != null
                                                                ? SelectedPaymentInfo.EntityId
                                                                : null);
            this.CloseFlyout();
        }
    }
}