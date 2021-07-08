using net_speed_indicator.UserControls.MiniColorPickerUtils;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Linq;

namespace net_speed_indicator.Utilities
{
    [ValueConversion(typeof(string), typeof(Color))]
    public class ColorNameToObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string colorName = (string)value;
            Color color =  ColorsList.Colors.FirstOrDefault(
                x => x.Name.ToLower().Equals(colorName.ToLower())
            );
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = (Color)value;
            return color.Name;
        }
    }
}
