using net_speed_indicator.Models;
using net_speed_indicator.UserControls.MiniColorPickerUtils;
using net_speed_indicator.Views;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using System;
using System.Threading;
using System.ComponentModel;
using net_speed_indicator.Utilities;
using System.Net.NetworkInformation;
using System.Diagnostics;
using iTuner;

namespace net_speed_indicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// MainWindow is the Widget that shows the Upload/Download speed
    /// </summary>
    public partial class MainWindow : Window, INetworkSpeedListener
    {
        private readonly object _thisLock = new();
        private readonly Thickness evenThickness = new(4, 4, 4, 4);
        private readonly Thickness topPanelThickness = new(4, 4, 4, 2);
        private readonly Thickness bottomPanelThickness = new(4, 2, 4, 4);
        private BackgroundWorker BackgroundWorker { get; set; }
        private SpeedChecker SpeedChecker { get; set; }
        private Window SettingsWindow { get; set; }
        public bool IsSystemOnline { get; set; }
        public string NetworkInterfaceName { get; set; }
        public AppData AppData { get; set; } = AppData.Instance;
        public SimpleCommand TrayIcon_LeftClickCommand { get; set; }
        public bool IsWidgetHidden { get; set; } = false;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TrayIcon_LeftClickCommand = new SimpleCommand(() => LaunchWidgetFromTrayIcon());
        }

        private void OnAppDataUpdated(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(AppData.Opacity): ApplyOpacity(); break;
                case nameof(AppData.FontSize): ApplyFontSize(); break;
                case nameof(AppData.AlwaysOnTop): ApplyAlwaysOnTop(); break;
                case nameof(AppData.RunAtStartup): ApplyRunAtStartup(); break;
                case nameof(AppData.BackgroundColor): ApplyBackgroundColor(); break;
                case nameof(AppData.ForegroundColor): ApplyForegroundColor(); break;
                case nameof(AppData.DataSpeedToShow): ApplyDataSpeedOption(); break;
                case nameof(AppData.RememberWidgetPosition): ApplyWidgetPosition(); break;
                case nameof(AppData.NetworkInterfaceId): ApplySelectedNetworkInterface(); break;
                case nameof(AppData.AutoSelectInterface): ApplySelectedNetworkInterface(); break;
                case nameof(AppData.AppTheme): ApplyAppTheme(); break;
                default: break;
            }
        }

        private void ApplyBackgroundColor()
        {
            string BgColorName = AppData.BackgroundColor;
            Color bgColor = ColorsList.Colors.First(x => x.Name.ToLower().Equals(BgColorName.ToLower()));
            Window_Widget.Background = bgColor.Brush;
        }
        private void ApplyForegroundColor()
        {
            string FgColorName = AppData.ForegroundColor;
            Color fgColor = ColorsList.Colors.First(x => x.Name.ToLower().Equals(FgColorName.ToLower()));
            Window_Widget.Foreground = fgColor.Brush;
        }
        private void ApplyOpacity()
        {
            int Opacity = AppData.Opacity;
            Window_Widget.Opacity = Opacity / 100.0;
        }
        private void ApplyFontSize()
        {
            int FontSize = AppData.FontSize;
            Window_Widget.FontSize = FontSize;
        }
        private void ApplyDataSpeedOption()
        {
            int id = AppData.DataSpeedToShow;
            switch (id)
            {
                case 0: // Both Downloads & Uploads
                    Panel_DownloadSpeed.Visibility = Visibility.Visible;
                    Panel_UploadSpeed.Visibility = Visibility.Visible;
                    Panel_DownloadSpeed.Margin = bottomPanelThickness;
                    Panel_UploadSpeed.Margin = topPanelThickness;
                    break;
                case 1: // Uploads only
                    Panel_DownloadSpeed.Visibility = Visibility.Collapsed;
                    Panel_UploadSpeed.Visibility = Visibility.Visible;
                    Panel_UploadSpeed.Margin = evenThickness;
                    break;
                case 2: // Downloads only
                    Panel_DownloadSpeed.Visibility = Visibility.Visible;
                    Panel_UploadSpeed.Visibility = Visibility.Collapsed;
                    Panel_DownloadSpeed.Margin = evenThickness;
                    break;
                default:
                    break;
            }
        }
        private void ApplyAlwaysOnTop()
        {
            bool alwaysOnTop = AppData.AlwaysOnTop;
            Window_Widget.Topmost = alwaysOnTop;
            MenuItem_AlwaysOnTop.IsChecked = alwaysOnTop;
            TaskBar_MenuItem_AlwaysOnTop.IsChecked = alwaysOnTop;
            if (alwaysOnTop)
            {
                BackgroundWorker = new BackgroundWorker
                {
                    WorkerSupportsCancellation = true
                };
                BackgroundWorker.DoWork += BackgroundWorker_DoWork;
                BackgroundWorker.RunWorkerAsync();
            }
            else
            {
                if (BackgroundWorker != null)
                {
                    BackgroundWorker.CancelAsync();
                    BackgroundWorker = null;
                }
            }
        }
        private void ApplyWidgetPosition()
        {
            if (!AppData.RememberWidgetPosition)
            {
                return;
            }
            Window_Widget.Top = AppData.PositionTop;
            Window_Widget.Left = AppData.PositionLeft;
        }
        private void ApplySelectedNetworkInterface()
        {
            if (NetworkStatus.IsAvailable)
            {
                OnSystemOnline();
            }
            else
            {
                OnSystemOffline();
            }
        }
        private void ApplyRunAtStartup()
        {
            CommonUtils.RegisterInStartup(AppData.RunAtStartup);
        }
        private void ApplyAppTheme()
        {
            switch (AppData.AppTheme)
            {
                case 0:
                    SystemAppTheme theme = CommonUtils.GetSystemDefaultAppTheme();
                    _ = theme == SystemAppTheme.Light
                        ? ControlzEx.Theming.ThemeManager.Current.ChangeTheme(this, "Light.Blue")
                        : ControlzEx.Theming.ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                    break;
                case 1:
                    _ = ControlzEx.Theming.ThemeManager.Current.ChangeTheme(this, "Light.Blue");
                    break;
                case 2:
                    _ = ControlzEx.Theming.ThemeManager.Current.ChangeTheme(this, "Dark.Blue");
                    break;
                default: break;
            }
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(1000);
                        if (BackgroundWorker == null || BackgroundWorker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        lock (_thisLock)
                        {
                            _ = Dispatcher.Invoke(new Action(() =>
                            {
                                Window_Widget.Topmost = false;
                                Window_Widget.Topmost = true;
                            }), null);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ApplyAppDataPreferences()
        {
            ApplyBackgroundColor();
            ApplyOpacity();
            ApplyFontSize();
            ApplyForegroundColor();
            ApplyDataSpeedOption();
            ApplyAlwaysOnTop();
            ApplyWidgetPosition();
            ApplySelectedNetworkInterface();
            ApplyRunAtStartup();
            /*ApplyAppTheme();*/
        }

        void INetworkSpeedListener.OnSpeedCheck(NetworkSpeed networkSpeed)
        {
            bool useBits = AppData.SpeedUnit.Equals(SpeedUnit.BitsPerSecond);
            string UploadText = useBits ? networkSpeed.GetBitsSent() : networkSpeed.GetBytesSent();
            string DownloadText = useBits ? networkSpeed.GetBitsReceived() : networkSpeed.GetBytesReceived();
            TextBlock_UploadSpeed.Text = UploadText;
            TextBlock_DownloadSpeed.Text = DownloadText;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }

        private void MenuItem_Settings_Click(object sender, RoutedEventArgs e)
        {
            if (SettingsWindow == null)
            {
                SettingsWindow = new Settings();
                SettingsWindow.Show();
                SettingsWindow.Closed += SettingsWindow_Closed;
            }
            else
            {
                _ = SettingsWindow.Activate();
            }
        }

        private void SettingsWindow_Closed(object sender, System.EventArgs e)
        {
            SettingsWindow = null;
        }
        private void LaunchWidgetFromTrayIcon()
        {
            Show();
            IsWidgetHidden = false;
            TaskBar_MenuItem_ShowHideWidget.Header = "Hide Indicator";
        }
        private void MenuItem_Minimize_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            IsWidgetHidden = true;
            TaskBar_MenuItem_ShowHideWidget.Header = "Show Indicator";
        }
        private void TaskBar_MenuItem_ShowHideWidget_Click(object sender, RoutedEventArgs e)
        {
            if (IsWidgetHidden)
            {
                LaunchWidgetFromTrayIcon();
            }
            else
            {
                MenuItem_Minimize_Click(sender, e);
            }
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_AlwaysOnTop_Click(object sender, RoutedEventArgs e)
        {
            AppData.AlwaysOnTop = !AppData.AlwaysOnTop;
        }
        private void MenuItem_ResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Reset settings to default?", "Confirm Reset", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                AppData.Instance.ResetAppDataToDefault();
            }
        }
        private void Window_Widget_Loaded(object sender, RoutedEventArgs e)
        {
            SpeedChecker = new SpeedChecker();
            SpeedChecker.SetListener(this);
            SpeedChecker.Start();
            ApplyAppDataPreferences();
            AppData.PropertyChanged += OnAppDataUpdated;
            NetworkStatus.AvailabilityChanged += OnNetworkAvailabilityChanged;
            LocationChanged += Widget_LocationChanged;
            IsSystemOnline = NetworkStatus.IsAvailable;
            if (IsSystemOnline)
            {
                OnSystemOnline();
            }
            else
            {
                OnSystemOffline();
            }
        }

        private void Widget_LocationChanged(object sender, EventArgs e)
        {
            if (Window_Widget.WindowState == WindowState.Normal)
            {
                AppData.PositionTop = (int)Window_Widget.Top;
                AppData.PositionLeft = (int)Window_Widget.Left;
            }
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkStatusChangedArgs e)
        {
            IsSystemOnline = NetworkStatus.IsAvailable;
            if (IsSystemOnline)
            {
                Trace.WriteLine("System is online");
                OnSystemOnline();
            }
            else
            {
                Trace.WriteLine("System is offline");
                OnSystemOffline();
            }
        }
        private void OnSystemOnline()
        {
            if (SpeedChecker == null)
            {
                SpeedChecker = new SpeedChecker();
            }            
            NetworkInterface active = CommonUtils.GetActiveNetworkInterface();
            NetworkInterface selected = CommonUtils.GetInterfaceForId(AppData.NetworkInterfaceId);
            NetworkInterface nInterface = ((!AppData.AutoSelectInterface) && (selected != null)) ? selected : active;
            AppData.NetworkInterfaceId = nInterface.Id;
            SpeedChecker.NetworkInterface = nInterface;
            SpeedChecker.PreviousBytesReceived = nInterface.GetIPv4Statistics().BytesReceived;
            SpeedChecker.PreviousBytesSent = nInterface.GetIPv4Statistics().BytesSent;
            SpeedChecker.Start();
            NetworkInterfaceName = nInterface.Name;

            _ = Dispatcher.Invoke(new Action(() =>
            {
                Panel_DownloadSpeed.Visibility = Visibility.Visible;
                Panel_UploadSpeed.Visibility = Visibility.Visible;
                Panel_Offline.Visibility = Visibility.Collapsed;
            }), null);
        }
        private void OnSystemOffline()
        {
            SpeedChecker.Stop();
            NetworkInterfaceName = "No network access";

            _ = Dispatcher.Invoke(new Action(() =>
            {
                Panel_Offline.Visibility = Visibility.Visible;
                Panel_DownloadSpeed.Visibility = Visibility.Collapsed;
                Panel_UploadSpeed.Visibility = Visibility.Collapsed;
            }), null);
        }
        private void MenuItem_ChangeNetworkInterface_Click(object sender, RoutedEventArgs e)
        {
            MenuItem_Settings_Click(sender, e);
            ((SettingsViewModel)SettingsWindow.DataContext).ActiveTab = SettingsTab.NetworkInterface;
        }
    }
}
