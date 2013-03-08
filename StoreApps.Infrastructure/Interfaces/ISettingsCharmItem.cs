// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


namespace Microsoft.Practices.StoreApps.Infrastructure.Interfaces
{
    /// <summary>
    /// This interface is used for unifiying different types of Setting Charm Items that the Settings Charm Service accepts,
    /// specifically the SettingsCharmFlyoutItem and the SettingsCharmLinkItem.
    /// </summary>
    public interface ISettingsCharmItem
    {
        /// <summary>
        /// Gets the id of the settings charm item.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        string Id { get; }

        /// <summary>
        /// Gets the title of the settings charm item.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        string Title { get; }
    }
}
