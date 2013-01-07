// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using System.Collections.Generic;
using Kona.UILogic.Models;
using Kona.UILogic.Repositories;
using Kona.UILogic.Services;
using Windows.UI.Xaml.Navigation;
using System.Collections.ObjectModel;

namespace Kona.UILogic.ViewModels
{
    public class ShippingAddressUserControlViewModel : ViewModel, INavigationAware, IShippingAddressUserControlViewModel
    {
        private Address _address;
        private IReadOnlyCollection<ComboBoxItemValue> _states;
        private bool _saveAddress;
        private bool _setAsDefault;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private readonly ILocationService _locationService;
        private EntityValidator _validator;
        private int _currentFormStatus;
        private bool _validationEnabled;

        public ShippingAddressUserControlViewModel(ICheckoutDataRepository checkoutDataRepository, ILocationService locationService)
        {
            _address = new Address() { Id = Guid.NewGuid().ToString() };
            _address.PropertyChanged += Address_PropertyChanged;

            _validator = new EntityValidator(_address);
            _validator.ErrorsChanged += ValidatorErrorsChanged;
            _validationEnabled = true;

            _checkoutDataRepository = checkoutDataRepository;
            _locationService = locationService;
            _currentFormStatus = FormStatus.Incomplete;
        }

        [RestorableState]
        public Address Address
        {
            get { return _address; }
            private set
            {
                if (_address != value)
                {
                    if (_address != null)
                    {
                        _address.PropertyChanged -= Address_PropertyChanged;
                    }

                    if (value != null)
                    {
                        value.PropertyChanged += Address_PropertyChanged;
                    }

                    SetProperty(ref _address, value);
                    Validator = new EntityValidator(_address);
                }
            }
        }

        public IReadOnlyCollection<ComboBoxItemValue> States
        {
            get { return _states; }
            set { SetProperty(ref _states, value); }
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

        public EntityValidator Validator
        {
            get { return _validator; }
            private set
            {
                if (_validator != value)
                {
                    if (_validator != null)
                    {
                        _validator.ErrorsChanged -= ValidatorErrorsChanged;
                    }

                    if (value != null)
                    {
                        value.ErrorsChanged += ValidatorErrorsChanged;
                    }

                    SetProperty(ref _validator, value);
                }
            }
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
                        _validator.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                if (_checkoutDataRepository.ContainsDefaultValue("ShippingAddress"))
                {
                    var defaultAddress = _checkoutDataRepository.GetDefaultValue<Address>("ShippingAddress");
                    Address = defaultAddress;

                    // Validate form fields
                    bool isValid = await Validator.ValidatePropertiesAsync();
                    CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            if (!suspending)
            {
                Address.PropertyChanged -= Address_PropertyChanged;
                Validator.ErrorsChanged -= ValidatorErrorsChanged;
            }

            base.OnNavigatedFrom(viewState, suspending);

            // Store the errors collection manually
            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _validator.GetAllErrors(), viewState);
            }
        }

        public async Task<bool> ValidateFormAsync()
        {
            bool isValid = await Validator.ValidatePropertiesAsync();
            CurrentFormStatus = isValid ? FormStatus.Complete : FormStatus.Invalid;
            return isValid;
        }

        public void ProcessForm()
        {
            if (SaveAddress)
            {
                _checkoutDataRepository.SaveShippingAddress(Address);
            }

            if (SetAsDefault)
            {
                _checkoutDataRepository.SetAsDefaultValue("ShippingAddress", Address.Id);
            }
        }

        private void Address_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e == null || string.IsNullOrEmpty(e.PropertyName))
            {
                return;
            }

            if (_validationEnabled)
            {
                Validator.ValidateProperty(e.PropertyName);

                if (e.PropertyName == "State" || e.PropertyName == "ZipCode")
                {
                    Validator.ValidatePropertyAsync("ZipCode");
                }
            }
        }

        private async Task PopulateStatesAsync()
        {
            var items = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = "State" } };
            var states = await _locationService.GetStatesAsync();

            items.AddRange(states.Select(state => new ComboBoxItemValue() { Id = state, Value = state }));
            States = new ReadOnlyCollection<ComboBoxItemValue>(items);

            // We need to set the default empty value to the State property
            // but we don't want to fire the validation
            _validationEnabled = false;
            _address.State = States.First().Id;
            _validationEnabled = true;
        }

        private void ValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var allErrors = Validator.GetAllErrors();
            CurrentFormStatus = allErrors.Values.Count > 0 ? FormStatus.Invalid : FormStatus.Incomplete;
        }
    }
}
