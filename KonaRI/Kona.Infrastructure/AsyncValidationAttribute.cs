// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace Kona.Infrastructure
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class AsyncValidationAttribute : Attribute
    {
        public AsyncValidationAttribute(Type validatorType, string method)
        {
            Method = method;
            ValidatorType = validatorType;
        }

        public string Method { get; private set; }

        public Type ValidatorType { get; private set; }

        public Task ValidateAsync(object value, ValidationContext validationContext)
        {
            return IsValid(value, validationContext);
        }

        public Task<ValidationResult> GetValidationResultAsync(object value, ValidationContext validationContext)
        {
            return IsValid(value, validationContext);
        }

        private Task<ValidationResult> IsValid(object value, ValidationContext validationContext)
        {
            if (ValidatorType == null)
            {
                string errorMessage = "The AsyncValidationAttribute.ValidatorType was not specified";
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, errorMessage));
            }

            if (Method == null)
            {
                string errorMessage = "The AsyncValidationAttribute.Method was not specified";
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, errorMessage));
            }

            Type[] methodParameters = new[] { typeof(object), typeof(ValidationContext) };
            var validationMethod = ValidatorType.GetRuntimeMethod(Method, methodParameters);

            if (validationMethod == null || !validationMethod.IsStatic || validationMethod.ReturnType != typeof(Task<ValidationResult>))
            {
                string errorMessage = "The AsyncValidationAttribute method '{0}' does not exist in type '{1}' or is not public and static and does not return Task<ValidationResult>";
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, errorMessage, Method, ValidatorType));
            }

            Task<ValidationResult> result = (Task<ValidationResult>)validationMethod.Invoke(null, new[] { value, validationContext }); // null because is a static method

            return result;
        }
    }
}
