using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public enum ControlCommand
    {
        SND_NKE = 0x40,
        SND_UD = 0x53,
        REQ_UD2 = 0x7b, //0x4b | 0x5b | 0x6b | 0x7b
        REQ_UD1 = 0x5a,
        RSP_UD = 0x08,
    }
}
