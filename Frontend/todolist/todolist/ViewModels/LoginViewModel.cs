using System;
using System.Threading.Tasks;
using todolist.Services.Settings;
using todolist.ViewModels.Base;

namespace todolist.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //variable
        private bool _isMock;

        //service
        private ISettingsService _settingsService;

        //Binding Variable
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
            ISettingsService settingsService
        )
        {
            _settingsService = settingsService;

            InvalidateMock();
        }

        public void InvalidateMock()
        {
            IsMock = _settingsService.UseMocks;
        }
    }
}
