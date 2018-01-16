using MeterBusLibrary.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Responses
{
    public abstract class _UD_Data : _UD_Base
    {
        public abstract UInt32 IdentificationNo { get; }
        public abstract byte AccessNo { get; }
        public abstract MediumFixedData Medium { get; }

        public _UD_Data(_UD_Base UD)
                : base(UD.AccessDemand, UD.DataFlowControl, UD.Address)
        { }
    }
}
