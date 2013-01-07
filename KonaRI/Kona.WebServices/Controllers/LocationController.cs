// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Web.Http;

namespace Kona.WebServices.Controllers
{
    public class LocationController : ApiController
    {
        //
        // GET: /api/Location/
        public ReadOnlyCollection<string> GetStates()
        {
            return new ReadOnlyCollection<string>(states);
        }

        //
        // GET: /api/Location?state={state}&zipCode={zipCode}
        public bool GetIsZipCodeValid(string state, string zipCode)
        {
            // This is where you would perform a proper zipCode + state validation.
            // This is an example of cross field validation.
            bool result = false;

            if (zipCode != null)
            {
                return result = zipCode.Length == 5;
            }

            return result;
        }

        private readonly static IList<string> states = new List<string>
                {
                    "AL",
                    "AK",
                    "AS",
                    "AZ",
                    "AR",
                    "CA",
                    "CO",
                    "CT",
                    "DE",
                    "DC",
                    "FL",
                    "GA",
                    "GU",
                    "HI",
                    "ID",
                    "IL",
                    "IN",
                    "IA",
                    "KS",
                    "KY",
                    "LA",
                    "ME",
                    "MD",
                    "MH",
                    "MA",
                    "MI",
                    "FM",
                    "MN",
                    "MS",
                    "MO",
                    "MT",
                    "NE",
                    "NV",
                    "NH",
                    "NJ",
                    "NM",
                    "NY",
                    "NC",
                    "ND",
                    "MP",
                    "OH",
                    "OK",
                    "OR",
                    "PW",
                    "PA",
                    "PR",
                    "RI",
                    "SC",
                    "SD",
                    "TN",
                    "TX",
                    "UT",
                    "VT",
                    "VA",
                    "VI",
                    "WA",
                    "WV",
                    "WI",
                    "WY"
                };
    }
}
