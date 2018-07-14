using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    internal enum ResponseCodes : byte
    {
        ACK = 0xE5,
        SHORT_FRAME_START = 0x10,
        LONG_FRAME_START = 0x68,
        CONTROL_FRAME_START = 0x68,
        FRAME_END = 0x16,
    }
}
