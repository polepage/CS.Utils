using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CS.Utils.Network
{
    public class Beacon: IDisposable
    {
        private readonly UdpClient _udp;

        private bool _running;

        public Beacon(string serviceId, ushort port, string serviceName)
        {
            Message = new BeaconMessage(serviceId, port, serviceName);

            _udp = new UdpClient(new IPEndPoint(IPAddress.Any, Protocol.DiscoveryPort));
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udp.AllowNatTraversal(true);

            Task.Run(Receive);
        }

        public BeaconMessage Message { get; }

        public void Dispose()
        {
            _running = false;
            _udp.Send(new byte[0], 0, new IPEndPoint(IPAddress.Loopback, Protocol.DiscoveryPort));
            _udp.Dispose();
        }

        private void Receive()
        {
            _running = true;
            while(_running)
            {
                var remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _udp.Receive(ref remote);

                if (buffer == null || buffer.Length == 0)
                {
                    continue;
                }

                if (Protocol.TryDecodeProbeMessage(buffer, out string remoteId) && Message.ServiceId.Equals(remoteId))
                {
                    byte[] response = Protocol.Encode(Message);
                    _udp.Send(response, response.Length, remote);
                }
            }
        }
    }
}
