using Microsoft.Win32;
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

        public static SettingsTab ToggleBtnToEnum(string buttonName)
        {
            return buttonName switch
            {
                "ToggleBtn_General" => SettingsTab.General,
                "ToggleBtn_Appearance" => SettingsTab.Appearance,
                "ToggleBtn_NetworkInterface" => SettingsTab.NetworkInterface,
                "ToggleBtn_Theme" => SettingsTab.Theme,
                "ToggleBtn_About" => SettingsTab.About,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        public static NetworkInterface GetActiveNetworkInterface()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface activeAdapter = interfaces.FirstOrDefault(x => x.NetworkInterfaceType != NetworkInterfaceType.Loopback
                    && x.NetworkInterfaceType != NetworkInterfaceType.Tunnel
                    && x.OperationalStatus == OperationalStatus.Up
                    && x.Name.StartsWith("vEthernet") == false);
            return activeAdapter;
        }

        public static NetworkInterface GetInterfaceForId(string id)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            return interfaces.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static void RegisterInStartup(bool isEnabled)
        {
            RegistryKey registryKey = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isEnabled)
            {
                if (registryKey.GetValue(Constants.AppName) == null)
                {
                    registryKey.SetValue(Constants.AppName, GetExecutablePath());
                }
            }
            else
            {
                if (registryKey.GetValue(Constants.AppName) != null)
                {
                    registryKey.DeleteValue(Constants.AppName);
                }
            }
        }

        public static string GetExecutablePath()
        {
            return Process.GetCurrentProcess().MainModule.FileName;
        }

        public static SystemAppTheme GetSystemDefaultAppTheme()
        {
            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize", true);
                int value = (int)registryKey.GetValue("AppsUseLightTheme");
                return (SystemAppTheme)value;
            }
            catch
            {
                return SystemAppTheme.Light;
            }
        }
    }
}
