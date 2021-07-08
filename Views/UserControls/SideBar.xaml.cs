using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using net_speed_indicator.Models;
using System.ComponentModel;
using net_speed_indicator.Utilities;

namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SideBar.xaml
    /// </summary>
    public partial class SideBar : UserControl
    {

        private SettingsViewModel Context
        { get => DataContext as SettingsViewModel; }

        public SideBar()
        {
            InitializeComponent();
            ToggleBtn_General.IsChecked = true;
            DataContextChanged += OnDataContextSet;
        }

        private void OnDataContextSet(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Context != null)
            {
                Context.PropertyChanged += DataPropertyChanged;
                DataContextChanged -= OnDataContextSet;
            }
        }

        private void DataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (DataContext != null)
            {
                SettingsViewModel model = DataContext as SettingsViewModel;
                ToggleBtn_General.IsChecked = model.ActiveTab == SettingsTab.General;
                ToggleBtn_Appearance.IsChecked = model.ActiveTab == SettingsTab.Appearance;
                ToggleBtn_NetworkInterface.IsChecked = model.ActiveTab == SettingsTab.NetworkInterface;
                ToggleBtn_Theme.IsChecked = model.ActiveTab == SettingsTab.Theme;
                ToggleBtn_About.IsChecked = model.ActiveTab == SettingsTab.About;
            }
        }

        private void ToggleBtn_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = (ToggleButton)sender;
            if (Context != null)
            {
                Context.ActiveTab = CommonUtils.ToggleBtnToEnum(btn.Name);
            }
        }
    }
}
