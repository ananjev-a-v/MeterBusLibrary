using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public sealed class ControlMeterBusPackage : MeterBusPackage
    {
        private readonly ControlMask _control;
        private readonly ControlInformationMask _controlInformation;
        private readonly byte _address;
        private readonly byte _crc;

        public ControlMeterBusPackage(ControlMask control, ControlInformationMask controlInformation, byte address)
        {
            _control = control;
            _controlInformation = controlInformation;
            _address = address;
            var data = new byte[] { (byte)_control, _address, (byte)_controlInformation };
            _crc = CheckSum(data, 0, data.Length);
        }

        internal void Write(Stream stream)
        {
            stream.WriteByte((byte)ResponseCodes.CONTROL_FRAME_START);
            stream.WriteByte((byte)3);
            stream.WriteByte((byte)3);
            stream.WriteByte((byte)ResponseCodes.CONTROL_FRAME_START);
            stream.WriteByte(_crc);
            stream.WriteByte((byte)ResponseCodes.FRAME_END);
        }
    }
}
