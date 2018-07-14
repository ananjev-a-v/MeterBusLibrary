using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Bindings;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Protocols.MeterBus
{
    public abstract class MeterBusPackage : INetworkPacket
    {
        internal abstract void Write(Stream stream);

        protected static byte CheckSum(byte[] buffer, int offset, int length)
        {
            return (byte)buffer.Skip(offset).Take(length).Sum(b => b);
        }
    }
}
