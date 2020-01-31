namespace CS.Utils.Network
{
    public struct BeaconMessage
    {
        public BeaconMessage(string serviceId, ushort port, string serviceName)
        {
            ServiceId = serviceId;
            Port = port;
            ServiceName = serviceName;
        }

        public string ServiceId { get; }
        public ushort Port { get; }
        public string ServiceName { get; }
    }
}
