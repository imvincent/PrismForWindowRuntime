// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Linq;
using Kona.Infrastructure.Interfaces;

namespace HelloWorld.Services
{
    public class DataRepository : IDataRepository
    {
        private const string UserEnteredData = "UserEnteredData";
        IRestorableStateService _stateService;

        public DataRepository(IRestorableStateService restorableStateService)
        {
            _stateService = restorableStateService;
        }

        public List<string> GetKonaFeatures()
        {
            return new List<string>
            {
                "Application structuring with MVVM and dependencies",
                "Page navigation with ViewModel participation and navigation commanding",
                "Application state management through suspend, terminate, and resume",
                "User input validation on client and server side with validation error displays",
                "Loosely coupled communications with Commands and Pub/Sub events"
            };
        }

        public string GetUserEnteredData()
        {
            return _stateService.GetState(UserEnteredData) as string;
        }

        public void SetUserEnteredData(string data)
        {
            _stateService.SaveState(UserEnteredData, data);
        }
    }
}
