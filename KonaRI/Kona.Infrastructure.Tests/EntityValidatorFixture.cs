// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Kona.Infrastructure.Tests
{
    [TestClass]
    public class EntityValidatorFixture
    {
        [TestMethod]
        public async Task Validate_Title_And_Description_Async()
        {
            var model = new MockModelWithSimpleAsyncValidation();
            var target = new EntityValidator(model);

            bool result = false;
            result = await target.ValidatePropertiesAsync();

            Assert.IsTrue(result);
            Assert.IsTrue(target.GetAllErrors().Values.Count == 0);
        }

        [TestMethod]
        public async Task Validate_Title_Async_Throws_An_Exception()
        {
            var model = new MockModelWithExceptionInAsyncValidation();
            var target = new EntityValidator(model);

            bool result = false;

            try
            {
                result = await target.ValidatePropertiesAsync();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.IsFalse(result);
            }
        }
    }


    public class MockModelWithSimpleAsyncValidation
    {
        [AsyncValidation(typeof(MockModelWithSimpleAsyncValidation), "ValidateTitleAsync")]
        public string Title { get; set; }

        [AsyncValidation(typeof(MockModelWithSimpleAsyncValidation), "ValidateDescriptionAsync")]
        public string Description { get; set; }

        public static async Task<ValidationResult> ValidateTitleAsync(object value, ValidationContext validationContext)
        {
            await Task.Delay(1000);
            return await Task.FromResult(ValidationResult.Success);
        }

        public static async Task<ValidationResult> ValidateDescriptionAsync(object value, ValidationContext validationContext)
        {
            await Task.Delay(1000);
            return await Task.FromResult(ValidationResult.Success);
        }
    }

    public class MockModelWithExceptionInAsyncValidation
    {
        [AsyncValidation(typeof(MockModelWithExceptionInAsyncValidation), "ValidateTitleAsync")]
        public string Title { get; set; }

        public static async Task<ValidationResult> ValidateTitleAsync(object value, ValidationContext validationContext)
        {
            throw new Exception("error while performing the async validation");
        }
    }
}
