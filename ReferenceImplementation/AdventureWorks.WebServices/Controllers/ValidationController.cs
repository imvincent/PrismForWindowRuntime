// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Globalization;
using AdventureWorks.WebServices.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Controllers
{
    public class ValidationController : ApiController
    {
        // POST /api/validation/validateaddress

        [HttpPost]
        [ActionName("validateaddress")]
        public HttpResponseMessage ValidateAddress(Address address)
        {
            if (address == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidAddress);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
