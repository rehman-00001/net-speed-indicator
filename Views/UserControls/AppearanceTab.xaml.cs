using net_speed_indicator.Models;
using net_speed_indicator.UserControls.MiniColorPickerUtils;
using Serilog;
using System;
using System.Windows.Controls;
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
            Log.Information("{0}:: AppearanceTab() - Instance Created", GetType().Name);
        }
        private void BackgroundColorPicker_OnColorSelected(object sender, EventArgs e)
        {
            ColorArgs args = (ColorArgs)e;
            Color SelectedColor = args.Color;
            Context.AppData.BackgroundColor = SelectedColor.Name;
            Log.Information("{0}::BackgroundColorPicker_OnColorSelected - Selected BG color: {1}", GetType().Name, SelectedColor.Name);
        }

        private void ForegroundColorPicker_OnColorSelected(object sender, EventArgs e)
        {
            ColorArgs args = (ColorArgs)e;
            Color SelectedColor = args.Color;
            Context.AppData.ForegroundColor = SelectedColor.Name;
            Log.Information("{0}::ForegroundColorPicker_OnColorSelected - Selected FG color: {1}", GetType().Name, SelectedColor.Name);
        }
    }
}
