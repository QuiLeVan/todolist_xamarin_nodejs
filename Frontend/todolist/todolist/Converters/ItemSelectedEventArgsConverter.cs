using System;
using System.Globalization;
using Xamarin.Forms;

namespace todolist.Converters
{
    public class ItemSelectedEventArgsConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as SelectionChangedEventArgs;
            if (eventArgs == null)
                throw new ArgumentException("Expected TappedEventArgs as value", "value");

            return eventArgs.CurrentSelection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
