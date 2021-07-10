using System;
using Serilog;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using net_speed_indicator.Models;

namespace net_speed_indicator
{
    internal class SpeedChecker
    {
        private readonly DispatcherTimer timer;
        private readonly AppData AppData = AppData.Instance;
        private INetworkSpeedListener Listener;
        public long PreviousBytesSent, PreviousBytesReceived;
        public NetworkInterface NetworkInterface { get; set; }

        public SpeedChecker()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += CheckSpeed;
            Log.Information("{0}:: SpeedChecker() - Instance created", GetType().Name);
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
            Log.Information("{0}:: Start() - SpeedChecker started", GetType().Name);
        }

        public void Stop()
        {
            timer.Stop();
            Log.Information("{0}:: Stop() - SpeedChecker stopped", GetType().Name);
        }
    }
}
