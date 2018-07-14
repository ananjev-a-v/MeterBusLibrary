using System;
using System.Collections.Generic;
using System.Net.Bindings;
using System.Text;

namespace MeterBusLibrary
{
    public sealed class MBusPacketSerializer : IPacketSerializer
    {
        public INetworkPacket Deserialize(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(INetworkPacket package, out int length)
        {
            var data = new byte[] { 0x10, 0x40, 0x0a, 0x4a, 0x16 };

            var ok = new byte[] { 0xe5 };

            length = data.Length;

            return data;
        }
    }
}
