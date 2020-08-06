using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using todolist.Services;
using todolist.Views;
using todolist.Services.Settings;
using todolist.ViewModels.Base;
using System.Threading.Tasks;
using todolist.Services.Navigation;

namespace todolist
{
    public partial class App : Application
    {
        ISettingsService _settingsService;

        public App()
        {
            InitializeComponent();

            InitApp();
        }

        private void InitApp() {
            _settingsService = ViewModelLocator.Resolve<ISettingsService>();
            if (!_settingsService.UseMocks)
                ViewModelLocator.UpdateDependencies(_settingsService.UseMocks);

            //DependencyService.Register<MockDataStore>();
            //MainPage = new MainPage();
        }

        private Task InitNavigation()
        {
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            return navigationService.InitializeAsync();
        }

        protected override async void OnStart()
        {
            base.OnStart();
            await InitNavigation();

            //Will init some other service
            //if (_settingsService.AllowGpsLocation && !_settingsService.UseFakeLocation)
            //{
            //    await GetGpsLocation();
            //}
            //if (!_settingsService.UseMocks && !string.IsNullOrEmpty(_settingsService.AuthAccessToken))
            //{
            //    await SendCurrentLocation();
            //}

            base.OnResume();
        }

        
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        ///
        /// IF USE SPECIAL SERVICE WILL DEFINE HERE...
        ///
        /*
        private async Task GetGpsLocation()
        {
            var dependencyService = ViewModelLocator.Resolve<IDependencyService>();
            var locator = dependencyService.Get<ILocationServiceImplementation>();

            if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
            {
                locator.DesiredAccuracy = 50;

                try
                {
                    var position = await locator.GetPositionAsync();
                    _settingsService.Latitude = position.Latitude.ToString();
                    _settingsService.Longitude = position.Longitude.ToString();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            else
            {
                _settingsService.AllowGpsLocation = false;
            }
        }

        private async Task SendCurrentLocation()
        {
            var location = new Location
            {
                Latitude = double.Parse(_settingsService.Latitude, CultureInfo.InvariantCulture),
                Longitude = double.Parse(_settingsService.Longitude, CultureInfo.InvariantCulture)
            };

            var locationService = ViewModelLocator.Resolve<ILocationService>();
            await locationService.UpdateUserLocation(location, _settingsService.AuthAccessToken);
        }
        */
    }
}
