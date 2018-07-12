﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    public enum MediumFixedData : byte
    {
        Other = 0x00, //Other
        Oil = 0x01, //Oil
        Electricity = 0x02, //Electricity
        Gas = 0x03, //Gas
        Heat = 0x04, //Heat
        Steam = 0x05, //Steam
        HotWater = 0x06, //Hot Water
        Water = 0x07, //Water
        HCA = 0x08, //H.C.A.
        Reserved = 0x09, //Reserved
        Gas2 = 0x0A, //Gas Mode 2
        Hea2 = 0x0B, //Heat Mode 2
        HotWater2 = 0x0C, //Hot Water Mode 2
        Water2 = 0x0D, //Water Mode 2
        HCA2 = 0x0E, //H.C.A. Mode 2
        Reserved2 = 0x0F, //Reserved
    }
}
