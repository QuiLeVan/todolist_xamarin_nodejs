using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using todolist.Helpers;
using todolist.Models.Token;
using todolist.Models.User;
using todolist.Services.Dialog;
using todolist.Services.RequestProvider;
using todolist.Services.Settings;
using todolist.Validations;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------
        #region VARIABLE
        private const string ApiUrlBase = "auth/local";
        //variable
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _password;
        private bool _isMock;
        private bool _isValid;
        private bool _isLogin;
        //service
        private ISettingsService _settingsService;
        private IDialogService _dialogService;
        private IRequestProvider _requestProvider;

        /// <summary>
        /// 
        /// Binding Variable
        /// </summary>
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
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------


        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR ICommand Function
        ///------------------------------------------------------------------------
        #region ICommand Function
        public ICommand SignInCommand => new Command(async () => await SignInAsync());

        public ICommand MockSignInCommand => new Command(async () => await MockSignInAsync());

        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());

        public ICommand OpenSettingViewCommand => new Command(async () => await OpenSettingView());
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR ICommand Function
        ///------------------------------------------------------------------------

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
            IRequestProvider requestProvider,
            IDialogService dialogService
        )
        {
            _settingsService = settingsService;
            _dialogService = dialogService;
            _requestProvider = requestProvider;

            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            InvalidateMock();
            AddValidations();
        }

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR LOGIC FUNCTION
        ///------------------------------------------------------------------------
        #region LOGIC FUNCTION
        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A email is required." });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = " Need correct format of email" });
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
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR LOGIC FUNCTION
        ///------------------------------------------------------------------------


        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR FUNC OF COMMAND
        ///------------------------------------------------------------------------
        #region FUNC OF COMMAND
        private async Task OpenSettingView()
        {
            await NavigationService.NavigateToAsync<SettingsViewModel>();
        }

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

        private async Task SignInAsync()
        {
            IsBusy = true;
            IsValid = true;

            bool isValid = Validate();
            bool isAuthenticated = false;

            if (isValid)
            {
                try
                {
                    UserLoginInfo userLoginInfo = new UserLoginInfo();
                    userLoginInfo.Email = _email.Value;
                    userLoginInfo.Password = _password.Value;

                    var uri = UriHelper.CombineUri(GlobalSetting.Instance.EndpointBase, ApiUrlBase);
                    string data = JsonConvert.SerializeObject(userLoginInfo);

                    UserToken userToken = await _requestProvider.PostAsync<UserToken>(uri, data, null, null);

                    if (userToken.AccessToken != null)
                    {
                        _settingsService.AuthAccessToken = userToken.AccessToken;
                        isAuthenticated = true;
                    }
                    
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
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR FUNC OF COMMAND
        ///------------------------------------------------------------------------
    }
}
