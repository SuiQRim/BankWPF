using System;
using System.Windows.Data;
using System.Windows;

namespace DataSource.Converters
{
    public class BooleanReverseConverter : IValueConverter
    {
        public object Convert(object item, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var visibility = (Visibility)item;

            if (visibility == Visibility.Collapsed || visibility == Visibility.Hidden)
            {
                return true;
            }
            return false;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new NotSupportedException();
        }
    }
}
