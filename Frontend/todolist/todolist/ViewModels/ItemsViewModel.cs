using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using todolist.Models.Item;
using todolist.ViewModels.Base;
using System.Windows.Input;
using todolist.Services.Settings;
using todolist.Services.DataStore;

namespace todolist.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------
        #region VARIABLE


        private ISettingsService _settingsService;
        private IDataStore<Item> _dataStore;
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR VARIABLE
        ///------------------------------------------------------------------------
        public ObservableCollection<Item> Items { get; set; }


        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR ICommand
        ///------------------------------------------------------------------------
        #region ICommand
        public ICommand LoadItemsCommand => new Command(async () => await ExecuteLoadItemsCommand());
        public ICommand AddTodoCommand => new Command(async () => await AddTodoAsync());

        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR ICommand
        ///------------------------------------------------------------------------

        public ItemsViewModel(ISettingsService settingsService, IDataStore<Item> dataStore)
        {
            _settingsService = settingsService;
            _dataStore = dataStore;
            Items = new ObservableCollection<Item>();

            InitItems();
        }

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        #region LOGIC FUNC

        private void InitItems()
        {
            _ = ExecuteLoadItemsCommand();
            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }


        private async Task AddTodoAsync()
        {
            await Task.Delay(10);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            await Task.Delay(10);
            try
            {
                Items.Clear();
                var items = await _dataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
        ///------------------------------------------------------------------------
        /// [END] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        
    }
}