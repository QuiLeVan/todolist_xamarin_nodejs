using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using todolist.Helpers;
using todolist.Models.Token;
using todolist.Models.User;
using todolist.Services.RequestProvider;
using todolist.Services.Settings;
using todolist.Validations;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class RegisterViewModel:ViewModelBase
    {
        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR Variable
        ///------------------------------------------------------------------------
        #region Variable
        private readonly string ApiUrlBase = "auth/local/register";
        private ValidatableObject<string> _userName;
        private ValidatableObject<string> _email;
        private ValidatableObject<string> _password;
        private bool _isValid;

        private IRequestProvider _requestProvider;
        private ISettingsService _settingsService;
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR Variable
        ///------------------------------------------------------------------------
        ///

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR BINDING DATA
        ///------------------------------------------------------------------------
        #region BINDING DATA

        public ValidatableObject<string> UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                RaisePropertyChanged(() => UserName);
            }
        }

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
        /// [END] DEFINE FOR BINDING DATA
        ///------------------------------------------------------------------------

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR ICommand
        ///------------------------------------------------------------------------
        #region ICommand

        public ICommand SwithLoginCommand => new Command(async () => await SwitchLoginAsync());

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());

        public ICommand ValidateUserCommand => new Command(() => ValidateUserName());

        public ICommand ValidateEmailCommand => new Command(() => ValidateEmail());

        public ICommand ValidatePasswordCommand => new Command(() => ValidatePassword());
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR ICommand
        ///------------------------------------------------------------------------
        ///

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        #region LOGIC FUNC

        private void AddValidations()
        {
            _email.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A email is required." });
            _email.Validations.Add(new EmailRule<string> { ValidationMessage = " Need correct format of email" });
            _password.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A password is required." });
            _userName.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A user name is required. " });
        }

        private bool ValidateUserName()
        {
            return _userName.Validate();
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
            bool isValidUserName = ValidateUserName();
            bool isValidEmail = ValidateEmail();
            bool isValidPassword = ValidatePassword();

            return isValidUserName && isValidEmail && isValidPassword;
        }

        private async Task RegisterAsync()
        {
            IsBusy = true;
            IsValid = true;

            bool isValid = Validate();
            bool isRegisteredSuccess = false;

            if (isValid)
            {
                try
                {
                    UserRegisterInfo userRegisterInfo = new UserRegisterInfo();
                    userRegisterInfo.UserName = _userName.Value;
                    userRegisterInfo.Email = _email.Value;
                    userRegisterInfo.Password = _password.Value;

                    var uri = UriHelper.CombineUri(GlobalSetting.Instance.EndpointBase, ApiUrlBase);
                    string data = JsonConvert.SerializeObject(userRegisterInfo);

                    UserToken userToken = await _requestProvider.PostAsync<UserToken>(uri, data, null, null);

                    if (userToken.AccessToken != null)
                    {
                        _settingsService.AuthAccessToken = GlobalSetting.Instance.AuthToken = userToken.AccessToken;
                        isRegisteredSuccess = true;
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

            if (isRegisteredSuccess)
            {
                await NavigationService.NavigateToAsync<MainViewModel>();
                await NavigationService.RemoveLastFromBackStackAsync();
            }

            IsBusy = false;
        }

        private async Task SwitchLoginAsync()
        {
            await NavigationService.NavigateToAsync<LoginViewModel>();
            await NavigationService.RemoveBackStackAsync();
        }

        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        ///

        public override Task InitializeAsync(object navigationData)
        {
            return base.InitializeAsync(navigationData);
        }

        public RegisterViewModel(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            _requestProvider = requestProvider;
            _settingsService = settingsService;

            _userName = new ValidatableObject<string>();
            _email = new ValidatableObject<string>();
            _password = new ValidatableObject<string>();

            AddValidations();
        }
    }
}
