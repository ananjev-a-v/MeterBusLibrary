using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Helpers
{
    internal static class VariableLengthData
    {
        public static string ReadString(BinaryReader source)
        {
            int Length = source.ReadByte();
            byte[] buf = source.ReadBytes(Length);
            Array.Reverse(buf);

            return ASCIIEncoding.ASCII.GetString(buf);
        }

        public static object ReadValue(BinaryReader source)
        {
            var Length = source.ReadByte();

            if ((Length >= 0x00) && (Length <= 0xbf))
            { // ASCII string with LVAR characters
                byte[] buf = source.ReadBytes(Length);
                Array.Reverse(buf);
                return ASCIIEncoding.ASCII.GetString(buf);
            }
            else if ((Length >= 0xc0) && (Length <= 0xcf))
            { // positive BCD number with (LVAR - C0h) · 2 digits
                Length -= 0xc0;
                byte[] buf = source.ReadBytes(Length);
                if (Length <= 1)
                    return sbyte.Parse(buf.BCDToString());
                else if (Length <= 2)
                    return Int16.Parse(buf.BCDToString());
                else if (Length <= 4)
                    return Int32.Parse(buf.BCDToString());
                else if (Length <= 9)
                    return Int64.Parse(buf.BCDToString());
                else
                    throw new OverflowException();
            }
            else if ((Length >= 0xd0) && (Length <= 0xdf))
            { // negative BCD number with (LVAR - D0h) · 2 digits
                Length -= 0xd0;
                byte[] buf = source.ReadBytes(Length);
                if (Length <= 1)
                    return -sbyte.Parse(buf.BCDToString());
                else if (Length <= 2)
                    return -Int16.Parse(buf.BCDToString());
                else if (Length <= 4)
                    return -Int32.Parse(buf.BCDToString());
                else if (Length <= 9)
                    return -Int64.Parse(buf.BCDToString());
                else
                    throw new OverflowException();
            }
            else if ((Length >= 0xe0) && (Length <= 0xef))
            { // binary number with (LVAR - E0h) bytes
                Length -= 0xe0;
                byte[] buf = source.ReadBytes(Length);
                if (Length <= 1)
                    return (sbyte)(new byte[1 - buf.Length].Concat(buf).ToArray())[0];
                else if(Length <= 2)
                    return BitConverter.ToInt16(new byte[2 - buf.Length].Concat(buf).ToArray(), 0);
                else if (Length <= 4)
                    return BitConverter.ToInt32(new byte[4 - buf.Length].Concat(buf).ToArray(), 0);
                else if (Length <= 8)
                    return BitConverter.ToInt64(new byte[8 - buf.Length].Concat(buf).ToArray(), 0);
                else
                    throw new OverflowException();
            }
            else if ((Length >= 0xf0) && (Length <= 0xfa))
            { // floating point number with (LVAR - F0h) bytes [to be defined]
                Length -= 0xf0;
                byte[] buf = source.ReadBytes(Length);
                if (Length == sizeof(Single))
                    return BitConverter.ToSingle(buf, 0);
                else if (Length == sizeof(Double))
                    return BitConverter.ToDouble(buf, 0);
                else
                    throw new NotImplementedException();
            }
            throw new NotImplementedException();
        }
    }
}
