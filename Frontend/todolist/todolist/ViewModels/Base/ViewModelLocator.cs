using System;
using System.Globalization;
using System.Reflection;
using TinyIoC;
using todolist.Models.Item;
using todolist.Services.DataStore;
using todolist.Services.Dialog;
using todolist.Services.Navigation;
using todolist.Services.RequestProvider;
using todolist.Services.Settings;
using Xamarin.Forms;

namespace todolist.ViewModels.Base
{
    public static class ViewModelLocator
    {
        private static TinyIoCContainer _container;

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(ViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(ViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static bool UseMockService { get; set; }

        static ViewModelLocator()
        {
            _container = new TinyIoCContainer();

            // View models - by default, TinyIoC will register concrete classes as multi-instance.
            _container.Register<LoginViewModel>();
            _container.Register<RegisterViewModel>();
            _container.Register<MainViewModel>();

            // Services - by default, TinyIoC will register interface registrations as singletons.
            _container.Register<INavigationService, NavigationService>();
            _container.Register<IDialogService, DialogService>();
            _container.Register<ISettingsService, SettingsService>();
            _container.Register<IRequestProvider, RequestProvider>();
            _container.Register<IDataStore<Item>, MockDataStore>();
        }

        /// <summary>
        /// Update Dependencies service after change setting using mock service
        /// </summary>
        /// <param name="useMockServices"></param>
        public static void UpdateDependencies(bool useMockServices)
        {
            // Change injected dependencies
            if (useMockServices)
            {
                //Update service here
                _container.Register<IDataStore<Item>, MockDataStore>();
                //Change binding
                UseMockService = true;
            }
            else {
                _container.Register<IDataStore<Item>, DataStore>();
                UseMockService = false;
            }
        }

        #region Util Function
        public static void RegisterSingleton<TInterface, T>() where TInterface : class where T : class, TInterface
        {
            _container.Register<TInterface, T>().AsSingleton();
        }

        public static T Resolve<T>() where T : class
        {
            return _container.Resolve<T>();
        }

        /// <summary>
        /// Auto Link View & ViewModel, but the name of View have to end with View and name of ViewModel have to end with ViewModel
        /// This is process by name
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }
            var viewModel = _container.Resolve(viewModelType);
            view.BindingContext = viewModel;
        }
        #endregion
    }
}
