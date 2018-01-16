using MeterBusLibrary.Domain;
using MeterBusLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Responses
{
    public class FixedData : _UD_Data
    {
        public override UInt32 IdentificationNo { get; }
        public override byte AccessNo { get; }
        public override MediumFixedData Medium { get; }
        public bool CountersFixed { get; }
        public UnitsFixedData Units1 { get; }
        public UnitsFixedData Units2 { get; }
        public UInt32 Counter1 { get; }
        public UInt32 Counter2 { get; }

        public FixedData(_UD_Base UD, BinaryReader source)
                : base(UD)
        {
            {
                byte[] buf = new byte[4];
                if (source.Read(buf, 0, buf.Length) != buf.Length)
                    throw new InvalidDataException();
                IdentificationNo = UInt32.Parse(buf.BCDToString());
            }
            AccessNo = source.ReadByte();
            byte Status = source.ReadByte();
            bool CountersBCD = (Status & 0x01) == 0;
            CountersFixed = (Status & 0x02) != 0;
            byte buf6 = source.ReadByte(); ;
            byte buf7 = source.ReadByte(); ;
            Units1 = (UnitsFixedData)(buf6 & 0x3F);
            Units2 = (UnitsFixedData)(buf7 & 0x3F);
            Medium = (MediumFixedData)(((buf6 & 0xc0) >> 6) | ((buf7 & 0xc0) >> 4));
            if (CountersBCD)
            {
                byte[] buf8 = new byte[4];
                if (source.Read(buf8, 0, buf8.Length) != buf8.Length)
                    throw new InvalidDataException();
                Counter1 = UInt32.Parse(buf8.BCDToString());
                byte[] buf12 = new byte[4];
                if (source.Read(buf12, 0, buf12.Length) != buf12.Length)
                    throw new InvalidDataException();
                Counter2 = UInt32.Parse(buf12.BCDToString());
            }
            else
            {
                Counter1 = source.ReadUInt32();
                Counter2 = source.ReadUInt32();
            }
        }
    }
}
