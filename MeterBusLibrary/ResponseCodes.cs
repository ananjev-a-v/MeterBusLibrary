using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary
{
    enum ResponseCodes : byte
    {
        ACK = 0xE5,
        LONG_FRAME = 0x68,
        SHORT_FRAME_START = 0x10,
        FRAME_END = 0x16,
    }
}
