using net_speed_indicator.Models;
using System;
using System.Reflection;

namespace net_speed_indicator
{

    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class LabelAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public LabelAttribute(string value)
        {
            StringValue = value;
        }


        #endregion
    }

    public enum SpeedUnit
    {
        [Label("KB,MB/s (kilo/mega bytes per second)")]
        BytesPerSecond,
        [Label("kb,mb/s (kilo/mega bits per second)")]
        BitsPerSecond
    }

    public enum SettingsTab
    {
        General,
        Appearance,
        NetworkInterface,
        Theme,
        About
    }

    public static class Constants
    {
        /// <summary>
        /// Will get the string value for a given enums value, this will
        /// only work if you assign the Label attribute to
        /// the items in your enum.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static readonly string AppName = "Net Speed Indicator";
        public static readonly SettingsTab InitialTab = SettingsTab.General;
        public static readonly string RepositoryUrl = "https://github.com/rehman-00001/"; // TODO: to be updated
        public static readonly string GithubAccountUrl = "https://github.com/rehman-00001/"; // TODO: to be updated
        public static readonly string InstagramAccountUrl = "https://github.com/rehman-00001/"; // TODO: to be updated
        public static readonly DataSpeedOption[] DataSpeedOptions = new DataSpeedOption[] {
            new DataSpeedOption(0, "Both Uploads & Downloads"),
            new DataSpeedOption(1, "Uploads only"),
            new DataSpeedOption(2, "Downloads only")
        };

        public static string GetLabel(this Enum value)
        {
            // Get the typeta
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            LabelAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(LabelAttribute), false) as LabelAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }

}
