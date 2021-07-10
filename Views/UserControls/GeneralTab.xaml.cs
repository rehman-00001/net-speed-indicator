using Serilog;
using System.Windows.Controls;
namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for GeneralTab.xaml
    /// </summary>
    public partial class GeneralTab : UserControl
    {
        public GeneralTab()
        {
            InitializeComponent();
            Log.Information("{0}::GeneralTab - Instance created", GetType().Name);
        }

    }
}
