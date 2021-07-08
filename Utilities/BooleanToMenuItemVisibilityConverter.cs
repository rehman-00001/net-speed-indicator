using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace net_speed_indicator.Utilities
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class BooleanToMenuItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return original ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility original = (Visibility)value;
            return original == Visibility.Visible;
        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    class InvertedBooleanToMenuItemVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool original = (bool)value;
            return !original ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility original = (Visibility)value;
            return original != Visibility.Visible;
        }
    }
}
