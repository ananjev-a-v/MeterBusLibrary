using System.Net.Protocols.MeterBus.Domain;
using System.Net.Protocols.MeterBus.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Responses
{
    public sealed class VariableData : _UD_Data
    {
        private static bool _isVerbose = false;

        private class FixedDataHeader
        {
            public UInt32 IdentificationNo { get; }

            public UInt16 Manufr { get; }

            public byte Version { get; }

            public MediumFixedData Medium { get; }

            public byte AccessNo { get; }

            public byte Status { get; }

            public UInt16 Signature { get; }

            public FixedDataHeader(BinaryReader source)
            {
                {
                    var buf = new byte[4];

                    if (source.Read(buf, 0, buf.Length) != buf.Length)
                        throw new InvalidDataException();

                    IdentificationNo = UInt32.Parse(buf.BCDToString());
                }

                Manufr = source.ReadUInt16();
                Version = source.ReadByte();
                Medium = (MediumFixedData)source.ReadByte();
                AccessNo = source.ReadByte();
                Status = source.ReadByte();
                Signature = source.ReadUInt16();
            }
        }

        public sealed class Item
        {
            public DataTypes DataType { get; }

            public Functions Function { get; }

            public UInt64 StorageNumber { get; }

            public UInt32 Tariff { get; }

            public UInt16 SubUnit { get; }

            public object Value { get; }

            public sealed class UnitData
            {
                public UnitsVariableData Unit { get; }

                public int Magnitude { get; }

                public string VIF_string { get; }

                public UnitData(UnitsVariableData unit, int magnitude, string vif = null)
                {
                    this.Unit = unit;
                    this.Magnitude = magnitude;
                    this.VIF_string = vif;
                }

                public override string ToString()
                {
                    if (VIF_string == null)
                        return string.Format("Unit: {0}, Magnitude: {1}", Unit, Magnitude);
                    else
                        return string.Format("Unit: {0} ({2}), Magnitude: {1}", Unit, Magnitude, VIF_string);
                }
            }

            public UnitData[] Units { get; }

            public Item(BinaryReader source)
            {
                int dataLength;
                {
                    // Parse DIF
                    DIF dif;
                    do
                    {
                        dif = new DIF(source.ReadByte());
                    } while ((dif.DataType == DataTypes._Special_Functions) && (dif.Function == Functions.Minimum) && (!dif.StorageLSB) && (!dif.Extension));
                    DataType = dif.DataType;
                    Function = dif.Function;
                    StorageNumber = dif.StorageLSB ? 1UL : 0UL;
                    dataLength = dif.Length;
                    Tariff = 0;
                    SubUnit = 0;
                    int n = 0;
                    bool Extension = dif.Extension;
                    while (Extension)
                    {
                        // Parse DIFE
                        if (n > 10)
                            throw new InvalidDataException();
                        DIFE dife = new DIFE(source.ReadByte());
                        UInt64 snPart = (UInt64)dife.StorageNumber;
                        snPart <<= (n * 4 + 1);
                        StorageNumber |= snPart;
                        UInt32 tPart = (UInt32)dife.Tariff;
                        tPart <<= (n * 2);
                        Tariff |= tPart;
                        UInt16 suPart = (UInt16)dife.Device;
                        suPart <<= n;
                        SubUnit |= suPart;
                        Extension = dife.Extension;
                        n++;
                    }
                }
                {
                    // Parse VIF
                    VIF vif = new VIF(source);
                    List<UnitData> unitsList = new List<UnitData>();
                    if ((vif.VIFType != VIF.VIFTypes.LinearVIFExtensionFD)
                        &&
                        (vif.VIFType != VIF.VIFTypes.LinearVIFExtensionFB))
                        unitsList.Add(new UnitData(vif.Units, vif.Magnitude, vif.VIF_string));
                    bool Extension = vif.Extension;
                    int n = 0;
                    while (Extension)
                    {
                        // Parse VIFE
                        if (n > 10)
                            throw new InvalidDataException();
                        switch (vif.VIFType)
                        {
                            case VIF.VIFTypes.LinearVIFExtensionFB:
                                {
                                    VIFE_FB vife = new VIFE_FB(source.ReadByte());
                                    Extension = vife.Extension;
                                    unitsList.Add(new UnitData(vife.Units, vife.Magnitude));
                                }
                                break;
                            case VIF.VIFTypes.LinearVIFExtensionFD:
                                {
                                    VIFE_FD vife = new VIFE_FD(source.ReadByte());
                                    Extension = vife.Extension;
                                    unitsList.Add(new UnitData(vife.Units, vife.Magnitude));
                                }
                                break;
                            default:
                                {
                                    VIFE vife = new VIFE(source.ReadByte());
                                    Extension = vife.Extension;
                                    unitsList.Add(new UnitData(vife.Units, vife.Magnitude));
                                }
                                break;
                        }
                        n++;
                    }
                    Units = unitsList.ToArray();
                }
                {
                    // Parse Data
                    byte[] buf = (dataLength > 0) ? source.ReadBytes(dataLength) : null;
                    switch (DataType)
                    {
                        case DataTypes._No_data:
                            Value = null;
                            break;
                        case DataTypes._8_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 1);
                            Value = buf.Single();
                            break;
                        case DataTypes._16_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 2);
                            Value = BitConverter.ToInt16(buf, 0);
                            break;
                        case DataTypes._24_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 3);
                            Value = BitConverter.ToInt32(new byte[] { 0 }.Concat(buf).ToArray(), 0);
                            break;
                        case DataTypes._32_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 4);
                            Value = BitConverter.ToInt32(buf, 0);
                            break;
                        case DataTypes._32_Bit_Real:
                            System.Diagnostics.Debug.Assert(buf.Length == 4);
                            Value = BitConverter.ToSingle(buf, 0);
                            break;
                        case DataTypes._48_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 6);
                            Value = BitConverter.ToInt64(new byte[] { 0, 0 }.Concat(buf).ToArray(), 0);
                            break;
                        case DataTypes._64_Bit_Integer:
                            System.Diagnostics.Debug.Assert(buf.Length == 8);
                            Value = BitConverter.ToInt64(buf, 0);
                            break;
                        case DataTypes._Selection_for_Readout:
                            throw new NotImplementedException(); /// !!!
                        case DataTypes._2_digit_BCD:
                            System.Diagnostics.Debug.Assert(buf.Length == 1);
                            Value = sbyte.Parse(buf.BCDToString());
                            break;
                        case DataTypes._4_digit_BCD:
                            System.Diagnostics.Debug.Assert(buf.Length == 2);
                            Value = Int16.Parse(buf.BCDToString());
                            break;
                        case DataTypes._6_digit_BCD:
                            System.Diagnostics.Debug.Assert(buf.Length == 3);
                            Value = Int32.Parse(buf.BCDToString());
                            break;
                        case DataTypes._8_digit_BCD:
                            System.Diagnostics.Debug.Assert(buf.Length == 4);
                            Value = Int32.Parse(buf.BCDToString());
                            break;
                        case DataTypes._variable_length:
                            System.Diagnostics.Debug.Assert(buf == null);
                            Value = VariableLengthData.ReadValue(source);
                            break;
                        case DataTypes._12_digit_BCD:
                            System.Diagnostics.Debug.Assert(buf.Length == 6);
                            Value = Int64.Parse(buf.BCDToString());
                            break;
                        case DataTypes._Special_Functions:
                            //throw new NotImplementedException(); /// !!!
                            break;
                    }
                }
            }

            public Tuple<string, object> NormalizedValue
            {
                get
                {
                    if (DataType == DataTypes._No_data)
                        return null;
                    string name = String.Format("{0}.{1}.Tariff{2}.SubUnit{3}",
                        String.Join(":", Units
                        .Where(u =>
                            (u.Unit != UnitsVariableData.MultiplicativeCorrectionFactor) &&
                            (u.Unit != UnitsVariableData.MultiplicativeCorrectionFactor1000) &&
                            (u.Unit != UnitsVariableData.AdditiveCorrectionConstant) &&
                            true
                        )
                        .Select(u => u.VIF_string ?? u.Unit.ToString())),
                        Function, Tariff, SubUnit);
                    if (Value is string)
                        return new Tuple<string, object>(name, Value);
                    int Magnitude = Units
                        .Where(u =>
                            (u.Unit != UnitsVariableData.AdditiveCorrectionConstant) &&
                            true
                        )
                        .Sum(u => u.Magnitude);
                    int Offset = Units
                        .Where(u => (u.Unit == UnitsVariableData.AdditiveCorrectionConstant))
                        .Sum(u => u.Magnitude);
                    if (Value is Single)
                        return new Tuple<string, object>(name, ((Single)Value) * Math.Pow(10, Magnitude));
                    if (Value is Double)
                        return new Tuple<string, object>(name, ((Double)Value) * Math.Pow(10, Magnitude));
                    Decimal decValue;
                    if (Value is byte)
                        decValue = (Decimal)(byte)Value;
                    else if (Value is sbyte)
                        decValue = (Decimal)(sbyte)Value;
                    else if (Value is Int16)
                        decValue = (Decimal)(Int16)Value;
                    else if (Value is Int32)
                        decValue = (Decimal)(Int32)Value;
                    else if (Value is Int64)
                        decValue = (Decimal)(Int64)Value;
                    else
                        throw new InvalidCastException();
                    switch (Units[0].Unit)
                    {
                        case UnitsVariableData.OnTime:
                        case UnitsVariableData.OperatingTime:
                        case UnitsVariableData.AveragingDuration:
                        case UnitsVariableData.ActualityDuration:
                        case UnitsVariableData.StorageInterval:
                        case UnitsVariableData.DurationSinceLastReadout:
                        case UnitsVariableData.PeriodOfTariff:
                            {
                                TimeSpan span;
                                switch ((TimeMagnitudes)Magnitude)
                                {
                                    case TimeMagnitudes.seconds:
                                        span = new TimeSpan(hours: 0, minutes: 0, seconds: (int)decValue);
                                        break;
                                    case TimeMagnitudes.minutes:
                                        span = new TimeSpan(hours: 0, minutes: (int)decValue, seconds: 0);
                                        break;
                                    case TimeMagnitudes.hours:
                                        span = new TimeSpan(hours: (int)decValue, minutes: 0, seconds: 0);
                                        break;
                                    case TimeMagnitudes.days:
                                        span = new TimeSpan(days: (int)decValue, hours: 0, minutes: 0, seconds: 0);
                                        break;
                                    default:
                                        throw new InvalidDataException();
                                }
                                return new Tuple<string, object>(name, span);
                            }
                        case UnitsVariableData.TimePoint:
                            {
                                DateTime dateTime;
                                switch (Magnitude)
                                {
                                    case 0: //data type G
                                        {
                                            UInt16 temp = (UInt16)decValue;
                                            int day = temp & 0x1f;
                                            int month = (temp >> 8) & 0x0f;
                                            int year = ((temp >> 9) & 0x38) | ((temp >> 5) & 0x07);
                                            if (year < 70)
                                                year += 2000;
                                            else
                                                year += 1900;
                                            dateTime = new DateTime(year, month, day);
                                        }
                                        break;
                                    case 1: //data type F
                                        {
                                            UInt32 temp = (UInt32)decValue;
                                            int second = 0;
                                            int minute = (int)(temp & 0x3f);
                                            int hour = (int)((temp >> 8) & 0x1f);
                                            int day = (int)((temp >> 16) & 0x1f);
                                            int month = (int)((temp >> 24) & 0x0f);
                                            int year = (int)(((temp >> 25) & 0x38) | ((temp >> 21) & 0x07));
                                            if (year < 70)
                                                year += 2000;
                                            else
                                                year += 1900;
                                            bool valid = (temp & 0x80) != 0;
                                            bool summer = (temp & 0x8000) != 0;
                                            dateTime = new DateTime(year, month, day, hour, minute, second);
                                        }
                                        break;
                                    default:
                                        throw new InvalidDataException();
                                }
                                return new Tuple<string, object>(name, dateTime);
                            }
                    }
                    return new Tuple<string, object>(name, decValue * (Decimal)Math.Pow(10, Magnitude) + Offset);
                }
            }

            public override string ToString()
            {
                if (_isVerbose)
                    return String.Format(
                        "NormalizedValue: {7} (" +
                        "DataType: {0}, Function: {1}, StorageNumber: {2}, Tariff: {3}, SubUnit: {4}, " +
                        "Value: {5}, Units: {6})",
                        DataType, Function, StorageNumber, Tariff, SubUnit,
                        Value,
                        String.Join(", ", Units.Select(u => u.ToString())), NormalizedValue
                        );
                else
                    return String.Format("NormalizedValue: {0}", NormalizedValue);
            }
        }

        public List<Item> Items { get; }

        public override UInt32 IdentificationNo { get { return header.IdentificationNo; } }

        public override byte AccessNo { get { return header.AccessNo; } }

        public override MediumFixedData Medium { get { return header.Medium; } }

        private readonly FixedDataHeader header;

        public VariableData(_UD_Base UD, BinaryReader source)
                : base(UD)
        {
            header = new FixedDataHeader(source);
            Items = GetItems(source);
        }

        private List<Item> GetItems(BinaryReader source)
        {
            var result = new List<Item>();

            while (source.BaseStream.Position < source.BaseStream.Length)
            {
                Item item = new Item(source);
                result.Add(item);
            }

            return result;
        }

        public override string ToString()
        {
            return string.Format(
                "IdentificationNo: {0}, AccessNo: {1}, Medium: {2}, " +
                "Items: {3}",
                IdentificationNo, AccessNo, Medium,
                String.Join("\n", Items.Select(i => i.ToString()))
                );
        }
    }
}
