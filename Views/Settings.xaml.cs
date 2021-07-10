using net_speed_indicator.Models;
using System.ComponentModel;
using System.Net.NetworkInformation;
using net_speed_indicator.Utilities;
using ControlzEx.Theming;
using System.Windows;
using MahApps.Metro.Controls;
using iTuner;
using Serilog;

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
            Log.Information("{0}::Settings() - Instance created", GetType().Name);
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Log.Information("{0}::MetroWindow_Loaded()", GetType().Name);
            InitializeContext();
            InitializeView();
            AppData.Instance.PropertyChanged += AppData_PropertyChanged;
        }
        private void InitializeContext()
        {
            Log.Information("{0}::InitializeContext()", GetType().Name);
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            DataSpeedOption[] dataSpeedOptions = Constants.DataSpeedOptions;
            DataContext = new SettingsViewModel(interfaces, AppData.Instance, Constants.InitialTab, dataSpeedOptions);
            Context.PropertyChanged += DataPropertyChanged;
        }

        private void AppData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Log.Information("{0}::AppData_PropertyChanged() - PropertyName: ", GetType().Name, e.PropertyName);
            switch (e.PropertyName)
            {
                case nameof(AppData.AppTheme):
                    ApplyAppTheme();
                    return;
                case nameof(AppData.AutoSelectInterface):
                    ApplySelectedNetworkInterface();
                    return;
                default:
                    break;
            }
        }
        private void InitializeView()
        {
            Log.Information("{0}::InitializeView()", GetType().Name);
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
            Log.Information("{0}::SetActiveTabInView()", GetType().Name);
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
            Log.Information("{0}::ApplyAppTheme() - Theme: ", GetType().Name, Context.AppData.AppTheme);
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
                    _ = ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                    break;
                default: break;
            }
        }
        private void ApplySelectedNetworkInterface()
        {
            Log.Information("{0}::ApplySelectedNetworkInterface()", GetType().Name);
            bool autoSelect = AppData.Instance.AutoSelectInterface;
            bool isAvailable = NetworkStatus.IsAvailable;
            Log.Information("{0}:: AutoSelectInterface: {1} && NetworkStatus.IsAvailable: {2}", GetType().Name, autoSelect, isAvailable);
            if (AppData.Instance.AutoSelectInterface && NetworkStatus.IsAvailable)
            {
                NetworkInterface @interface = CommonUtils.GetActiveNetworkInterface();
                string id = @interface?.Id;
                string name = @interface?.Name;
                AppData.Instance.NetworkInterfaceId = id;
                Log.Information("{0}:: Active interface id: {1}", GetType().Name, name);
            }
        }
    }
}
