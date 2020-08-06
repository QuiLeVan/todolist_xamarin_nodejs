using System;
using System.Collections.Generic;
using todolist.ViewModels;
using Xamarin.Forms;

namespace todolist.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() {

            var content = this.Content;
            this.Content = null;
            this.Content = content;

            var vm = BindingContext as LoginViewModel;

            if (vm != null) {
                vm.InvalidateMock();

                // Will add animation later
                //if (!vm.IsMock)
                //{
                    //_animate = true;
                    //await AnimateIn();
                //}
            }
        }
    }
}
