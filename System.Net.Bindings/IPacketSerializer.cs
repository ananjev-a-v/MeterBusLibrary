using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Bindings
{
    public interface IPacketSerializer
    {
        INetworkPacket Deserialize(byte[] buffer);

        byte[] Serialize(INetworkPacket package, out int length);
    }
}
