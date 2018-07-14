using System.Net.Protocols.MeterBus.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Responses
{
    public abstract class _UD_Data : _UD_Base
    {
        public abstract UInt32 IdentificationNo { get; }

        public abstract byte AccessNo { get; }

        public abstract MediumFixedData Medium { get; }

        public _UD_Data(_UD_Base ud) : base(ud.AccessDemand, ud.DataFlowControl, ud.Address)
        {

        }
    }
}
