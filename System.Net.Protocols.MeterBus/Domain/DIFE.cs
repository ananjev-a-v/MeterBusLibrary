using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    internal sealed class DIFE
    {
        public int StorageNumber { get; }

        public int Tariff { get; }

        public int Device { get; }

        public bool Extension { get; }

        public DIFE(byte b)
        {
            StorageNumber = b & 0x0f;
            Tariff = (b & 0x30) >> 4;
            Device = (b & 0x40) >> 6;
            Extension = (b & 0x80) != 0;
        }
    }
}
