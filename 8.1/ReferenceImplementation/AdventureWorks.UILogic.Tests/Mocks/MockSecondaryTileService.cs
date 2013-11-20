// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Services;
using Windows.UI.Notifications;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockSecondaryTileService : ISecondaryTileService
    {
        public Func<string, bool> SecondaryTileExistsDelegate { get; set; }
        public Func<string, string, string, Task<bool>> PinSquareSecondaryTileDelegate { get; set; }
        public Func<string, string, string, Task<bool>> PinWideSecondaryTileDelegate { get; set; }
        public Func<string, Task<bool>> UnpinTileDelegate { get; set; }
        public Action<string, Uri, PeriodicUpdateRecurrence> ActivateTileNotificationsDelegate { get; set; }

        public bool SecondaryTileExists(string tileId)
        {
            return SecondaryTileExistsDelegate(tileId);
        }

        public Task<bool> PinSquareSecondaryTile(string tileId, string displayName, string arguments)
        {
            return PinSquareSecondaryTileDelegate(tileId, displayName, arguments);
        }

        public Task<bool> PinWideSecondaryTile(string tileId, string displayName, string arguments)
        {
            return PinWideSecondaryTileDelegate(tileId, displayName, arguments);
        }

        public Task<bool> UnpinTile(string tileId)
        {
            return UnpinTileDelegate(tileId);
        }

        public void ActivateTileNotifications(string tileId, Uri tileContentUri, PeriodicUpdateRecurrence recurrence)
        {
            ActivateTileNotificationsDelegate(tileId, tileContentUri, recurrence);
        }
    }
}
