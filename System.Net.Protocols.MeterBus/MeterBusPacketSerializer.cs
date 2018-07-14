using System.Net.Protocols.MeterBus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Bindings;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public sealed class MeterBusPacketSerializer : IPacketSerializer
    {
        public INetworkPacket Deserialize(byte[] buffer)
        {
            var result_length = 0;

            switch ((ResponseCodes)buffer[result_length])
            {
                case ResponseCodes.ACK: return new AckMeterBusPackage();
                case ResponseCodes.SHORT_FRAME_START:
                    {
                        if (buffer.Length != 5)
                            throw new InvalidDataException();

                        if ((ResponseCodes)buffer[result_length] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();

                        if (buffer[result_length - 1] != CheckSum(buffer, 1, 2))
                            throw new InvalidDataException();

                    }
                    break;
                case ResponseCodes.LONG_FRAME_START:
                    {
                        if ((ResponseCodes)buffer[result_length] != ResponseCodes.LONG_FRAME_START)
                            throw new InvalidDataException();

                        if (buffer[1] != buffer[2])
                            throw new InvalidDataException();

                        var length = (int)buffer[1];

                        if ((ResponseCodes)buffer[length + 5] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();

                        if (buffer[length + 4] != CheckSum(buffer, 4, length))
                            throw new InvalidDataException();

                        Array.Copy(buffer, 4, buffer, 0, length);
                        Array.Resize(ref buffer, length);

                        var asd = ResponseMessage.Parse(buffer);

                        return new LongMeterBusPackage((ControlCommand)buffer[0], (ControlCommandInformation)buffer[2], buffer[1], buffer.Skip(3).Take(buffer.Length - 3).ToArray());
                    }
                default:
                    throw new InvalidDataException();
            }

            throw new NotImplementedException();
        }

        public byte[] Serialize(INetworkPacket package, out int length)
        {
            using (var stream = new MemoryStream())
            {
                switch (package)
                {
                    case AckMeterBusPackage ackPackage: ackPackage.Write(stream); break;
                    case ShortMeterBusPackage shortPackage: shortPackage.Write(stream); break;
                    case LongMeterBusPackage longPackage: longPackage.Write(stream); break;
                    case ControlMeterBusPackage controlPackage: controlPackage.Write(stream); break;
                    default: throw new NotImplementedException();
                }

                length = (int)stream.Length;

                return stream.ToArray();
            }
        }

        private static byte CheckSum(byte[] buffer, int offset, int length)
        {
            return (byte)buffer.Skip(offset).Take(length).Sum(b => b);
        }
    }
}
