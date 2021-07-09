using net_speed_indicator.Models;
using System.ComponentModel;
using System.Net.NetworkInformation;
using net_speed_indicator.Utilities;
using ControlzEx.Theming;
using System.Windows;
using MahApps.Metro.Controls;

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
        }
        private void MetroWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            InitializeContext();
            InitializeView();
            AppData.Instance.PropertyChanged += AppData_PropertyChanged;
        }
        private void InitializeContext()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface activeInterface = CommonUtils.GetActiveNetworkInterface();
            DataSpeedOption[] dataSpeedOptions = Constants.DataSpeedOptions;
            DataContext = new SettingsViewModel(interfaces, AppData.Instance, Constants.InitialTab, dataSpeedOptions);
            Context.PropertyChanged += DataPropertyChanged;
        }

        private void AppData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppData.AppTheme):
                    ApplyAppTheme();
                    return;
                default:
                    break;
            }
        }
        private void InitializeView()
        {
            SetActiveTabInView();
            ApplyAppTheme();
        }
        private void DataPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Context.ActiveTab):
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

        private void ApplyAppTheme()
        {
            switch (Context.AppData.AppTheme)
            {
                case 0:
                    SystemAppTheme theme = CommonUtils.GetSystemDefaultAppTheme();
                    _ = theme == SystemAppTheme.Light
                        ? ThemeManager.Current.ChangeTheme(this, "Light.Blue")
                        : ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                    break;
                case 1:
                    _ = ThemeManager.Current.ChangeTheme(this, "Light.Blue");
                    break;
                case 2:
                    ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                    break;
                default: break;
            }
        }
    }
}
