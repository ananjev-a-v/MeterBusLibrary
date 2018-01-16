using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    class DIF
    {
        private Dictionary<DataTypes, int> LenghtsInBits = new Dictionary<DataTypes, int>()
        {
            { DataTypes._No_data, 0 },
            { DataTypes._8_Bit_Integer, 8 },
            { DataTypes._16_Bit_Integer, 16 },
            { DataTypes._24_Bit_Integer, 24 },
            { DataTypes._32_Bit_Integer, 32 },
            { DataTypes._32_Bit_Real, 32 },
            { DataTypes._48_Bit_Integer, 48 },
            { DataTypes._64_Bit_Integer, 64 },
            { DataTypes._Selection_for_Readout, 0 },
            { DataTypes._2_digit_BCD, 8 },
            { DataTypes._4_digit_BCD, 16 },
            { DataTypes._6_digit_BCD, 24 },
            { DataTypes._8_digit_BCD, 32 },
            { DataTypes._variable_length, 0 /*N*/ },
            { DataTypes._12_digit_BCD, 48 },
            { DataTypes._Special_Functions, 64 },
        };
        public int Length
        {
            get
            {
                switch (DataType)
                {
                    case DataTypes._variable_length:
                        return -1;
                    default:
                        return LenghtsInBits[DataType] / 8;
                }
            }
        }
        public DataTypes DataType { get; }
        public Functions Function { get; }
        public bool StorageLSB { get; }
        public bool Extension { get; }

        public DIF(byte b)
        {
            DataType = (DataTypes)(b & 0x0f);
            Function = (Functions)((b & 0x30) >> 4);
            StorageLSB = (b & 0x40) != 0;
            Extension = (b & 0x80) != 0;
        }
    }
}
