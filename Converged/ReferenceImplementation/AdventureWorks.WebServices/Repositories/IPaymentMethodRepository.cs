// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Collections.Generic;
using AdventureWorks.WebServices.Models;

namespace AdventureWorks.WebServices.Repositories
{
    public interface IPaymentMethodRepository
    {
        IEnumerable<PaymentMethod> GetAll(string userName);
        void AddUpdate(string userName, PaymentMethod paymentMethod);
        void SetDefault(string userName, string defaultPaymentMethodId);
    }
}