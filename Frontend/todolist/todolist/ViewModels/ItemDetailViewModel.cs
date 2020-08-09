using System;

using todolist.Models.Item;
using todolist.ViewModels.Base;

namespace todolist.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        public Item Item { get; set; }
        public ItemDetailViewModel(Item item = null)
        {
            Item = item;
        }
    }
}
