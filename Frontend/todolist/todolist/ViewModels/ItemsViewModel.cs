using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using todolist.Models;
using todolist.Views;
using todolist.ViewModels.Base;
using System.Windows.Input;

namespace todolist.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
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

        public ItemsViewModel()
        {
            //Title = "Browse";
            //Items = new ObservableCollection<Item>();
            //LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            //{
            //    var newItem = item as Item;
            //    Items.Add(newItem);
            //    await DataStore.AddItemAsync(newItem);
            //});
        }

        ///------------------------------------------------------------------------
        /// [START] DEFINE FOR LOGIC FUNC
        ///------------------------------------------------------------------------
        #region LOGIC FUNC


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
                //var items = await DataStore.GetItemsAsync(true);
                //foreach (var item in items)
                //{
                //    Items.Add(item);
                //}
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