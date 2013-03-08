// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using AdventureWorks.UILogic.Services;
using System;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;

namespace AdventureWorks.AWShopper.Services
{
    public class SecondaryTileService : ISecondaryTileService
    {
        private Uri _squareLogoUri = new Uri("ms-appx:///Assets/Logo.png");
        private Uri _wideLogoUri = new Uri("ms-appx:///Assets/WideLogo.scale-100.png");

        public bool SecondaryTileExists(string tileId)
        {
            return SecondaryTile.Exists(tileId);
        }

        public async Task<bool> PinSquareSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            if (!SecondaryTileExists(tileId))
            {
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnLogo, _squareLogoUri, null);
                bool isPinned = await secondaryTile.RequestCreateAsync();
                
                return isPinned;
            }

            return true;
        }

        public async Task<bool> PinWideSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            if (!SecondaryTileExists(tileId))
            {
                // <snippet808>
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnWideLogo, _squareLogoUri, _wideLogoUri);
                // </snippet808>
                // <snippet809>
                bool isPinned = await secondaryTile.RequestCreateAsync();
                // </snippet809>

                return isPinned;
            }

            return true;
        }

        public async Task<bool> UnpinTile(string tileId)
        {
            if (SecondaryTileExists(tileId))
            {
                // <snippet810>
                var secondaryTile = new SecondaryTile(tileId);
                // </snippet810>
                // <snippet811>
                bool isUnpinned = await secondaryTile.RequestDeleteAsync();
                // </snippet811>
                return isUnpinned;
            }

            return true;
        }

        public void ActivateTileNotifications(string tileId, Uri tileContentUri, PeriodicUpdateRecurrence recurrence)
        {
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile(tileId);
            tileUpdater.StartPeriodicUpdate(tileContentUri, recurrence);
        }
    }
}
