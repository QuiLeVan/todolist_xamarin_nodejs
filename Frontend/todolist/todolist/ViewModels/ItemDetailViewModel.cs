using System;
using System.Diagnostics;
using System.Threading.Tasks;
using todolist.Models.Item;
using todolist.ViewModels.Base;

namespace todolist.ViewModels
{
    public class ItemDetailViewModel : ViewModelBase
    {
        private Item _item;

        public Item Item {
            get => _item;
            set {
                _item = value;
                RaisePropertyChanged(() => Item);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Item)
            {
                //Should be use api to get content from server
                Item = (Item)navigationData;
            }

            return base.InitializeAsync(navigationData);
        }

        public ItemDetailViewModel()
        {
            Debug.Write("GO HERE");
        }
    }
}
