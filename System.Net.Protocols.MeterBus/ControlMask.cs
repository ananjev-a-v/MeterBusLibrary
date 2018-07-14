using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public enum ControlMask
    {
        SND_NKE = 0x40,
        SND_UD = 0x53,
        REQ_UD2 = 0x5B,
        REQ_UD1 = 0x5A,
        RSP_UD = 0x08,
    }
}
