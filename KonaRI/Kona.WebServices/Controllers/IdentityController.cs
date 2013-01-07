// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Web;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Security;
using Kona.WebServices.Models;

namespace Kona.WebServices.Controllers
{
    public class IdentityController : ApiController
    {
        private static readonly Dictionary<string, string> identities = new Dictionary<string, string>
            {
                {"JohnDoe", "pwd"},
                {"user", "pwd"}
            };

        // GET /api/Identity/id?password={password}
        public UserValidationResult GetIsValid(string id, string password)
        {
            var result = new UserValidationResult();

            result.IsValid = identities.ContainsKey(id) && identities[id] == password;

            if (result.IsValid)
            {
                if (HttpContext.Current != null) // TODO - hack to avoid null ref in unit test, should find way to make this happy in the unit test instead
                    FormsAuthentication.SetAuthCookie(id, false);
                result.UserInfo = new UserInfo()
                {
                    UserName = id
                };
            }

            return result;
        }

        // GET /api/Identity/GetIsValidSession
        [Authorize]
        public bool GetIsValidSession()
        {
            return true;
        }
    }
}
