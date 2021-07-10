using Serilog;
using System.Windows.Controls;

namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for NetworkInterfaceTab.xaml
    /// </summary>
    public partial class NetworkInterfaceTab : UserControl
    {
        public NetworkInterfaceTab()
        {
            InitializeComponent();
            Log.Information("{0}::NetworkInterfaceTab - Instance created", GetType().Name);
        }
    }
}
