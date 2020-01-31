using System;
using System.Net;

namespace CS.Utils.Network
{
    public class BeaconLocation
    {
        public BeaconLocation(BeaconMessage message, IPEndPoint remote)
        {
            ServiceName = message.ServiceName;
            Address = new IPEndPoint(remote.Address, message.Port);
            RefreshTime();
        }

        public string ServiceName { get; }
        public IPEndPoint Address { get; }
        public DateTime Timestamp { get; private set; }

        public void RefreshTime()
        {
            Timestamp = DateTime.Now;
        }

        public override int GetHashCode()
        {
            return Address.GetHashCode() ^ ServiceName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode() &&
                   obj is BeaconLocation other &&
                   Address.Equals(other.Address) &&
                   ServiceName.Equals(other.ServiceName);
        }

        public override string ToString()
        {
            return ServiceName;
        }
    }
}
