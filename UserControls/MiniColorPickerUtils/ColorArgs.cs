using System;

namespace net_speed_indicator.UserControls.MiniColorPickerUtils
{
    public class ColorArgs : EventArgs
    {
        public Color Color;
        public ColorArgs(Color Color)
        {
            this.Color = Color;
        }
    }
}
