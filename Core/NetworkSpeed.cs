using net_speed_indicator.Utilities;

namespace net_speed_indicator
{
    internal class NetworkSpeed
    {
        public string Name;
        private readonly long BytesSent;
        private readonly long BytesReceived;
        private long BitsSent { get => BytesSent * 8; }
        private long BitsReceived { get => BytesReceived * 8; }

        public NetworkSpeed(string name, long bytesSent, long bytesReceived)
        {
            Name = name;
            BytesReceived = bytesReceived;
            BytesSent = bytesSent;
        }

        public string GetBytesSent()
        {
            return CommonUtils.BytesSizeSuffix(BytesSent, 2) + "/s";
        }
        public string GetBytesReceived()
        {
            return CommonUtils.BytesSizeSuffix(BytesReceived, 2) + "/s";
        }
        public string GetBitsSent()
        {
            return CommonUtils.BitsSizeSuffix(BitsSent, 2) + "/s";
        }
        public string GetBitsReceived()
        {
            return CommonUtils.BitsSizeSuffix(BitsReceived, 2) + "/s";
        }
    }
}
