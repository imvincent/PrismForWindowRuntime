// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// The SettingsCharmFlyoutItem is one of the item types, that implement ISettingsCharmItem, used by the SettingsCharmService class to populate the SettingsCharm.
    /// This item type has an associated flyout that will be opened when cliking it in the Settings Charm.
    /// </summary>
    public class SettingsCharmFlyoutItem : ISettingsCharmItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCharmFlyoutItem"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="flyoutName">Name of the flyout.</param>
        public SettingsCharmFlyoutItem(string title, string flyoutName) : this(title, flyoutName, flyoutName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCharmFlyoutItem"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="id">The id.</param>
        /// <param name="flyoutName">Name of the flyout.</param>
        public SettingsCharmFlyoutItem(string title, string id, string flyoutName)
        {
            Title = title;
            Id = id;
            FlyoutName = flyoutName;
        }

        /// <summary>
        /// Gets the id of the settings charm flyout item.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the title of the settings charm flyout item.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the name of the flyout that will be opened when this item is clicked in the Settings Charm.
        /// </summary>
        /// <value>
        /// The name of the flyout.
        /// </value>
        public string FlyoutName { get; private set; }
    }
}
