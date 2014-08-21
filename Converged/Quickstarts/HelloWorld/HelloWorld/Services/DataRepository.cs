// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.Practices.Prism.Mvvm.Interfaces;

namespace HelloWorld.Services
{
    public class DataRepository : IDataRepository
    {
        private const string UserEnteredData = "UserEnteredData";
        ISessionStateService _sessionStateService;

        public DataRepository(ISessionStateService sessionStateService)
        {
            _sessionStateService = sessionStateService;
        }

        public List<string> GetFeatures()
        {
            return new List<string>
            {
                "Application structuring with MVVM and dependencies",
                "Page navigation with ViewModel participation and navigation commanding",
                "Application state management through suspend, terminate, and resume",                
                "Loosely coupled communications with Commands and Pub/Sub events"
            };
        }

        public string GetUserEnteredData()
        {
            return _sessionStateService.SessionState.ContainsKey(UserEnteredData)
                       ? _sessionStateService.SessionState[UserEnteredData] as string
                       : string.Empty;
        }

        public void SetUserEnteredData(string data)
        {
            _sessionStateService.SessionState[UserEnteredData] = data;
        }
    }
}
