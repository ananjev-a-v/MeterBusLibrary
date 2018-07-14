using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Bindings;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Protocols.MeterBus
{
    public abstract class MeterBusPackage : INetworkPacket
    {
        private readonly byte[] _payload;

        public byte[] Payload => _payload;

        public MeterBusPackage()
        {
      
        }      

        public MeterBusPackage(byte[] data)
        {
            _payload = data;
        }
    }
}
