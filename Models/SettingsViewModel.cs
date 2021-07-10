#pragma warning disable IDE0051 // Remove unused private members
using System.Net.NetworkInformation;
using net_speed_indicator.Utilities;
using Serilog;

namespace net_speed_indicator.Models
{
    public class SettingsViewModel : ViewModelBase
    {
        public AppData AppData { get; set; }
        private SettingsTab _activeTab;       
        
        public DataSpeedOption[] DataSpeedOptions { get; set; }
        public NetworkInterface[] NetworkInterfaces { get; set; }
        public SettingsTab ActiveTab
        {
            get => _activeTab;
            set => SetProperty(ref _activeTab, value);
        }

        public SettingsViewModel(NetworkInterface[] networkInterfaces, AppData appData, SettingsTab activeTab, DataSpeedOption[] dataSpeedOptions)
        {
            AppData = appData;
            NetworkInterfaces = networkInterfaces;
            _activeTab = activeTab;
            DataSpeedOptions = dataSpeedOptions;
            Log.Information("{0}:: SettingsViewModel - Instance created", GetType().Name);
        }
    }
}
#pragma warning restore IDE0051 // Remove unused private members