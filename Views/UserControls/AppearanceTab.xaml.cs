using net_speed_indicator.Models;
using net_speed_indicator.UserControls.MiniColorPickerUtils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Color = net_speed_indicator.UserControls.MiniColorPickerUtils.Color;

namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for AppearanceTab.xaml
    /// </summary>
    public partial class AppearanceTab : UserControl
    {
        private SettingsViewModel Context => DataContext as SettingsViewModel;

        public AppearanceTab()
        {
            InitializeComponent();
        }
        private void BackgroundColorPicker_OnColorSelected(object sender, EventArgs e)
        {
            ColorArgs args = (ColorArgs)e;
            Color SelectedColor = args.Color;
            Context.AppData.BackgroundColor = SelectedColor.Name;
        }

        private void ForegroundColorPicker_OnColorSelected(object sender, EventArgs e)
        {
            ColorArgs args = (ColorArgs)e;
            Color SelectedColor = args.Color;
            Context.AppData.ForegroundColor = SelectedColor.Name;
        }
    }
}
