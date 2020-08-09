using System;
using System.Threading.Tasks;
using System.Windows.Input;
using todolist.Models.Item;
using todolist.Services.DataStore;
using todolist.ViewModels.Base;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class NewItemViewModel : ViewModelBase
    {
        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------
        #region VARIABLE

        private Item _item;
        public Item Item
        {
            get => _item;
            set
            {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }

        private IDataStore<Item> _dataStore;
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------


        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR ICommand
        ///------------------------------------------------------------------------
        #region ICommand
        public ICommand AddItemCommand => new Command(async () => await AddItemAsync());
        public ICommand CancelCommand => new Command(async () => await CancelItemAsync());

        
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR ICommand
        ///------------------------------------------------------------------------


        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        #region LOGIC FUNC

        private async Task CancelItemAsync()
        {
            await NavigationService.GoBackAsync();
            
        }

        private async Task AddItemAsync()
        {
            if (_item.Text != null && _item.Text != ""
                && _item.Description != null & _item.Description != ""
                )
            {
                MessagingCenter.Send(this, "AddItem", Item);
                await _dataStore.AddItemAsync(_item);
                await NavigationService.GoBackAsync();
            }
            else
            {
                await DialogService.ShowAlertAsync("Input Title & Description", "Alert", "OK");
            }
        }

        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        public override Task InitializeAsync(object navigationData) {

            return base.InitializeAsync(navigationData);
        }


        public NewItemViewModel(IDataStore<Item> dataStore)
        {
            _dataStore = dataStore;

            Item = new Item();
        }


    }
}
