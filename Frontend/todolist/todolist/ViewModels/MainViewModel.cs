using System;
using System.Threading.Tasks;
using System.Windows.Input;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR ICommand Function
        ///------------------------------------------------------------------------
        #region ICommand Function
        public ICommand SettingsCommand => new Command(async () => await SettingAsync());

        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR ICommand Function
        ///------------------------------------------------------------------------
        public MainViewModel()
        {
        }


        private async Task SettingAsync()
        {
            await NavigationService.NavigateToAsync<SettingsViewModel>();
        }
    }
}
