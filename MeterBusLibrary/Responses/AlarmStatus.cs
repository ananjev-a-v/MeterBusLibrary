using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Responses
{
    public class AlarmStatus : _UD_Base
    {
        public byte status { get; }

        public AlarmStatus(_UD_Base UD, byte status)
                : base(UD.AccessDemand, UD.DataFlowControl, UD.Address)
        {
            this.status = status;
        }

        public override string ToString()
        {
            return String.Format("{0}({1}):{2:x2}", this.GetType().Name, base.ToString(), status);
        }
    }
}
