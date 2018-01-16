using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Responses
{
    public class ApplicationError : _UD_Base
    {
        public enum Codes : byte
        {
            Unspecified = 0, //Unspecified error: also if data field is missing
            Unimplemented_CI = 1, //Unimplemented CI-Field
            BufferTooLong = 2, //Buffer too long, truncated
            TooManyRecords = 3, //Too many records
            PrematureEnd = 4, //Premature end of record
            TooManyDIFEs = 5, //More than 10 DIFE´s
            TooManyVIFEs = 6, //More than 10 VIFE´s
            Reserved = 7, //Reserved
            Busy = 8, //Application too busy for handling readout request
            TooManyReadouts = 9, //Too many readouts(for slaves with limited readouts per time)            }
        }

        public Codes Code { get; }

        public ApplicationError(_UD_Base UD, Codes code)
                :base(UD.AccessDemand, UD.DataFlowControl, UD.Address)
            {
            this.Code = code;
        }

        public ApplicationError(_UD_Base UD, byte code)
                : this(UD, (Codes)code)
            { }

        public override string ToString()
        {
            return String.Format("{0}({1}):{2}", this.GetType().Name, base.ToString(), Code);
        }
    }
}
