using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public enum FrameType
    {
        ACK = 0xE5,
        LONG = 0x68,
        SHORT = 0x10,
        CONTROL = 0x00,
    }
}
