// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Linq;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.Shopper.Services
{
    public class AlertMessageService : IAlertMessageService
    {
        private static bool _isShowing = false;

        public async Task ShowAsync(string message, string title)
        {
            await ShowAsync(message, title, null);
        }

        public async Task ShowAsync(string message, string title, IEnumerable<DialogCommand> dialogCommands)
        {
            // Only show one dialog at a time.
            if (!_isShowing)
            {
                var messageDialog = new MessageDialog(message, title);
                
                if (dialogCommands != null)
                {
                    var commands = dialogCommands.Select(c => new UICommand(c.Label, (command) => c.Invoked(), c.Id));

                    foreach (var command in commands)
                    {
                        messageDialog.Commands.Add(command);
                    }
                }

                _isShowing = true;
                await messageDialog.ShowAsync();
                _isShowing = false;
            }
        }
    }
}
