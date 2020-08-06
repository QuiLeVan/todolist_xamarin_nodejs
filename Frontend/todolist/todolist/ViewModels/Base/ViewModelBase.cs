using System;
using System.Threading.Tasks;
using todolist.Services.Dialog;
using todolist.Services.Navigation;
using todolist.Services.Settings;

namespace todolist.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public ViewModelBase()
        {
            DialogService = ViewModelLocator.Resolve<IDialogService>();
            NavigationService = ViewModelLocator.Resolve<INavigationService>();

            var settingsService = ViewModelLocator.Resolve<ISettingsService>();

            GlobalSetting.Instance.EndpointBase = settingsService.EndpointBase;

        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
