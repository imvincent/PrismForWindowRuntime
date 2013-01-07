// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Kona.Infrastructure;
using ValidationQuickStart.Models;

namespace ValidationQuickStart.ViewModels
{
    public class UserInfoViewModel : ViewModel
    {
        private readonly IUserInfo _userInfo;
        private readonly EntityValidator _validator;
        private ReadOnlyCollection<string> _allErrors;

        // <snippet1306> 
        public UserInfoViewModel()
        {
            _userInfo = new UserInfo();
            _validator = new EntityValidator(UserInfo);
            _allErrors = EntityValidator.EmptyErrorsCollection;
            _validator.ErrorsChanged += ValidatorErrorsChanged;
            ValidateCommand = new DelegateCommand(async () => await ValidateAsync());
        }
        // </snippet1306> 

        public UserInfoViewModel(IUserInfo userInfo)
        {
            _userInfo = userInfo;
            _validator = new EntityValidator(userInfo);
            _allErrors = EntityValidator.EmptyErrorsCollection;
            _validator.ErrorsChanged += ValidatorErrorsChanged;
            ValidateCommand = new DelegateCommand(async () => await ValidateAsync());
        }

        public IUserInfo UserInfo
        {
            get { return _userInfo; }
        }

        public EntityValidator Validator
        {
            get { return _validator; }
        }

        // <snippet1307>
        public string FirstName
        {
            get { return UserInfo.FirstName; }
            set
            {
                if (UserInfo.FirstName != value)
                {
                    UserInfo.FirstName = value;
                    OnPropertyChanged("FirstName");
                    _validator.ValidateProperty("FirstName");

                    // We also check if the Middle Name is valid
                    _validator.ValidateProperty("MiddleName");
                }
            }
        }
        // </snippet1307>

        public string MiddleName
        {
            get { return UserInfo.MiddleName; }
            set
            {
                if (UserInfo.MiddleName != value)
                {
                    UserInfo.MiddleName = value;
                    OnPropertyChanged("MiddleName");
                    _validator.ValidateProperty("MiddleName");
                }
            }
        }

        public string LastName
        {
            get { return UserInfo.LastName; }
            set
            {
                if (UserInfo.LastName != value)
                {
                    UserInfo.LastName = value;
                    OnPropertyChanged("LastName");
                    _validator.ValidateProperty("LastName");
                }
            }
        }

        public ReadOnlyCollection<string> AllErrors
        {
            get { return _allErrors; }
            private set { SetProperty(ref _allErrors, value); }
        }

        public ICommand ValidateCommand { get; set; }

        public async Task ValidateAsync()
        {
            await _validator.ValidatePropertiesAsync();
        }

        // <snippet1308>
        private void ValidatorErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AllErrors = new ReadOnlyCollection<string>(Validator.GetAllErrors().Values.SelectMany(c => c).ToList());
        }
        // </snippet1308>
    }
}
