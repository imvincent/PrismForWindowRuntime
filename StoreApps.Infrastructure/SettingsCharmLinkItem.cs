// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using Microsoft.Practices.StoreApps.Infrastructure.Interfaces;
using System;

namespace Microsoft.Practices.StoreApps.Infrastructure
{
    /// <summary>
    /// The SettingsCharmLinkItem is one of the item types, that implement ISettingsCharmItem, used by the SettingsCharmService class to populate the SettingsCharm.
    /// This item type has an associated Uri that will be opened when cliking it in the Settings Charm.
    /// </summary>
    public class SettingsCharmLinkItem : ISettingsCharmItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsCharmLinkItem"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="id">The id.</param>
        /// <param name="linkUri">The link URI.</param>
        public SettingsCharmLinkItem(string title, string id, Uri linkUri) 
        {
            Title = title;
            Id = id;
            LinkUri = linkUri;
        }

        /// <summary>
        /// Gets the id of the settings charm link item.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; private set; }

        /// <summary>
        /// Gets the title of the settings charm link item. This is the title that will be shown in the Settings Charm.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; private set; }

        /// <summary>
        /// Gets the URI that will be opended when this item is clicked in the Settings Charm.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public Uri LinkUri { get; private set; }
    }
}
