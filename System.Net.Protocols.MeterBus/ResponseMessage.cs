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
        public static Base Parse(byte[] buffer)
        {
            if (buffer.Length == 0)
                return new Empty();

            // RSP_UD
            if ((buffer[0] & 0x0f) != 0x08)
                throw new InvalidDataException();

            var ud = new _UD_Base(
                accessDemand: (buffer[0] & 0x20) != 0,
                dataFlowControl: (buffer[0] & 0x10) != 0,
                address: buffer[1]);

            using (var stream = new MemoryStream(buffer, 2, buffer.Length - 2))
            using (var source = new BinaryReader(stream))
            {
                // Response type
                switch (source.ReadByte())
                {
                    case 0x70: //report of general application errors
                        return new ApplicationError(ud, source.ReadByte());
                    case 0x71: //report of alarm status
                        return new AlarmStatus(ud, source.ReadByte());
                    case 0x72: //
                    case 0x76: //variable data respond
                        return new VariableData(ud, source);
                    case 0x73: //
                    case 0x77: //fixed data respond
                        return new FixedData(ud, source);
                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }
}
