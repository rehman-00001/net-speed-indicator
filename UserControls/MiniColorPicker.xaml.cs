using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using net_speed_indicator.UserControls.MiniColorPickerUtils;

namespace net_speed_indicator.UserControls
{
    /// <summary>
    /// Interaction logic for MiniColorPicker.xaml
    /// </summary>
    public partial class MiniColorPicker : UserControl
    {
        public event EventHandler OnColorSelected;
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(
                nameof(SelectedColor),
                typeof(MiniColorPickerUtils.Color),
                typeof(MiniColorPicker),
                new FrameworkPropertyMetadata(
                     null,
                     FrameworkPropertyMetadataOptions.BindsTwoWayByDefault
                )
            );
        public MiniColorPickerUtils.Color SelectedColor
        {
            get => (MiniColorPickerUtils.Color)GetValue(SelectedColorProperty);
            set => SetCurrentValue(SelectedColorProperty, value);
        }
        public MiniColorPickerUtils.Color[] Colors { get; set; } = ColorsList.Colors;
        public MiniColorPicker()
        {
            InitializeComponent();
        }
        public void ListView_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null)
            {
                MiniColorPickerUtils.Color color = (MiniColorPickerUtils.Color)((ListBoxItem)sender).Content;
                SelectedColor = color;
                OnColorSelected(this, new ColorArgs(color));
            }
        }
    }



    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2)
                return false;
            return values[0].Equals(values[1]);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
