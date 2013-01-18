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
using System.Net;
using System.Net.Http;
using System;

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
        // <snippet509>
        public UserValidationResult GetIsValid(string id, string password)
        {
            var result = new UserValidationResult();

            //To properly validate user credentials, you must validate both username and password.
            //Password validation is intentially omitted here to simplify the deployment of this sample service.
            result.IsValid = identities.ContainsKey(id);

            if (result.IsValid)
            {
                if (HttpContext.Current != null) // TODO - hack to avoid null ref in unit test, should find way to make this happy in the unit test instead
                    FormsAuthentication.SetAuthCookie(id, false);

                result.UserInfo = new UserInfo { UserName = id };
            }

            return result;
        }
        // </snippet509>

        // GET /api/Identity/GetIsValidSession
        [Authorize]
        public bool GetIsValidSession()
        {
            return true;
        }
    }
}
