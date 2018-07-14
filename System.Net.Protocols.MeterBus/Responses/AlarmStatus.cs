using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Responses
{
    public sealed class AlarmStatus : _UD_Base
    {
        public byte Status { get; }

        public AlarmStatus(_UD_Base ud, byte status) : base(ud.AccessDemand, ud.DataFlowControl, ud.Address)
        {
            this.Status = status;
        }

        public override string ToString()
        {
            return string.Format("{0}({1}):{2:x2}", this.GetType().Name, base.ToString(), Status);
        }
    }
}
