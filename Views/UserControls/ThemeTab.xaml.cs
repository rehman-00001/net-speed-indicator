using net_speed_indicator.Models;
using Serilog;
using System.Windows;
using System.Windows.Controls;

namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ThemeTab.xaml
    /// </summary>
    public partial class ThemeTab : UserControl
    {
        public ThemeTab()
        {
            InitializeComponent();
            Log.Information("{0}::ThemeTab - Instance created", GetType().Name);
        }
        private void RadioButton_LightTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 1;
            Log.Information("{0}::RadioButton_LightTheme_Checked ", GetType().Name);
        }
        private void RadioButton_DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 2;
            Log.Information("{0}::RadioButton_DarkTheme_Checked ", GetType().Name);
        }
        private void RadioButton_SystemTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 0;
            Log.Information("{0}::RadioButton_SystemTheme_Checked ", GetType().Name);
        }
    }
}
