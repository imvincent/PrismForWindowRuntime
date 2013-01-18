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

        public static string RequiredErrorMessage
        {
            get { return _resourceLoader.GetString("ErrorRequired"); }
        }

        public static string RegexErrorMessage
        {
            get { return _resourceLoader.GetString("ErrorRegex"); }
        }

        public static string CardNumberInvalidLengthErrorMessage
        {
            get { return _resourceLoader.GetString("ErrorCardNumberInvalidLength"); }
        }

        public static string CardYearInvalidLengthErrorMessage
        {
            get { return _resourceLoader.GetString("ErrorCardYearInvalidLength"); }
        } 
        
        public static string CardMonthInvalid
        {
            get { return _resourceLoader.GetString("ErrorCardMonthInvalid"); }
        }

        public static string CardYearInvalid
        {
            get { return _resourceLoader.GetString("ErrorCardYearInvalid"); }
        }
    }
}
