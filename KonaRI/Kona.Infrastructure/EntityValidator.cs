// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Kona.Infrastructure
{
    [DataContract]
    public class EntityValidator : INotifyPropertyChanged
    {
        [DataMember]
        private readonly object _entityToValidate;

        [DataMember]
        private IDictionary<string, ReadOnlyCollection<string>> _errors = new Dictionary<string, ReadOnlyCollection<string>>();

        public static readonly ReadOnlyCollection<string> EmptyErrorsCollection = new ReadOnlyCollection<string>(new List<string>());

        public EntityValidator(object entityToValidate)
        {
            _entityToValidate = entityToValidate;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyCollection<string> this[string propertyName]
        {
            get
            {
                return _errors.ContainsKey(propertyName) ? _errors[propertyName] : EmptyErrorsCollection;
            }
        }

        public IDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return _errors;
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> errors)
        {
            if (_errors != errors)
            {
                _errors = errors;
                OnPropertyChanged("Item[]");
                OnErrorsChanged(string.Empty);
            }
        }

        // <snippet1309>
        public bool ValidateProperty(string propertyName)
        {
            var propertyValue = _entityToValidate.GetType().GetRuntimeProperty(propertyName).GetValue(_entityToValidate);
            return ValidateProperty(propertyName, propertyValue);
        }

        public bool ValidateProperty(string propertyName, object propertyValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_entityToValidate) { MemberName = propertyName };

            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);
            bool errorsChanged = SetAllErrors(propertyName, results.Select(c => c.ErrorMessage).ToList());

            if (errorsChanged)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return isValid;
        }
        // </snippet1309>

        public async Task<bool> ValidatePropertyAsync(string propertyName)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_entityToValidate) { MemberName = propertyName };
            var propertyValue = _entityToValidate.GetType().GetRuntimeProperty(propertyName).GetValue(_entityToValidate);

            bool isValid = await AsyncValidator.TryValidatePropertyAsync(propertyValue, context, results);
            bool errorsChanged = SetAllErrors(propertyName, results.Select(c => c.ErrorMessage).ToList());

            if (errorsChanged)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return isValid;
        }

        public async Task<bool> ValidatePropertiesAsync()
        {
            // Get all the properties decorated with a ValidationAttribute or an AsyncValidationAttribute
            var propertiesToValidate = _entityToValidate.GetType()
                                                        .GetRuntimeProperties()
                                                        .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any() || c.GetCustomAttributes(typeof(AsyncValidationAttribute)).Any());
            var propertiesWithNewErrors = new List<string>();

            foreach (var property in propertiesToValidate)
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(_entityToValidate) { MemberName = property.Name };

                // Check for either sync validation or async validation
                Validator.TryValidateProperty(property.GetValue(_entityToValidate), context, results);
                await AsyncValidator.TryValidatePropertyAsync(property.GetValue(_entityToValidate), context, results);
                
                // Check if the errors for that property have changed
                bool errorsChanged = SetAllErrors(property.Name, results.Select(c => c.ErrorMessage).ToList());
                if (errorsChanged)
                {
                    propertiesWithNewErrors.Add(property.Name);
                }
            }

            // Raise an event for each property that contains new errors
            foreach (string propertyName in propertiesWithNewErrors)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return _errors.Values.Count == 0;
        }

        private bool SetAllErrors(string propertyName, IList<string> propertyNewErrors)
        {
            bool errorsChanged = false;

            if (!_errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                {
                    _errors.Add(propertyName, new ReadOnlyCollection<string>(propertyNewErrors));
                    errorsChanged = true;
                }
            }
            else
            {
                if (propertyNewErrors.Count != _errors[propertyName].Count
                    || _errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count())
                {
                    if (propertyNewErrors.Count > 0)
                    {
                        _errors[propertyName] = new ReadOnlyCollection<string>(propertyNewErrors);
                    }
                    else
                    {
                        _errors.Remove(propertyName);
                    }

                    errorsChanged = true;
                }
            }

            return errorsChanged;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            var eventHandler = ErrorsChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }

    public static class AsyncValidator
    {
        public static async Task<bool> TryValidatePropertyAsync(object value, ValidationContext validationContext, ICollection<ValidationResult> validationResults)
        {
            bool propertyIsValid = true;
            PropertyInfo propertyInfo = validationContext.ObjectType.GetRuntimeProperty(validationContext.MemberName);
            IEnumerable<Attribute> asyncValidationAttributes = propertyInfo.GetCustomAttributes(typeof(AsyncValidationAttribute));
            var propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance);

            foreach (AsyncValidationAttribute asyncValidationAttribute in asyncValidationAttributes)
            {
                // TODO: handle cancellation (timeout)
                ValidationResult result = await asyncValidationAttribute.GetValidationResultAsync(propertyValue, validationContext);

                if (result != ValidationResult.Success)
                {
                    validationResults.Add(result);
                }

                propertyIsValid = propertyIsValid && result == ValidationResult.Success;
            }

            return propertyIsValid;
        }
    }
}
