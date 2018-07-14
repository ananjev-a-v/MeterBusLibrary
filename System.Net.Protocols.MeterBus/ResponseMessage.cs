using System.Net.Protocols.MeterBus.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus
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
                switch ((ControlCommandInformation)source.ReadByte())
                {
                    case ControlCommandInformation.ERROR_GENERAL: //report of general application errors
                        return new ApplicationError(ud, source.ReadByte());
                    case ControlCommandInformation.STATUS_ALARM: //report of alarm status
                        return new AlarmStatus(ud, source.ReadByte());
                    case ControlCommandInformation.RESP_VARIABLE: //
                    case ControlCommandInformation.RESP_VARIABLE_MSB: //variable data respond
                        return new VariableData(ud, source);
                    case ControlCommandInformation.RESP_FIXED: //
                    case ControlCommandInformation.RESP_FIXED_MSB: //fixed data respond
                        return new FixedData(ud, source);
                    default:
                        throw new InvalidDataException();
                }
            }
        }
    }
}
