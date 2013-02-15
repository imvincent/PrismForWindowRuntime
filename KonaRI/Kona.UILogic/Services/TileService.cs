// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Threading.Tasks;
using Windows.UI.StartScreen;

namespace Kona.UILogic.Services
{
    public class TileService : ITileService
    {
        private IAssetsService _assetsService;

        public TileService(IAssetsService assetsService)
        {
            _assetsService = assetsService;
        }

        public bool SecondaryTileExists(string tileId)
        {
            return SecondaryTile.Exists(tileId);
        }

        public async Task<bool> PinSquareSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            if (!SecondaryTileExists(tileId))
            {
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnLogo, _assetsService.GetSquareLogoUri(), null);
                bool isPinned = await secondaryTile.RequestCreateAsync();
                
                return isPinned;
            }

            return true;
        }

        public async Task<bool> PinWideSecondaryTile(string tileId, string shortName, string displayName, string arguments)
        {
            if (!SecondaryTileExists(tileId))
            {
                var secondaryTile = new SecondaryTile(tileId, shortName, displayName, arguments, TileOptions.ShowNameOnWideLogo, _assetsService.GetSquareLogoUri(), _assetsService.GetWideLogoUri());
                bool isPinned = await secondaryTile.RequestCreateAsync();

                return isPinned;
            }

            return true;
        }

        public async Task<bool> UnpinTile(string tileId)
        {
            if (SecondaryTileExists(tileId))
            {
                var secondaryTile = new SecondaryTile(tileId);

                bool isUnpinned = await secondaryTile.RequestDeleteAsync();
                return isUnpinned;
            }

            return true;
        }
    }
}
