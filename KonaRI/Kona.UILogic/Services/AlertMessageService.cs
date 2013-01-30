// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Kona.UILogic.Services
{
    public class AlertMessageService : IAlertMessageService
    {
        private static bool _isShowing = false;

        public async Task ShowAsync(string message, string title)
        {
            //Only show one dialog at a time.
            if (!_isShowing)
            {
                _isShowing = true;
                MessageDialog dlg = new MessageDialog(message, title);
                await dlg.ShowAsync();
                _isShowing = false;
            }
        }
    }
}
