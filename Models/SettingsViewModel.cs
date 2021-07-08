#pragma warning disable IDE0051 // Remove unused private members
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using net_speed_indicator.Utilities;

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
        }

        public SettingsViewModel()
        {
            AppData = AppData.Instance;
        }
    }
}
#pragma warning restore IDE0051 // Remove unused private members