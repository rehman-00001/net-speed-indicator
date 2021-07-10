using Microsoft.Win32;
using Serilog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;


namespace net_speed_indicator.Utilities
{
    internal class CommonUtils
    {
        private static readonly string[] BytesSizeSuffixes =
                   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private static readonly string[] BitsSizeSuffixes =
           { "bits", "kbits", "Mbits", "Gbits", "Tbits", "Pbits", "Ebits", "Zbits", "Ybits" };
        public static string BytesSizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + BytesSizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bytes", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                BytesSizeSuffixes[mag]);
        }
        public static string BitsSizeSuffix(long value, int decimalPlaces = 1)
        {
            if (decimalPlaces < 0) { throw new ArgumentOutOfRangeException("decimalPlaces"); }
            if (value < 0) { return "-" + BitsSizeSuffix(-value, decimalPlaces); }
            if (value == 0) { return string.Format("{0:n" + decimalPlaces + "} bits", 0); }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                BitsSizeSuffixes[mag]);
        }

        public static string TabToToggleButtonName(SettingsTab tab) => tab switch
        {
            SettingsTab.General => "ToggleBtn_General",
            SettingsTab.Appearance => "ToggleBtn_Appearance",
            SettingsTab.NetworkInterface => "ToggleBtn_NetworkInterface",
            SettingsTab.Theme => "ToggleBtn_Theme",
            SettingsTab.About => "ToggleBtn_About",
            _ => throw new ArgumentOutOfRangeException(),
        };

        public static SettingsTab ToggleBtnToEnum(string buttonName) => buttonName switch
        {
            "ToggleBtn_General" => SettingsTab.General,
            "ToggleBtn_Appearance" => SettingsTab.Appearance,
            "ToggleBtn_NetworkInterface" => SettingsTab.NetworkInterface,
            "ToggleBtn_Theme" => SettingsTab.Theme,
            "ToggleBtn_About" => SettingsTab.About,
            _ => throw new ArgumentOutOfRangeException(),
        };

        public static NetworkInterface GetActiveNetworkInterface()
        {
            Log.Information("CommonUtils::GetActiveNetworkInterface():");
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            Log.Information("Total available network interfaces count: {0}", interfaces.Length);
            NetworkInterface activeAdapter = interfaces.FirstOrDefault(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && x.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && x.OperationalStatus == OperationalStatus.Up
                    && x.Name.StartsWith("vEthernet") == false);
            Log.Information("Active Network Interface - Id: {0}, Name: {1}, Type: {2}", activeAdapter.Id, activeAdapter.Name, activeAdapter.NetworkInterfaceType);
            return activeAdapter;
        }

        public static NetworkInterface GetInterfaceForId(string id)
        {
            Log.Information("CommonUtils::GetInterfaceForId(): id: {0}", id);
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface @interface = interfaces.FirstOrDefault(x => x.Id.Equals(id));
            Log.Information("Total available network interfaces count: {0}", interfaces.Length);
            Log.Information("Network Interface - Id: {0}, Name: {1}, Type: {2}", @interface.Id, @interface.Name, @interface.NetworkInterfaceType);
            return @interface;
        }

        public static void RegisterInStartup(bool isEnabled)
        {
            Log.Information("CommonUtils::RegisterInStartup(): isEnabled: {0}", isEnabled);
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (isEnabled)
                {
                    object value = registryKey.GetValue(Constants.AppName);
                    Log.Information("Value: \"{0}\" in Registry: {1}", Constants.AppName, value);
                    if (value == null)
                    {
                        registryKey.SetValue(Constants.AppName, GetExecutablePath());
                        Log.Information("SUCCESS: Registered to run in startup");
                    }
                }
                else
                {
                    object value = registryKey.GetValue(Constants.AppName);
                    Log.Information("Value: \"{0}\" in Registry: {1}", Constants.AppName, value);
                    if (value != null)
                    {
                        registryKey.DeleteValue(Constants.AppName);
                        Log
                           .Information("SUCCESS: Run at startup - Registry entry removed");
                    }
                }
            }
            catch (Exception e)
            {
                Log
                   .Error("Unable to make changes in registry:\n{0}\n{1}", e.Message, e.StackTrace);
            }
        }

        public static string GetExecutablePath()
        {
            string name = Process.GetCurrentProcess().MainModule.FileName;
            Log.Error("CommonUtils::GetExecutablePath() -- path: {0}", name);
            return name;
        }

        public static SystemAppTheme GetSystemDefaultAppTheme()
        {
            Log.Information("CommonUtils::GetSystemDefaultAppTheme()");
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", true);
                int value = (int)registryKey.GetValue("AppsUseLightTheme");
                Log.Information("Value for AppsUseLightTheme: {0}", value);
                return (SystemAppTheme)value;
            }
            catch (Exception e)
            {
                Log
                   .Error("Unable to read app default theme from registry:\n{0}\n{1}", e.Message, e.StackTrace);
                return SystemAppTheme.Light;
            }
        }
    }
}
