// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.StoreApps.Infrastructure;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class BillingAddressUserControlViewModel : ViewModel, IBillingAddressUserControlViewModel
    {
        private Address _address;
        private bool _isEnabled;
        private bool _loadDefault;
        private IReadOnlyCollection<ComboBoxItemValue> _states;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly ILocationService _locationService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        
        public BillingAddressUserControlViewModel(ICheckoutDataRepository checkoutDataRepository, ILocationService locationService, IResourceLoader resourceLoader,
                                                  IAlertMessageService alertMessageService)
        {
            _address = new Address();
            _isEnabled = true;
            _checkoutDataRepository = checkoutDataRepository;
            _locationService = locationService;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
        }

        [RestorableState]
        public Address Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public IReadOnlyCollection<ComboBoxItemValue> States
        {
            get { return _states; }
            private set { SetProperty(ref _states, value); }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (SetProperty(ref _isEnabled, value))
                {
                    // Enable/Disable validation based on the value of this property
                    Address.IsValidationEnabled = IsEnabled;
                }
            }
        }

        // <snippet911>
        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            // The States collection needs to be populated before setting the State property
            await PopulateStatesAsync();

            if (viewState != null)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewState);

                if (navigationMode == NavigationMode.Refresh)
                {
                    // Restore the errors collection manually
                    var errorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("errorsCollection", viewState);

                    if (errorsCollection != null)
                    {
                        _address.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                var addressId = navigationParameter as string;
                if (addressId != null)
                {
                    Address = await _checkoutDataRepository.GetBillingAddressAsync(addressId);
                    return;
                }

                if (_loadDefault)
                {
                    var defaultAddress = await _checkoutDataRepository.GetDefaultBillingAddressAsync();
                    if (defaultAddress != null)
                    {
                        // Update the information and validate the values
                        Address.FirstName = defaultAddress.FirstName;
                        Address.MiddleInitial = defaultAddress.MiddleInitial;
                        Address.LastName = defaultAddress.LastName;
                        Address.StreetAddress = defaultAddress.StreetAddress;
                        Address.OptionalAddress = defaultAddress.OptionalAddress;
                        Address.City = defaultAddress.City;
                        Address.State = defaultAddress.State;
                        Address.ZipCode = defaultAddress.ZipCode;
                        Address.Phone = defaultAddress.Phone;
                    }
                }
            }
        }
        // </snippet911>

        // <snippet910>
        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _address.GetAllErrors(), viewState);
            }
        }
        // </snippet910>

        public bool ValidateForm()
        {
            return _address.ValidateProperties();
        }

        public async Task ProcessFormAsync()
        {
            await _checkoutDataRepository.SaveBillingAddressAsync(_address);
        }

        public async Task PopulateStatesAsync()
        {
            string errorMessage = string.Empty;
            try
            {
                var states = await _locationService.GetStatesAsync();

                var items = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = _resourceLoader.GetString("State") } };
                items.AddRange(states.Select(state => new ComboBoxItemValue() {Id = state, Value = state}));
                States = new ReadOnlyCollection<ComboBoxItemValue>(items);

                // Select the first item on the list
                // But disable validation first, because we don't want to fire validation at this point
                _address.IsValidationEnabled = false;
                _address.State = States.First().Id;
                _address.IsValidationEnabled = true;
            }
            catch (HttpRequestException ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"),
                                             Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }
        }

        public void SetLoadDefault(bool loadDefault)
        {
            _loadDefault = loadDefault;
        }
    }
}
