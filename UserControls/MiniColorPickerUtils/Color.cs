using System.Windows.Media;

namespace net_speed_indicator.UserControls.MiniColorPickerUtils
{
    public class Color
    {
        public string Name { get; set; }
        public Brush Brush { get; set; }
        public Color(string name, Brush brush)
        {
            Name = name;
            Brush = brush;
        }
    }
}
