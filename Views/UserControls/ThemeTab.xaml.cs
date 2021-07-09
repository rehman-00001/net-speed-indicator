using ControlzEx.Theming;
using net_speed_indicator.Models;
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
        }

        private void RadioButton_LightTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 1;
        }

        private void RadioButton_DarkTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 2;
        }

        private void RadioButton_SystemTheme_Checked(object sender, RoutedEventArgs e)
        {
            AppData.Instance.AppTheme = 0;
        }
    }
}
