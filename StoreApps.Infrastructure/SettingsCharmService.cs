// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using Windows.UI.ApplicationSettings;
using System.Linq;
using System;
using Windows.System;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// This service is used to populate the Settings Charm pane with the Charm items returned by the func passed to the constructor of this class.
    /// If the charm item is a <see cref="SettingsCharmFlyoutItem"/> it will use a IFlyoutService implementation to display that flyout.
    /// If the item is a <see cref="SettingsCharmLinkItem"/> it will launch the browser and browse to the specified link.
    /// </summary>
    public class SettingsCharmService
    {
        private readonly Func<IList<ISettingsCharmItem>> _getSettingsCharmItems;
        private readonly IFlyoutService _flyoutService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCharmService"/> class.
        /// </summary>
        /// <param name="getSettingsCharmItems">The function that returns the list of Settings Charm items.</param>
        /// <param name="flyoutService">The flyout service.</param>
        public SettingsCharmService(Func<IList<ISettingsCharmItem>> getSettingsCharmItems, IFlyoutService flyoutService)
        {
            _getSettingsCharmItems = getSettingsCharmItems;
            _flyoutService = flyoutService;
        }

        // <snippet519>
        /// <summary>
        /// Called when the Settings Charm is invoked, this handler populate the Settings Charm with the charm items returned by the getSettingCharm Items func.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="SettingsPaneCommandsRequestedEventArgs"/> instance containing the event data.</param>
        public void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null
                || _getSettingsCharmItems == null)
            {
                return;
            }

            var applicationCommands = args.Request.ApplicationCommands;
            var settingsCharmItems = _getSettingsCharmItems();

            foreach (var item in settingsCharmItems)
            {
                SettingsCommand cmd = applicationCommands.FirstOrDefault((s) => s.Id.ToString() == item.Id);

                if (cmd == null)
                {
                    // Switch between a SettingsCharmFlyoutItem or a SettingsCharmLinkItem
                    if (item.GetType() == typeof(SettingsCharmFlyoutItem))
                    {
                        var flyoutItem = item as SettingsCharmFlyoutItem;
                        cmd = new SettingsCommand(flyoutItem.Id, flyoutItem.Title, (o) => _flyoutService.ShowFlyout(flyoutItem.FlyoutName));
                    }

                    if (cmd == null && item.GetType() == typeof(SettingsCharmLinkItem))
                    {
                        var linkItem = item as SettingsCharmLinkItem;
                        cmd = new SettingsCommand(linkItem.Id, linkItem.Title, async (o) => await Launcher.LaunchUriAsync(linkItem.LinkUri));
                    }
                                           
                    applicationCommands.Add(cmd);
                }
            }
        }
        // </snippet519>
    }
}
