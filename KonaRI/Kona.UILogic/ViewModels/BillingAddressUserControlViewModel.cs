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
using System.Windows.Input;
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
        private EntityValidator _validator;
        private bool _saveAddress;
        private bool _setAsDefault;
        private IReadOnlyCollection<ComboBoxItemValue> _states;
        private readonly ILocationService _locationService;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private int _currentFormStatus;

        public BillingAddressUserControlViewModel(ICheckoutDataRepository checkoutDataRepository, ILocationService locationService)
        {
            _address = new Address() { Id = Guid.NewGuid().ToString() };
            _validator = new EntityValidator(_address);
            _validator.ErrorsChanged += ValidatorErrorsChanged;

            _checkoutDataRepository = checkoutDataRepository;
            _locationService = locationService;
        }

        public EntityValidator Validator
        {
            get { return _validator; }
        }

        [RestorableState]
        public string FirstName
        {
            get { return _address.FirstName; }
            set
            {
                if (_address.FirstName != value)
                {
                    _address.FirstName = value;
                    OnPropertyChanged("FirstName");
                    Validator.ValidateProperty("FirstName");
                }
            }
        }

        [RestorableState]
        public string MiddleInitial
        {
            get { return _address.MiddleInitial; }
            set
            {
                if (_address.MiddleInitial != value)
                {
                    _address.MiddleInitial = value;
                    OnPropertyChanged("MiddleInitial");
                    Validator.ValidateProperty("MiddleInitial");
                }
            }
        }

        [RestorableState]
        public string LastName
        {
            get { return _address.LastName; }
            set
            {
                if (_address.LastName != value)
                {
                    _address.LastName = value;
                    OnPropertyChanged("LastName");
                    Validator.ValidateProperty("LastName");
                }
            }
        }

        [RestorableState]
        public string StreetAddress
        {
            get { return _address.StreetAddress; }
            set
            {
                if (_address.StreetAddress != value)
                {
                    _address.StreetAddress = value;
                    OnPropertyChanged("StreetAddress");
                    Validator.ValidateProperty("StreetAddress");
                }
            }
        }

        [RestorableState]
        public string OptionalAddress
        {
            get { return _address.OptionalAddress; }
            set
            {
                if (_address.OptionalAddress != value)
                {
                    _address.OptionalAddress = value;
                    OnPropertyChanged("OptionalAddress");
                    Validator.ValidateProperty("OptionalAddress");
                }
            }
        }

        [RestorableState]
        public string City
        {
            get { return _address.City; }
            set
            {
                if (_address.City != value)
                {
                    _address.City = value;
                    OnPropertyChanged("City");
                    Validator.ValidateProperty("City");
                }
            }
        }

        [RestorableState]
        public string State
        {
            get { return _address.State; }
            set
            {
                if (_address.State != value)
                {
                    _address.State = value;
                    OnPropertyChanged("State");
                    Validator.ValidateProperty("State");
                    Validator.ValidatePropertyAsync("ZipCode");
                }
            }
        }

        [RestorableState]
        public string ZipCode
        {
            get { return _address.ZipCode; }
            set
            {
                if (_address.ZipCode != value)
                {
                    _address.ZipCode = value;
                    OnPropertyChanged("ZipCode");
                    Validator.ValidateProperty("ZipCode");
                    Validator.ValidatePropertyAsync("ZipCode");
                }
            }
        }

        [RestorableState]
        public string Phone
        {
            get { return _address.Phone; }
            set
            {
                if (_address.Phone != value)
                {
                    _address.Phone = value;
                    OnPropertyChanged("Phone");
                    Validator.ValidateProperty("Phone");
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
                if (_checkoutDataRepository.ContainsDefaultValue("BillingAddress"))
                {
                    var defaultAddress = _checkoutDataRepository.GetDefaultValue<Address>("BillingAddress");
                    UpdateModel(defaultAddress);

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
                _checkoutDataRepository.SaveBillingAddress(_address);
            }

            if (SetAsDefault)
            {
                _checkoutDataRepository.SetAsDefaultValue("BillingAddress", _address.Id);
            }
        }

        public Address GetAddress()
        {
            return new Address
                {
                    Id = EntityId,
                    FirstName = FirstName,
                    MiddleInitial = MiddleInitial,
                    LastName = LastName,
                    StreetAddress = StreetAddress,
                    OptionalAddress = OptionalAddress,
                    City = City,
                    State = State,
                    ZipCode = ZipCode,
                    Phone = Phone,
                };
        }

        private void UpdateModel(Address address)
        {
            EntityId = address.Id;
            FirstName = address.FirstName;
            MiddleInitial = address.MiddleInitial;
            LastName = address.LastName;
            StreetAddress = address.StreetAddress;
            OptionalAddress = address.OptionalAddress;
            City = address.City;
            State = address.State;
            ZipCode = address.ZipCode;
            Phone = address.Phone;
        }

        private async Task PopulateStatesAsync()
        {
            var items = new List<ComboBoxItemValue> { new ComboBoxItemValue() { Id = string.Empty, Value = "State" } };
            var states = await _locationService.GetStatesAsync();

            items.AddRange(states.Select(state => new ComboBoxItemValue() { Id = state, Value = state }));
            States = new ReadOnlyCollection<ComboBoxItemValue>(items);

            // We need to set the default empty value to the State property
            // but we don't want to fire the validation (we cannot use the property setter)
            _address.State = States.First().Id;
            OnPropertyChanged("State");
        }

        private void ValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            var allErrors = Validator.GetAllErrors();
            CurrentFormStatus = allErrors.Values.Count > 0 ? FormStatus.Invalid : FormStatus.Incomplete;
        }
    }
}
