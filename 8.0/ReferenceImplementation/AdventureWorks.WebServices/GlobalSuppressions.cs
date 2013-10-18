// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the 
// Code Analysis results, point to "Suppress Message", and click 
// "In Suppression File".
// You do not need to add suppressions to this file manually.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "AdventureWorks.WebServices", Justification = "The only class in this namespace is template generated.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Mvc", Scope = "type", Target = "AdventureWorks.WebServices.MvcApplication", Justification = "Mvc is a standard term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.MvcApplication.#Application_Start()", Justification = "Standard template code for Mvc application.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.IdentityController.#GetPasswordChallenge(System.String)", Justification = "Mvc action methods cannot be static.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.IdentityController.#GetIsValid(System.String,System.String,System.String)", Justification = "Mvc action methods cannot be static.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.IdentityController.#GetIsValidSession()", Justification = "Mvc action methods cannot be static.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.TileNotificationController.#GetTileNotification()", Justification = "Mvc action methods cannot be static.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.ShoppingCartController.#Reset(System.Boolean)", Justification = "Mvc action methods cannot be static.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.IdentityController.#GetIsValidSession()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Repositories.IProductRepository.#GetTodaysDealsProducts()", Justification = "This method performs a database operation.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Repositories.IProductRepository.#GetProducts()", Justification = "This method performs a database operation.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Repositories.IRepository`1.#GetAll()", Justification = "This method performs a database operation.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.LocationController.#GetStates()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.OrderController.#CreateOrder(AdventureWorks.WebServices.Models.Order)", Justification = "Caller manages lifetime of HttpResponseMessage.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope = "member", Target = "AdventureWorks.WebServices.Repositories.ProductRepository.#PopulateProducts()", Justification = "Stubbed data functionaltiy.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Scope = "member", Target = "AdventureWorks.WebServices.Repositories.ProductRepository.#PopulateProducts()", Justification = "Stubbed data functionaltiy.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.ShippingMethodController.#GetShippingMethods()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.ShippingMethodController.#GetBasicShippingMethod()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.TileNotificationController.#GetTileNotification()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.TileNotificationController.#GetTileNotification()", Justification = "Caller manages lifetime of HttpResponseMessage.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.TileNotificationController.#GetSecondaryTileNotification(System.String)", Justification = "Caller manages lifetime of HttpResponseMessage.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Api", Scope = "type", Target = "AdventureWorks.WebServices.WebApiConfig", Justification = "Api is a standard term.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.AddressController.#GetAll()", Justification = "Mvc action methods cannot be properties.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope = "member", Target = "AdventureWorks.WebServices.Controllers.PaymentMethodController.#GetAll()", Justification = "Mvc action methods cannot be properties.")]
