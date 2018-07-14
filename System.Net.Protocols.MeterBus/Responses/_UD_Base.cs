using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Responses
{
    public class _UD_Base : Base
    {
        public bool AccessDemand { get; }

        public bool DataFlowControl { get; }

        public byte Address { get; }

        internal _UD_Base(bool accessDemand, bool dataFlowControl, byte address)
        {
            this.AccessDemand = accessDemand;
            this.DataFlowControl = dataFlowControl;
            this.Address = address;
        }

        public override string ToString()
        {
            return string.Format("Address={0:x2}, AccessDemand={1}, DataFlowControl={2}", Address, AccessDemand, DataFlowControl);
        }
    }
}
