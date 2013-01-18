// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Kona.Infrastructure;
using System.Collections.Generic;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;

namespace Kona.UILogic.ViewModels
{
    public class BillingAddressUserControlViewModel : ViewModel, INavigationAware, IBillingAddressUserControlViewModel
    {
        private Address _address;
        private bool _saveAddress;
        private bool _setAsDefault;
        private IReadOnlyCollection<ComboBoxItemValue> _states;
        private readonly ILocationService _locationService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private int _currentFormStatus;
        private bool _isEnabled = true;

        public BillingAddressUserControlViewModel(ICheckoutDataRepository checkoutDataRepository, ILocationService locationService)
        {
            _address = new Address() { Id = Guid.NewGuid().ToString() };
            _address.ErrorsChanged += ValidatorErrorsChanged;
            _checkoutDataRepository = checkoutDataRepository;
            _locationService = locationService;
            _currentFormStatus = FormStatus.Incomplete;
        }

        [RestorableState]
        public Address Address
        {
            get { return _address; }
            set
            {
                if (SetProperty(ref _address, value))
                {
                    _address.ErrorsChanged += ValidatorErrorsChanged;
                    OnPropertyChanged("Errors");
                }
            }
        }

        [RestorableState]
        public bool SaveAddress
        {
            get { return _saveAddress; }
            set { SetProperty(ref _saveAddress, value); }
        }

        [RestorableState]
        public bool SetAsDefault
        {
            get { return _setAsDefault; }
            set { SetProperty(ref _setAsDefault, value); }
        }

        [RestorableState]
        public int CurrentFormStatus
        {
            get { return _currentFormStatus; }
            set { SetProperty(ref _currentFormStatus, value); }
        }

        public IReadOnlyCollection<ComboBoxItemValue> States
        {
            get { return _states; }
            set { SetProperty(ref _states, value); }
        }

        public BindableValidator Errors
        {
            get { return _address.Errors; }
        }

        public void UpdateAddressInformation(Address address)
        {
            _address.FirstName = address.FirstName;
            _address.MiddleInitial = address.MiddleInitial;
            _address.LastName = address.LastName;
            _address.StreetAddress = address.StreetAddress;
            _address.OptionalAddress = address.OptionalAddress;
            _address.City = address.City;
            _address.State = address.State;
            _address.ZipCode = address.ZipCode;
            _address.Phone = address.Phone;
        }

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
                if (_checkoutDataRepository.ContainsDefaultValue("BillingAddress"))
                {
                    Address = _checkoutDataRepository.GetDefaultBillingAddressValue();

                    // Validate form fields
                    bool isValid = _address.ValidateProperties();
                    CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            if (!suspending)
            {
                _address.ErrorsChanged -= ValidatorErrorsChanged;
            }

            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _address.GetAllErrors(), viewState);
            }
        }

        public bool ValidateForm()
        {
            bool isValid = _address.ValidateProperties();
            CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
            return isValid;
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        public void ProcessForm()
        {
            if (SaveAddress)
            {
                _checkoutDataRepository.SaveBillingAddress(_address);
            }

            if (SetAsDefault)
            {
                _checkoutDataRepository.SetAsDefaultBillingAddress(_address.Id);
            }
        }

        public async Task PopulateStatesAsync()
        {
            var items = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = "State" } };
            var states = await _locationService.GetStatesAsync();

            items.AddRange(states.Select(state => new ComboBoxItemValue() { Id = state, Value = state }));
            States = new ReadOnlyCollection<ComboBoxItemValue>(items);

            // Select the first item on the list
            // But disable validation first, because we don't want to fire it at this point
            _address.IsValidationEnabled = false;
            _address.State = States.First().Id;
            _address.IsValidationEnabled = true;
        }

        private void ValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var allErrors = _address.GetAllErrors();
            CurrentFormStatus = allErrors.Values.Count > 0 ? FormStatus.Invalid : FormStatus.Incomplete;
        }
    }
}
