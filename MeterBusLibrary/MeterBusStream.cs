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
        private readonly List<IDisposable> objectsToDispose = new List<IDisposable>();

        private readonly Stream stream;
        public MeterBusStream(Stream stream, bool ownStream)
        {
            this.stream = stream;
            if (ownStream)
                objectsToDispose.Add(stream);
        }

        public MeterBusStream(SettingsSerial settings)
        {
            System.IO.Ports.SerialPort serialPort = new System.IO.Ports.SerialPort(settings.PortName, settings.BaudRate, settings.Parity, settings.DataBits, settings.StopBits);
            objectsToDispose.Add(serialPort);
            serialPort.Open();
            this.stream = serialPort.BaseStream;
        }

        public void Write(byte[] buf)
        {
            System.Diagnostics.Debug.Assert(buf.Length > 0);
            System.Diagnostics.Debug.Assert(buf.Length <= 256);
            switch (buf.Length)
            {
                case 1:
                    Write1(buf);
                    break;
                case 2:
                    WriteShort(buf);
                    break;
                default:
                    WriteLong(buf);
                    break;
            }
        }

        private void WriteLong(byte[] buf)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteByte((byte)ResponseCodes.LONG_FRAME);
                ms.WriteByte((byte)buf.Length);
                ms.WriteByte((byte)buf.Length);
                ms.WriteByte((byte)ResponseCodes.LONG_FRAME);
                ms.Write(buf, 0, buf.Length);
                ms.WriteByte(CheckSum(buf, 0, buf.Length));
                ms.WriteByte((byte)ResponseCodes.FRAME_END);

                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(stream);
            }
        }

        private void WriteShort(byte[] buf)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteByte((byte)ResponseCodes.SHORT_FRAME_START);
                ms.Write(buf, 0, buf.Length);
                byte checkSum = CheckSum(buf, 0, buf.Length);
                ms.WriteByte(checkSum);
                ms.WriteByte((byte)ResponseCodes.FRAME_END);

                ms.Seek(0, SeekOrigin.Begin);
                ms.CopyTo(stream);
            }
        }

        private void Write1(byte[] buf)
        {
            stream.Write(buf, 0, buf.Length);
        }

        private static byte CheckSum(byte[] buf, int offset, int length)
        {
            return (byte)buf.Skip(offset).Take(length).Sum(b => b);
        }

        public byte[] Read()
        {
            byte[] buf = new byte[256];
            int result_length = 0, result_offset;
            int read_result;
            read_result = stream.Read(buf, result_length, 1);
            if (read_result != 1)
                throw new InvalidDataException();
            result_length++;
            switch ((ResponseCodes)buf[result_length - 1])
            {
                case ResponseCodes.ACK:
                    {
                        result_offset = 0;
                    }
                    break;
                case ResponseCodes.SHORT_FRAME_START:
                    {
                        int size = 5;
                        do
                        {
                            read_result = stream.Read(buf, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);
                        if ((ResponseCodes)buf[result_length - 1] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();
                        if (buf[result_length - 2] != CheckSum(buf, 1, 2))
                            throw new InvalidDataException();
                        result_offset = 1;
                        result_length -= result_offset + 2;
                    }
                    break;
                case ResponseCodes.LONG_FRAME:
                    {
                        int size = 4;
                        do
                        {
                            read_result = stream.Read(buf, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);
                        if ((ResponseCodes)buf[result_length - 1] != ResponseCodes.LONG_FRAME)
                            throw new InvalidDataException();
                        if (buf[1] != buf[2])
                            throw new InvalidDataException();
                        size += (int)buf[1] + 2;
                        do
                        {
                            read_result = stream.Read(buf, result_length, size - result_length);
                            result_length += read_result;
                        }
                        while (result_length < size);
                        if ((ResponseCodes)buf[result_length - 1] != ResponseCodes.FRAME_END)
                            throw new InvalidDataException();
                        if (buf[result_length - 2] != CheckSum(buf, 4, result_length - 4 - 2))
                            throw new InvalidDataException();
                        result_offset = 4;
                        result_length -= result_offset + 2;
                    }
                    break;
                default:
                    throw new InvalidDataException();
            }
            //return buf.Skip(result_offset).Take(result_length).ToArray();
            Array.Copy(buf, result_offset, buf, 0, result_length);
            Array.Resize(ref buf, result_length);
            return buf;
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
                foreach (IDisposable dispObj in objectsToDispose.Reverse<IDisposable>())
                    dispObj.Dispose();
                // TODO: set large fields to null.
                objectsToDispose.Clear();

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
