﻿using System;
using System.Runtime.CompilerServices;
using net_speed_indicator.Utilities;

namespace net_speed_indicator.Models
{
    public sealed class AppData : ViewModelBase
    {
        public static bool DEFAULT_ALWAYS_ON_TOP = true;
        public static bool DEFAULT_RUN_AT_START_UP = true;
        public static bool DEFAULT_AUTO_SELECT_INTERFACE = true;
        public static bool DEFAULT_REMEMBER_WIDGET_POSITION = true;
        public static SpeedUnit DEFAULT_SPEED_UNIT = SpeedUnit.BytesPerSecond;
        public static int DEFAULT_POSITION_TOP = 48;
        public static int DEFAULT_POSITION_LEFT = 48;
        public static string DEFAULT_BG_COLOR = "White";
        public static string DEFAULT_FG_COLOR = "Black";
        public static int DEFAULT_OPACITY = 70;
        public static int DEFAULT_FONT_SIZE = 14;
        public static int DEFAULT_DATA_SPEED_OPTION = 0;

        private bool _alwaysOnTop;
        private bool _runAtStartup;
        private bool _autoSelectInterface;
        private bool _rememberWidgetPosition;
        private string _networkInterfaceId;
        private SpeedUnit _speedUnit;
        private int _positionTop, _positionLeft;

        // Appearance settings
        private string _bgColor;
        private string _fgColor;
        private int _opacity;
        private int _fontSize;
        private int _dataSpeedToShow;

        public bool AlwaysOnTop
        {
            get => _alwaysOnTop;
            set => SetProperty(ref _alwaysOnTop, value);
        }
        public bool RunAtStartup
        {
            get => _runAtStartup;
            set => SetProperty(ref _runAtStartup, value);
        }
        public bool AutoSelectInterface
        {
            get => _autoSelectInterface;
            set => SetProperty(ref _autoSelectInterface, value);
        }
        public bool RememberWidgetPosition
        {
            get => _rememberWidgetPosition;
            set => SetProperty(ref _rememberWidgetPosition, value);
        }
        public string NetworkInterfaceId
        {
            get => _networkInterfaceId;
            set => SetProperty(ref _networkInterfaceId, value);
        }
        public SpeedUnit SpeedUnit
        {
            get => _speedUnit;
            set => SetProperty(ref _speedUnit, value);
        }
        public int PositionTop
        {
            get => _positionTop;
            set => SetProperty(ref _positionTop, value);
        }
        public int PositionLeft
        {
            get => _positionLeft;
            set => SetProperty(ref _positionLeft, value);
        }
        public string BackgroundColor
        {
            get => _bgColor;
            set => SetProperty(ref _bgColor, value);
        }
        public string ForegroundColor
        {
            get => _fgColor;
            set => SetProperty(ref _fgColor, value);
        }
        public int Opacity
        {
            get => _opacity;
            set => SetProperty(ref _opacity, value);
        }
        public int FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }
        public int DataSpeedToShow
        {
            get => _dataSpeedToShow;
            set => SetProperty(ref _dataSpeedToShow, value);
        }
        private AppData()
        {
            AlwaysOnTop = Properties.Settings.Default.AlwaysOnTop;
            RunAtStartup = Properties.Settings.Default.RunAtStartup;
            AutoSelectInterface = Properties.Settings.Default.AutoSelectInterface;
            NetworkInterfaceId = Properties.Settings.Default.NetworkInterfaceId;
            SpeedUnit = (SpeedUnit)Properties.Settings.Default.SpeedUnit;
            PositionTop = Properties.Settings.Default.PositionTop;
            PositionLeft = Properties.Settings.Default.PositionLeft;
            RememberWidgetPosition = Properties.Settings.Default.RememberWidgetPosition;

            //Appearance props
            BackgroundColor = Properties.Settings.Default.BackgroundColor;
            ForegroundColor = Properties.Settings.Default.ForegroundColor;
            FontSize = Properties.Settings.Default.FontSize;
            Opacity = Properties.Settings.Default.Opacity;
            DataSpeedToShow = Properties.Settings.Default.DataSpeedToShow;
        }
        public static AppData Instance => Nested.instance;

        private class Nested
        {
            static Nested() { }
            internal static readonly AppData instance = new AppData();
        }


        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = "")
        {
            bool result = base.SetProperty(ref storage, value, propertyName);
            if (typeof(T) == SpeedUnit.GetType())
            {
                SpeedUnit v = (SpeedUnit)Convert.ChangeType(value, SpeedUnit.GetType());
                Properties.Settings.Default[propertyName] = (int)v;
                Properties.Settings.Default.Save();
                return result;
            }

            Properties.Settings.Default[propertyName] = value;
            Properties.Settings.Default.Save();
            return result;
        }

        public void ResetAppDataToDefault()
        {
            AlwaysOnTop = DEFAULT_ALWAYS_ON_TOP;
            RunAtStartup = DEFAULT_RUN_AT_START_UP;
            AutoSelectInterface = DEFAULT_AUTO_SELECT_INTERFACE;
            RememberWidgetPosition = DEFAULT_REMEMBER_WIDGET_POSITION;
            SpeedUnit = DEFAULT_SPEED_UNIT;
            PositionTop = DEFAULT_POSITION_TOP;
            PositionLeft = DEFAULT_POSITION_LEFT;
            BackgroundColor = DEFAULT_BG_COLOR;
            ForegroundColor = DEFAULT_FG_COLOR;
            Opacity = DEFAULT_OPACITY;
            FontSize = DEFAULT_FONT_SIZE;
            DataSpeedToShow = DEFAULT_DATA_SPEED_OPTION;
        }
    }
}
