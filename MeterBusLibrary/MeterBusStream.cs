using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeterBusLibrary
{
    public class MeterBusStream : IDisposable
    {
        // Disposable objects
        private readonly List<IDisposable> _objectsToDispose = new List<IDisposable>();
        private readonly Stream _stream;

        public MeterBusStream(Stream stream, bool ownStream)
        {
            _stream = stream;

            if (ownStream)
                _objectsToDispose.Add(stream);
        }

        public MeterBusStream(SettingsSerial settings)
        {
            var serialPort = new System.IO.Ports.SerialPort(settings.PortName, settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits);

            _objectsToDispose.Add(serialPort);

            serialPort.Open();

            _stream = serialPort.BaseStream;
        }

        public void Write(byte[] buffer)
        {
            System.Diagnostics.Debug.Assert(buffer.Length > 0);
            System.Diagnostics.Debug.Assert(buffer.Length <= 256);
            
            switch (buffer.Length)
            {
                case 1:
                    WriteByte(buffer);
                    break;
                case 2:
                    WriteShort(buffer);
                    break;
                default:
                    WriteLong(buffer);
                    break;
            }
        }

        private void WriteLong(byte[] buffer)
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
                stream.WriteByte((byte)buffer.Length);
                stream.WriteByte((byte)buffer.Length);
                stream.WriteByte((byte)ResponseCodes.LONG_FRAME);
                stream.Write(buffer, 0, buffer.Length);
                stream.WriteByte(CheckSum(buffer, 0, buffer.Length));
                stream.WriteByte((byte)ResponseCodes.FRAME_END);

                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(_stream);
            }
        }

        private void WriteShort(byte[] buffer)
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte((byte)ResponseCodes.SHORT_FRAME_START);
                stream.Write(buffer, 0, buffer.Length);
                var checkSum = CheckSum(buffer, 0, buffer.Length);
                stream.WriteByte(checkSum);
                stream.WriteByte((byte)ResponseCodes.FRAME_END);

                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(_stream);
            }
        }

        private void WriteByte(byte[] buffer)
        {
            _stream.Write(buffer, 0, buffer.Length);
        }

        private static byte CheckSum(byte[] buf, int offset, int length)
        {
            return (byte)buf.Skip(offset).Take(length).Sum(b => b);
        }

        public byte[] Read()
        {
            var buffer = new byte[256];
            int result_length = 0, result_offset;
            var read_result = _stream.Read(buffer, result_length, 1);

            if (read_result != 1)
                throw new InvalidDataException();

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
                        var size = 5;

                        do
                        {
                            read_result = _stream.Read(buffer, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);

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
                        var size = 4;

                        do
                        {
                            read_result = _stream.Read(buffer, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);

                        if ((ResponseCodes)buffer[result_length - 1] != ResponseCodes.LONG_FRAME)
                            throw new InvalidDataException();

                        if (buffer[1] != buffer[2])
                            throw new InvalidDataException();

                        size += (int)buffer[1] + 2;

                        do
                        {
                            read_result = _stream.Read(buffer, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);

                        if ((ResponseCodes)buffer[result_length - 1] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();

                        if (buffer[result_length - 2] != CheckSum(buffer, 4, result_length - 4 - 2))
                            throw new InvalidDataException();

                        result_offset = 4;
                        result_length -= result_offset + 2;
                    }
                    break;
                default:
                    throw new InvalidDataException();
            }

            //return buf.Skip(result_offset).Take(result_length).ToArray();
            Array.Copy(buffer, result_offset, buffer, 0, result_length);
            Array.Resize(ref buffer, result_length);

            return buffer;
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                foreach (IDisposable dispObj in _objectsToDispose.Reverse<IDisposable>())
                    dispObj.Dispose();
                // TODO: set large fields to null.
                _objectsToDispose.Clear();

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MeterBusStream() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }

        #endregion
    }
}
