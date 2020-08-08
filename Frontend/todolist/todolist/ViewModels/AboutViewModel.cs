using System;
using System.Windows.Input;
using todolist.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace todolist.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://blog.quilv.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}