using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public sealed class ShortMeterBusPackage : MeterBusPackage
    {
        private readonly ControlCommand _control;
        private readonly byte _address;
        private readonly byte _crc;

        public ShortMeterBusPackage(ControlCommand control, byte address)
        {
            _control = control;
            _address = address;
            var data = new byte[] { (byte)_control, _address };
            _crc = CheckSum(data, 0, data.Length);
        }

        internal override void Write(Stream stream)
        {
            stream.WriteByte((byte)ResponseCodes.SHORT_FRAME_START);
            stream.WriteByte((byte)_control);
            stream.WriteByte(_address);
            stream.WriteByte(_crc);
            stream.WriteByte((byte)ResponseCodes.FRAME_END);
        }
    }
}
