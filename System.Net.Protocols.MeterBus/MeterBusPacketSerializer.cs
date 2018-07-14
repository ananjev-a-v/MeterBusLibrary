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
            var result_length = 1;

            switch ((ResponseCodes)buffer[result_length - 1])
            {
                case ResponseCodes.ACK: return new AckMeterBusPackage();
                case ResponseCodes.SHORT_FRAME_START:
                    {
                        if (buffer.Length != 5)
                            throw new InvalidDataException();

                        if ((ResponseCodes)buffer[result_length - 1] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();

                        if (buffer[result_length - 2] != CheckSum(buffer, 1, 2))
                            throw new InvalidDataException();


                    }
                    break;
                case ResponseCodes.LONG_FRAME:
                    {
                        if ((ResponseCodes)buffer[result_length - 1] != ResponseCodes.LONG_FRAME)
                            throw new InvalidDataException();

                        if (buffer[1] != buffer[2])
                            throw new InvalidDataException();


                    }
                    break;
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
                    case AckMeterBusPackage ackPackage: break;
                    case ShortMeterBusPackage shortPackage: shortPackage.Write(stream); break;
                    case LongMeterBusPackage longPackage: longPackage.Write(stream); break;
                    case ControlMeterBusPackage longPackage: break;
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
