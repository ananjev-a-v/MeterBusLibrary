using MeterBusLibrary.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeterBusLibrary
{
    public static class ResponseMessage
    {
        public static Base Parse(byte[] buf)
        {
            if (buf.Length == 0)
            {
                return new Empty();
            }

            // RSP_UD
            if ((buf[0] & 0x0f) != 0x08)
                throw new InvalidDataException();
            _UD_Base UD = new _UD_Base(
                accessDemand: (buf[0] & 0x20) != 0,
                dataFlowControl: (buf[0] & 0x10) != 0,
                address: buf[1]
                );

            using (MemoryStream stream = new MemoryStream(buf, 2, buf.Length - 2))
            using (BinaryReader source = new BinaryReader(stream))
            {
                // Response type
                switch (source.ReadByte())
                {
                    case 0x70: //report of general application errors
                        return new ApplicationError(UD, source.ReadByte());
                    case 0x71: //report of alarm status
                        return new AlarmStatus(UD, source.ReadByte());
                    case 0x72: //
                    case 0x76: //variable data respond
                        return new VariableData(UD, source);
                    case 0x73: //
                    case 0x77: //fixed data respond
                        return new FixedData(UD, source);
                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }
}
