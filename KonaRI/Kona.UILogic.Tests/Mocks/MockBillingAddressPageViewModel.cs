// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kona.UILogic.Models;
using Kona.UILogic.ViewModels;
namespace Kona.UILogic.Tests.Mocks
{
    public class MockBillingAddressPageViewModel : IBillingAddressUserControlViewModel
    {
        public Func<Task<bool>> ValidateFormAsyncDelegate { get; set; }
        public Address GetAddressDelegate { get; set; }
        public Action ProcessFormDelegate { get; set; }

        public string FirstName { get; set; }

        public string MiddleInitial { get; set; }

        public string LastName { get; set; }

        public string StreetAddress { get; set; }

        public string OptionalAddress { get; set; }

        public string City { get; set; }

        public IReadOnlyCollection<ComboBoxItemValue> States { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public bool SaveAddress { get; set; }

        public bool SetAsDefault { get; set; }

        public string FirstError { get; set; }

        public int CurrentFormStatus { get; set; }

        public Address GetAddress()
        {
            return GetAddressDelegate;
        }

        public Infrastructure.EntityValidator Validator
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoBackCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public System.Windows.Input.ICommand GoNextCommand
        {
            get { throw new System.NotImplementedException(); }
        }

        public string EntityId { get; set; }


        public void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewState)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedFrom(System.Collections.Generic.Dictionary<string, object> viewState, bool suspending)
        {
            throw new System.NotImplementedException();
        }

        public void GoNext()
        {
            throw new System.NotImplementedException();
        }

        public void ProcessForm()
        {
            ProcessFormDelegate();
        }

        public Task<bool> ValidateFormAsync()
        {
            return ValidateFormAsyncDelegate();
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
