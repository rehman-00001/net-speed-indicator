using Serilog;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace net_speed_indicator.Views.UserControls
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class About : UserControl
    {
        public About()
        {
            InitializeComponent();
            Log.Information("{0}:: About - Instance Created", GetType().Name);
        }

        private void Hyperlink_SourceCode_Click(object sender, RequestNavigateEventArgs e)
        {
            // for .NET Core you need to add UseShellExecute = true
            // see https://docs.microsoft.com/dotnet/api/system.diagnostics.processstartinfo.useshellexecute#property-value
            _ = Process.Start(new ProcessStartInfo(Constants.RepositoryUrl) { UseShellExecute = true });
            e.Handled = true;
            Log.Information("{0}::Hyperlink_SourceCode_Click - ", GetType().Name);
        }

        private void Button_GithubAccount_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _ = Process.Start(new ProcessStartInfo(Constants.GithubAccountUrl) { UseShellExecute = true });
            e.Handled = true;
            Log.Information("{0}::Hyperlink_GithubAccount_Click - ", GetType().Name);
        }

        private void Button_InstagramAccount_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _ = Process.Start(new ProcessStartInfo(Constants.InstagramAccountUrl) { UseShellExecute = true });
            e.Handled = true;
            Log.Information("{0}::Hyperlink_InstagramAccount_Click - ", GetType().Name);
        }

        private void InstaAccountIdRedirect_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Button_InstagramAccount_Click(sender, e);
            Log.Information("{0}::Hyperlink_InstagramAccount_Click - ", GetType().Name);
        }
    }
}
