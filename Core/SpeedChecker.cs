using System;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using net_speed_indicator.Models;
using net_speed_indicator.Utilities;

namespace net_speed_indicator
{
    internal class SpeedChecker
    {
        private readonly DispatcherTimer timer;
        private readonly AppData AppData = AppData.Instance;
        private INetworkSpeedListener Listener;
        public long PreviousBytesSent = 0, PreviousBytesReceived = 0;
        public NetworkInterface NetworkInterface { get; set; }

        public SpeedChecker()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += CheckSpeed;
        }

        private void CheckSpeed(object sender, EventArgs e)
        {
            if (NetworkInterface == null)
            {
                return;
            }

            Console.WriteLine("Checking...");
            string name = NetworkInterface.Name;
            
            long bytesSent, bytesReceived;
            bytesSent = NetworkInterface.GetIPv4Statistics().BytesSent - PreviousBytesSent;
            bytesReceived = NetworkInterface.GetIPv4Statistics().BytesReceived - PreviousBytesReceived;

            this.Listener.OnSpeedCheck(new NetworkSpeed(name, bytesSent, bytesReceived));
            PreviousBytesSent = NetworkInterface.GetIPv4Statistics().BytesSent;
            PreviousBytesReceived = NetworkInterface.GetIPv4Statistics().BytesReceived;
        }

        public void SetListener(INetworkSpeedListener Listener)
        {
            this.Listener = Listener;
        }

        public void Start()
        {
            timer.Start();            
        }

        public void Stop()
        {
            timer.Stop();
            Console.WriteLine("Timer stopped");
        }
    }
}
