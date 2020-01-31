using System;
using System.Linq;
using System.Net;
using System.Text;

namespace CS.Utils.Network
{
    static class Protocol
    {
        public static readonly int DiscoveryPort = 35891;
        public static readonly string BeaconMessageTag = "BMv1";
        public static readonly string ProbeMessageTag = "PMv1";

        private static readonly int LengthSize = sizeof(short);

        public static byte[] Encode(string probeMessage)
        {
            return InternalEncode(ProbeMessageTag)
                .Concat(InternalEncode(probeMessage))
                .ToArray();
        }

        public static byte[] Encode(BeaconMessage beaconMessage)
        {
            return InternalEncode(BeaconMessageTag)
                .Concat(InternalEncode(beaconMessage.ServiceId))
                .Concat(BitConverter.GetBytes((ushort)IPAddress.HostToNetworkOrder((short)beaconMessage.Port)))
                .Concat(InternalEncode(beaconMessage.ServiceName))
                .ToArray();
        }

        public static string DecodeProbeMessage(byte[] data)
        {
            if (!TryDecodeProbeMessage(data, out string message))
            {
                throw new ArgumentException("Byte array is not a ProbeMessage");
            }

            return message;
        }

        public static bool TryDecodeProbeMessage(byte[] data, out string probeMessage)
        {
            string tag = InternalDecode(data, out int index);
            if (!ProbeMessageTag.Equals(tag))
            {
                probeMessage = null;
                return false;
            }

            probeMessage = InternalDecode(data, index);
            return true;
        }

        public static BeaconMessage DecodeBeaconMessage(byte[] data)
        {
            if (!TryDecodeBeaconMessage(data, out BeaconMessage message))
            {
                throw new ArgumentException("Byte array is not a BeaconMessage");
            }

            return message;
        }

        public static bool TryDecodeBeaconMessage(byte[] data, out BeaconMessage beaconMessage)
        {
            string tag = InternalDecode(data, out int index);
            if (!BeaconMessageTag.Equals(tag))
            {
                beaconMessage = new BeaconMessage();
                return false;
            }

            string serviceId = InternalDecode(data, index, out index);
            
            ushort port = (ushort)IPAddress.NetworkToHostOrder((short)BitConverter.ToUInt16(data, index));
            index += sizeof(ushort);

            string serviceName = InternalDecode(data, index);

            beaconMessage = new BeaconMessage(serviceId, port, serviceName);
            return true;
        }

        private static byte[] InternalEncode(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            short length = IPAddress.HostToNetworkOrder((short)bytes.Length);

            return BitConverter.GetBytes(length)
                .Concat(bytes)
                .ToArray();
        }

        private static string InternalDecode(byte[] data, int index)
        {
            return InternalDecode(data, index, out int _);
        }

        private static string InternalDecode(byte[] data, out int length)
        {
            return InternalDecode(data, 0, out length);
        }

        private static string InternalDecode(byte[] data, int index, out int length)
        {
            length = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(data.Take(LengthSize).ToArray(), index));
            return Encoding.UTF8.GetString(data.Skip(LengthSize + index).Take(length).ToArray());
        }
    }
}
