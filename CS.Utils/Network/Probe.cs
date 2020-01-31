using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CS.Utils.Network
{
    public class Probe : INotifyCollectionChanged, IDisposable
    {
        private readonly UdpClient _udp;
        private readonly ObservableCollection<BeaconLocation> _locations;

        private bool _receiving;
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
            _udp.AllowNatTraversal(true);

            _locations = new ObservableCollection<BeaconLocation>();

            Task.Run(Receive);
            Task.Run(Search);
        }

        public string ServiceId { get; }
        public IReadOnlyCollection<BeaconLocation> Locations => _locations;

        public void Dispose()
        {
            _receiving = false;
            _searching = false;
            _udp.Close();
            _udp.Dispose();
        }

        private void Receive()
        {
            _receiving = true;
            while (_receiving)
            {
                var remote = new IPEndPoint(IPAddress.Any, 0);
                byte[] buffer = _udp.Receive(ref remote);

                if (buffer == null || buffer.Length == 0)
                {
                    return;
                }

                if (Protocol.TryDecodeBeaconMessage(buffer, out BeaconMessage message) && ServiceId.Equals(message.ServiceId))
                {
                    AddLocation(message, remote);
                }
            }
        }

        private async void Search()
        {
            _searching = true;
            while (_searching)
            {
                byte[] service = Protocol.Encode(ServiceId);
                _udp.Send(service, service.Length, new IPEndPoint(IPAddress.Broadcast, Protocol.DiscoveryPort));

                await Task.Delay(2000);

                PruneLocations();
            }
        }

        private void AddLocation(BeaconMessage message, IPEndPoint address)
        {
            var location = new BeaconLocation(message, address);
            var existingLocation = _locations.First(bl => bl.Equals(location));
            if (existingLocation != null)
            {
                existingLocation.RefreshTime();
            }
            else
            {
                _locations.Add(location);
            }
        }

        private void PruneLocations()
        {
            var toRemove = new List<BeaconLocation>(_locations.Where(bl => bl.Timestamp < (DateTime.Now - TimeSpan.FromSeconds(5))));
            foreach (BeaconLocation location in toRemove)
            {
                _locations.Remove(location);
            }
        }
    }
}
