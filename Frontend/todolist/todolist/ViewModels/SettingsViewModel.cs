using System;
using System.Threading.Tasks;
using System.Windows.Input;
using todolist.Services.Settings;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class SettingsViewModel: ViewModelBase
    {
        /// <summary>
        /// VARIABLE
        /// </summary>
        #region Define Var
        private bool _useNodeServices;
        private string _baseEndpoint;

        private readonly ISettingsService _settingsService;
        #endregion

        /// <summary>
        /// BINDING PROPERTY
        /// </summary>
        #region Define Property Binding
        public bool UseNodeServices
        {
            get => _useNodeServices;
            set
            {
                _useNodeServices = value;
                UpdateUseNodeServices();
                RaisePropertyChanged(() => UseNodeServices);
            }
        }

        public string TitleUseNodeServices
        {
            get { return !UseNodeServices ? "Use Mock Services" : "Use Microservices"; }
        }

        public string DescriptionUseNodeServices
        {
            get
            {
                return !UseNodeServices
                    ? "Mock Services are simulated objects that mimic the behavior of real services using a controlled approach."
                        : "When enabling the use of microservices/containers, the app will attempt to use real services deployed as Docker/Kubernetes containers at the specified base endpoint, which will must be reachable through the network.";
            }
        }

        public string BaseEndpoint
        {
            get => _baseEndpoint;
            set
            {
                _baseEndpoint = value;
                if (!string.IsNullOrEmpty(_baseEndpoint))
                {
                    UpdateBaseEndpoint();
                }
                RaisePropertyChanged(() => BaseEndpoint);
            }
        }

        public bool UserIsLogged => !string.IsNullOrEmpty(_settingsService.AuthAccessToken);



        #endregion

        /// <summary>
        /// ICommand DEFINE
        /// </summary>
        #region ICommand Define

        public ICommand ToggleMockServicesCommand => new Command(async () => await ToggleMockServicesAsync());

        

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            _useNodeServices = !_settingsService.UseMocks;
            _baseEndpoint = _settingsService.EndpointBase;
        }


        /// <summary>
        /// LOGIC METHOD
        /// </summary>
        #region Method Logic

        private void UpdateUseNodeServices()
        {
            // Save use mocks services to local storage
            _settingsService.UseMocks = !_useNodeServices;
        }

        private void UpdateBaseEndpoint()
        {
            //Save Endpoint to local storage
            GlobalSetting.Instance.EndpointBase = _settingsService.EndpointBase = _baseEndpoint;
        }

        private async Task ToggleMockServicesAsync()
        {
            ViewModelLocator.UpdateDependencies(!_useNodeServices);
            RaisePropertyChanged(() => TitleUseNodeServices);
            RaisePropertyChanged(() => DescriptionUseNodeServices);

            var previousPageViewModel = NavigationService.PreviousPageViewModel;
            if (previousPageViewModel != null)
            {
                if (previousPageViewModel is MainViewModel)
                {
                    // Slight delay so that page navigation isn't instantaneous
                    await Task.Delay(1000);
                    if (_useNodeServices)
                    {
                        _settingsService.AuthAccessToken = string.Empty;
                        _settingsService.AuthIdToken = string.Empty;

                        await NavigationService.NavigateToAsync<LoginViewModel>(
                            null
                            //new LogoutParameter { Logout = true }
                        );
                        await NavigationService.RemoveBackStackAsync();
                    }
                }
            }
        }

        #endregion
    }
}
