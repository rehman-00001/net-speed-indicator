using MahApps.Metro.Controls;
using net_speed_indicator.Models;
using System.ComponentModel;
using System.Net.NetworkInformation;
using net_speed_indicator.Utilities;

namespace net_speed_indicator.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        private SettingsViewModel Context => DataContext as SettingsViewModel;

        public Settings()
        {
            InitializeComponent();
            InitializeContext();
            InitializeView();
        }

        private void InitializeContext()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface activeInterface = CommonUtils.GetActiveNetworkInterface();
            DataSpeedOption[] dataSpeedOptions = Constants.DataSpeedOptions;
            DataContext = new SettingsViewModel(interfaces, AppData.Instance, Constants.InitialTab, dataSpeedOptions);
            Context.PropertyChanged += DataPropertyChanged;
        }
        private void InitializeView()
        {
            SetActiveTabInView();
        }
        private void DataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "ActiveTab":
                    SetActiveTabInView();
                    return;
                default:
                    break;
            }
        }
        private void SetActiveTabInView()
        {
            MainView.Children.Clear();
            switch (Context.ActiveTab)
            {
                case SettingsTab.General:
                    {
                        _ = MainView.Children.Add(vGeneralTab);
                        break;
                    }
                case SettingsTab.Appearance:
                    {
                        _ = MainView.Children.Add(vAppearanceTab);
                        break;
                    }
                case SettingsTab.NetworkInterface:
                    {
                        _ = MainView.Children.Add(vNetworkInterfaceTab);
                        break;
                    }
                case SettingsTab.Theme:
                    {
                        _ = MainView.Children.Add(vThemeTab);
                        break;
                    }
                case SettingsTab.About:
                    {
                        _ = MainView.Children.Add(vAboutTab);
                        break;
                    }
                default:
                    break;
            };

        }
    }
}
