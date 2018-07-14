using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public sealed class AckMeterBusPackage : MeterBusPackage
    {
        internal override void Write(Stream stream)
        {
            stream.WriteByte((byte)ResponseCodes.ACK);
        }
    }
}
