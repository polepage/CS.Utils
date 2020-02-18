using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CS.Utils.Network
{
    public class Probe : INotifyCollectionChanged, IDisposable
    {
        private readonly UdpClient _udp;
        private readonly ObservableCollection<BeaconLocation> _locations;
        private readonly SynchronizationContext _callerThread;

        private bool _searching;

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { _locations.CollectionChanged += value; }
            remove { _locations.CollectionChanged -= value; }
        }

        public Probe(string serviceId)
        {
            ServiceId = serviceId;

            _udp = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
            _udp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            try
            {
                _udp.AllowNatTraversal(true);
            }
            catch (SocketException) { /* Use NAT traversal only if supported */ }

            _locations = new ObservableCollection<BeaconLocation>();

            _callerThread = SynchronizationContext.Current;

            _udp.BeginReceive(Receive, null);
            Task.Run(Search);
        }

        public string ServiceId { get; }
        public IReadOnlyCollection<BeaconLocation> Locations => _locations;

        public void Dispose()
        {
            _searching = false;
            _udp.Dispose();
        }

        private void Receive(IAsyncResult asyncResult)
        {
            try
            {
                var remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _udp.EndReceive(asyncResult, ref remote);

                if (buffer != null && buffer.Length > 0 &&
                    Protocol.TryDecodeBeaconMessage(buffer, out BeaconMessage message) &&
                    ServiceId.Equals(message.ServiceId))
                {
                    AddLocation(message, remote);
                }

                _udp.BeginReceive(Receive, null);
            }
            catch (ObjectDisposedException)
            {
                // Normal behavior of UdpClient/Socket to throw when closed.
            }
        }

        private async void Search()
        {
            _searching = true;
            while (_searching)
            {
                try
                {
                    byte[] service = Protocol.Encode(ServiceId);
                    _udp.Send(service, service.Length, new IPEndPoint(IPAddress.Broadcast, Protocol.DiscoveryPort));

                    await Task.Delay(2000);

                    PruneLocations();
                }
                catch (ObjectDisposedException)
                {
                    // Normal behavior of UdpClient/Socket to throw when closed.
                    _searching = false;
                }
            }
        }

        private void AddLocation(BeaconMessage message, IPEndPoint address)
        {
            // Modifications to the collection are done in the thread that created the Probe
            // To ensure handler to the CollectionChanged event are not surprised
            var location = new BeaconLocation(message, address);
            _callerThread.Send(s =>
            {
                var existingLocation = _locations.FirstOrDefault(bl => bl.Equals(location));
                if (existingLocation != null)
                {
                    existingLocation.RefreshTime();
                }
                else
                {
                    
                    _locations.Add(location);
                }
            }, null);
        }

        private void PruneLocations()
        {
            // Modifications to the collection are done in the thread that created the Probe
            // To ensure handler to the CollectionChanged event are not surprised
            _callerThread.Send(s =>
            {
                var toRemove = new List<BeaconLocation>(_locations.Where(bl => bl.Timestamp < (DateTime.Now - TimeSpan.FromSeconds(5))));
                foreach (BeaconLocation location in toRemove)
                {
                    _locations.Remove(location);
                }
            }, null);
        }
    }
}
