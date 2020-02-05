using System;
using System.Net;
using System.Net.Sockets;

namespace CS.Utils.Network
{
    public class Beacon: IDisposable
    {
        private readonly UdpClient _udp;

        public Beacon(string serviceId, ushort port, string serviceName)
        {
            Message = new BeaconMessage(serviceId, port, serviceName);

            _udp = new UdpClient(new IPEndPoint(IPAddress.Any, Protocol.DiscoveryPort));
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _udp.AllowNatTraversal(true);

            _udp.BeginReceive(Receive, null);
        }

        public BeaconMessage Message { get; }

        public void Dispose()
        {
            _udp.Dispose();
        }

        private void Receive(IAsyncResult asyncResult)
        {
            try
            {
                var remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _udp.EndReceive(asyncResult, ref remote);

                if (buffer != null && buffer.Length > 0 &&
                    Protocol.TryDecodeProbeMessage(buffer, out string remoteId) &&
                    Message.ServiceId.Equals(remoteId))
                {
                    byte[] response = Protocol.Encode(Message);
                    _udp.Send(response, response.Length, remote);
                }

                _udp.BeginReceive(Receive, null);
            }
            catch (ObjectDisposedException)
            {
                // Normal behavior of UdpClient/Socket to throw when closed.
            }
        }
    }
}
