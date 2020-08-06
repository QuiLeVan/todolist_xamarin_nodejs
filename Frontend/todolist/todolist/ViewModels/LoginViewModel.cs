using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using todolist.Services.Dialog;
using todolist.Services.Settings;
using todolist.Validations;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region variable
        //variable
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _password;
        private bool _isMock;
        private bool _isValid;
        private bool _isLogin;
        //service
        private ISettingsService _settingsService;
        private IDialogService _dialogService;

        //Binding Variable
        public ValidatableObject<string> Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                RaisePropertyChanged(() => Email);
            }
        }

        public ValidatableObject<string> Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        public bool IsMock
        {
            get
            {
                return _isMock;
            }
            set
            {
                _isMock = value;
                RaisePropertyChanged(() => IsMock);
            }
        }

        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        #endregion

        #region ICommand Function

        public ICommand MockSignInCommand => new Command(async () => await MockSignInAsync());

        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        #endregion

        public override Task InitializeAsync(object navigationData)
        {
            //if (navigationData is LogoutParameter)
            //{
            //    var logoutParameter = (LogoutParameter)navigationData;

            //    if (logoutParameter.Logout)
            //    {
            //        Logout();
            //    }
            //}

            return base.InitializeAsync(navigationData);
        }

        public LoginViewModel(
            ISettingsService settingsService,
            IDialogService dialogService
        )
        {
            _settingsService = settingsService;
            _dialogService = dialogService;

            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            InvalidateMock();
            AddValidations();
        }

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A email is required." });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
        }

        public void InvalidateMock()
        {
            IsMock = _settingsService.UseMocks;
        }

        private bool ValidateEmail()
        {
            return _email.Validate();
        }

        private bool ValidatePassword()
        {
            return _password.Validate();
        }

        private bool Validate()
        {
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();

            return isValidEmail && isValidPassword;
        }

        #region COMMAND FUNCTION

        private async Task MockSignInAsync()
        {
            IsBusy = true;
            IsValid = true;

            bool isValid = Validate();
            bool isAuthenticated = false;

            if (isValid)
            {
                try
                {
                    await Task.Delay(10);

                    isAuthenticated = true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"[SignIn] Error signing in: {ex}");
                }
            }
            else
            {
                IsValid = false;
            }

            if (isAuthenticated)
            {
                //_settingsService.AuthAccessToken = GlobalSetting.Instance.AuthToken;

                await NavigationService.NavigateToAsync<MainViewModel>();
                await NavigationService.RemoveLastFromBackStackAsync();
            }

            IsBusy = false;

        }

        

        #endregion
    }
}
