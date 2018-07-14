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
            int result_length = 0, result_offset;

            result_length++;

            switch ((ResponseCodes)buffer[result_length - 1])
            {
                case ResponseCodes.ACK:
                    {
                        result_offset = 0;
                    }
                    break;
                case ResponseCodes.SHORT_FRAME_START:
                    {
                        if ((ResponseCodes)buffer[result_length - 1] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();

                        if (buffer[result_length - 2] != CheckSum(buffer, 1, 2))
                            throw new InvalidDataException();

                        result_offset = 1;
                        result_length -= result_offset + 2;
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
                    case AckMeterBusPackage ackPackage:
                        {
                        
                        }
                        break;
                    case ShortMeterBusPackage shortPackage:
                        {
                            stream.WriteByte((byte)ResponseCodes.SHORT_FRAME_START);
                            stream.Write(shortPackage.Payload, 0, shortPackage.Payload.Length);
                            var checkSum = CheckSum(shortPackage.Payload, 0, shortPackage.Payload.Length);
                            stream.WriteByte(checkSum);
                            stream.WriteByte((byte)ResponseCodes.FRAME_END);
                        }
                        break;
                    case LongMeterBusPackage longPackage:
                        {
                            stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
                            stream.WriteByte((byte)longPackage.Payload.Length);
                            stream.WriteByte((byte)longPackage.Payload.Length);
                            stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
                            stream.Write(longPackage.Payload, 0, longPackage.Payload.Length);
                            stream.WriteByte(CheckSum(longPackage.Payload, 0, longPackage.Payload.Length));
                            stream.WriteByte((byte)ResponseCodes.FRAME_END);
                        }
                        break;
                    case ControlMeterBusPackage longPackage:
                        {

                        }
                        break;
                    default: throw new NotImplementedException();
                }

                length = (int)stream.Length;

                return stream.ToArray();
            }

            //var data = new byte[] { 0x10, 0x40, 0x0a, 0x4a, 0x16 };

            //length = data.Length;

            //return data;
        }

        private static byte CheckSum(byte[] buffer, int offset, int length)
        {
            return (byte)buffer.Skip(offset).Take(length).Sum(b => b);
        }
    }
}
