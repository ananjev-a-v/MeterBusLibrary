using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System.Net.Protocols.MeterBus
{
    public sealed class LongMeterBusPackage : MeterBusPackage
    {
        private readonly ControlMask _control;
        private readonly ControlInformationMask _controlInformation;
        private readonly byte _address;
        private readonly byte[] _payload;
        private readonly byte _crc;

        public LongMeterBusPackage(ControlMask control, ControlInformationMask controlInformation, byte address, byte[] payload)
        {
            _control = control;
            _controlInformation = controlInformation;
            _address = address;
            _payload = payload;
            var data = AppendTwoByteArrays(new byte[] { (byte)_control, _address, (byte)_controlInformation }, _payload);
            _crc = CheckSum(data, 0, data.Length);
        }

        internal void Write(Stream stream)
        {
            stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
            stream.WriteByte((byte)(_payload.Length + 3));
            stream.WriteByte((byte)(_payload.Length + 3));
            stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
            stream.WriteByte((byte)_control);
            stream.WriteByte(_address);
            stream.WriteByte((byte)_controlInformation);
            stream.Write(_payload, 0, _payload.Length);
            stream.WriteByte(_crc);
            stream.WriteByte((byte)ResponseCodes.FRAME_END);
        }

        static byte[] AppendTwoByteArrays(byte[] arrayA, byte[] arrayB)
        {
            var outputBytes = new byte[arrayA.Length + arrayB.Length];

            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);

            return outputBytes;
        }
    }
}
