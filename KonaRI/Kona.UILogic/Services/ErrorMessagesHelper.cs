// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Kona.UILogic.Services
{
    public static class ErrorMessagesHelper
    {
        private static ResourceLoader _resourceLoader = new ResourceLoader();

        public static string FirstNameRequired
        {
            get { return _resourceLoader.GetString("ErrorFirstNameRequired"); }
        }

        public static string MiddleInitialRequired
        {
            get { return _resourceLoader.GetString("ErrorMiddleInitialRequired"); }
        }

        public static string LastNameRequired
        {
            get { return _resourceLoader.GetString("ErrorLastNameRequired"); }
        }

        public static string AddressRequired
        {
            get { return _resourceLoader.GetString("ErrorAddressRequired"); }
        }

        public static string CityRequired
        {
            get { return _resourceLoader.GetString("ErrorCityRequired"); }
        }

        public static string StateRequired
        {
            get { return _resourceLoader.GetString("ErrorStateRequired"); }
        }

        public static string ZipCodeRequired
        {
            get { return _resourceLoader.GetString("ErrorZipCodeRequired"); }
        }

        public static string ZipCodeState
        {
            get { return _resourceLoader.GetString("ErrorZipCodeState"); }
        }

        public static string PhoneRequired
        {
            get { return _resourceLoader.GetString("ErrorPhoneRequired"); }
        }

        public static string CardNumberRequired
        {
            get { return _resourceLoader.GetString("ErrorCardNumberRequired"); }
        }

        public static string CardNumberInvalidLength
        {
            get { return _resourceLoader.GetString("ErrorCardNumberInvalidLength"); }
        }

        public static string CardholderNameRequired
        {
            get { return _resourceLoader.GetString("ErrorCardholderNameRequired"); }
        }

        public static string ExpirationMonthRequired
        {
            get { return _resourceLoader.GetString("ErrorExpirationMonthRequired"); }
        }

        public static string ExpirationYearRequired
        {
            get { return _resourceLoader.GetString("ErrorExpirationYearRequired"); }
        }

        public static string CardVerificationCodeRequired
        {
            get { return _resourceLoader.GetString("ErrorCardVerificationCodeRequired"); }
        }
    }
}
