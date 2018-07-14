using System.Net.Protocols.MeterBus.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Domain
{
    class VIF
    {
        public enum VIFTypes
        {
            PrimaryVIF, // E000 0000b .. E111 1011b
                        // The unit and multiplier is taken from the table for primary VIF (chapter 8.4.3).
            PlainTextVIF, // E111 1100b
                          /// In case of VIF = 7Ch / FCh the true VIF is represented by the following ASCII string with the length given in the first byte. Please note that the byte order of the characters after the length byte depends on the used byte sequence.This plain text VIF allows the user to code units that are not included in the VIF tables.
            LinearVIFExtensionFD, // FDh and FBh
            LinearVIFExtensionFB, // FDh and FBh
                                  // In case of VIF = FDh and VIF = FBh the true VIF is given by the next byte and the coding is taken from the table for secondary VIF (chapter 8.4.4). This extends the available VIF�s by another 256 codes.
            AnyVIF, // 7Eh / FEh
                    // This VIF-Code can be used in direction master to slave for readout selection of all VIF�s.See chapter 6.4.3.
            ManufacturerSpecific, // 7Fh / FFh
        }

        public enum TimePointMagnitudes : byte
        {
            Date = 0,
            TimeDate = 1
        }

        public VIFTypes VIFType { get; }

        public bool Extension { get; }

        public UnitsVariableData Units { get; }

        public int Magnitude { get; }

        public string VIF_string { get; }

        public VIF(BinaryReader source)
        {
            byte b = source.ReadByte();
            if (((b >= 0) && (b <= 0x7b))
                ||
                ((b >= 0x80) && (b <= 0xfb)))
                VIFType = VIFTypes.PrimaryVIF;
            else if ((b & 0x7f) == 0x7c)
                VIFType = VIFTypes.PlainTextVIF;
            else if ((b & 0x7f) == 0x7e)
                VIFType = VIFTypes.AnyVIF;
            else if ((b & 0x7f) == 0x7f)
                VIFType = VIFTypes.ManufacturerSpecific;
            else if (b == 0xfd)
                VIFType = VIFTypes.LinearVIFExtensionFD;
            else if (b == 0xfb)
                VIFType = VIFTypes.LinearVIFExtensionFB;
            else
                throw new InvalidDataException();
            Extension = (b & 0x80) != 0;
            b &= 0x7f; // clear Extension bit
            if ((b >= 0) && (b <= 0x07))
            {
                Units = UnitsVariableData.EnergyWh;
                Magnitude = (b & 0x07) - 3;
            }
            else if ((b >= 0x08) && (b <= 0x0f))
            {
                Units = UnitsVariableData.EnergyJ;
                Magnitude = b & 0x07;
            }
            else if ((b >= 0x10) && (b <= 0x17))
            {
                Units = UnitsVariableData.Volume_m3;
                Magnitude = (b & 0x07) - 6;
            }
            else if ((b >= 0x18) && (b <= 0x1f))
            {
                Units = UnitsVariableData.Mass_kg;
                Magnitude = (b & 0x07) - 3;
            }
            else if ((b >= 0x20) && (b <= 0x23))
            {
                Units = UnitsVariableData.OnTime;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x24) && (b <= 0x27))
            {
                Units = UnitsVariableData.OperatingTime;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x28) && (b <= 0x2f))
            {
                Units = UnitsVariableData.PowerW;
                Magnitude = (b & 0x07) - 3;
            }
            else if ((b >= 0x30) && (b <= 0x37))
            {
                Units = UnitsVariableData.PowerJ_per_h;
                Magnitude = b & 0x07;
            }
            else if ((b >= 0x38) && (b <= 0x3f))
            {
                Units = UnitsVariableData.VolumeFlowM3_per_h;
                Magnitude = (b & 0x07) - 6;
            }
            else if ((b >= 0x40) && (b <= 0x47))
            {
                Units = UnitsVariableData.VolumeFlowExtM3_per_min;
                Magnitude = (b & 0x07) - 7;
            }
            else if ((b >= 0x48) && (b <= 0x4f))
            {
                Units = UnitsVariableData.VolumeFlowExtM3_per_s;
                Magnitude = (b & 0x07) - 9;
            }
            else if ((b >= 0x50) && (b <= 0x57))
            {
                Units = UnitsVariableData.MassFlowKg_per_h;
                Magnitude = (b & 0x07) - 3;
            }
            else if ((b >= 0x58) && (b <= 0x5b))
            {
                Units = UnitsVariableData.FlowTemperatureC;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x5c) && (b <= 0x5f))
            {
                Units = UnitsVariableData.ReturnTemperatureC;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x60) && (b <= 0x63))
            {
                Units = UnitsVariableData.TemperatureDifferenceK;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x64) && (b <= 0x67))
            {
                Units = UnitsVariableData.ExternalTemperatureC;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x68) && (b <= 0x6b))
            {
                Units = UnitsVariableData.PressureBar;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x6c) && (b <= 0x6d))
            {
                Units = UnitsVariableData.TimePoint;
                Magnitude = b & 0x01;
            }
            else if (b == 0x6e)
            {
                Units = UnitsVariableData.UnitsForHCA;
                Magnitude = 0;
            }
            else if (b == 0x6f)
            {
                Units = UnitsVariableData.Reserved;
                Magnitude = 0;
            }
            else if ((b >= 0x70) && (b <= 0x73))
            {
                Units = UnitsVariableData.AveragingDuration;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x74) && (b <= 0x77))
            {
                Units = UnitsVariableData.ActualityDuration;
                Magnitude = b & 0x03;
            }
            else if (b == 0x78)
            {
                Units = UnitsVariableData.FabricationNo;
                Magnitude = 0;
            }
            else if (b == 0x79)
            {
                Units = UnitsVariableData.EnhancedIdentification;
                Magnitude = 0;
            }
            else if (b == 0x7A)
            {
                Units = UnitsVariableData.BusAddress;
                Magnitude = 0;
            }
            else if (b == 0x7B)
            {
                Units = UnitsVariableData.Extension_7B;
                Magnitude = 0;
            }
            else if (b == 0x7C)
            {
                Units = UnitsVariableData.VIF_string;
                Magnitude = 0;
                VIF_string = VariableLengthData.ReadString(source);
            }
            else if (b == 0x7D)
            {
                Units = UnitsVariableData.Extension_7D;
                Magnitude = 0;
            }
            else if (b == 0x7E)
            {
                Units = UnitsVariableData.AnyVIF;
                Magnitude = 0;
            }
            else if (b == 0x7F)
            {
                Units = UnitsVariableData.ManufacturerSpecific;
                Magnitude = 0;
            }
            else
                throw new InvalidDataException();
        }
    }
}
