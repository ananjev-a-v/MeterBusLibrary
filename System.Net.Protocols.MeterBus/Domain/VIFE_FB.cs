using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Net.Protocols.MeterBus.Domain
{
    internal sealed class VIFE_FB
    {
        public bool Extension { get; }

        public UnitsVariableData Units { get; }

        public int Magnitude { get; }

        public VIFE_FB(byte b)
        {
            Extension = (b & 0x80) != 0;
            b &= 0x7f; // clear Extension bit
            if ((b >= 0x00) && (b <= 0x01))
            {
                Units = UnitsVariableData.EnergyMWh;
                Magnitude = (b & 0x01) - 1;
            }
            else if ((b >= 0x02) && (b <= 0x03))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_02;
                Magnitude = b & 0x01;
            }
            else if ((b >= 0x04) && (b <= 0x07))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_04;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x08) && (b <= 0x09))
            {
                Units = UnitsVariableData.EnergyGJ;
                Magnitude = (b & 0x01) - 1;
            }
            else if ((b >= 0x0a) && (b <= 0x0b))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_0a;
                Magnitude = b & 0x01;
            }
            else if ((b >= 0x0c) && (b <= 0x0f))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_0c;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x10) && (b <= 0x11))
            {
                Units = UnitsVariableData.Volume_m3;
                Magnitude = (b & 0x01) + 2;
            }
            else if ((b >= 0x12) && (b <= 0x13))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_12;
                Magnitude = b & 0x01;
            }
            else if ((b >= 0x14) && (b <= 0x17))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_14;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x18) && (b <= 0x19))
            {
                Units = UnitsVariableData.Mass_t;
                Magnitude = (b & 0x01) + 2;
            }
            else if ((b >= 0x1a) && (b <= 0x20))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_1a;
                Magnitude = b - 0x1a;
            }
            else if (b == 0x21)
            {
                Units = UnitsVariableData.Volume_feet3;
                Magnitude = -1;
            }
            else if ((b >= 0x22) && (b >= 0x23))
            {
                Units = UnitsVariableData.Volume_american_gallon;
                Magnitude = b - 0x23;
            }
            else if (b == 0x24)
            {
                Units = UnitsVariableData.Volume_flow_american_gallon_per_min;
                Magnitude = -3;
            }
            else if (b == 0x25)
            {
                Units = UnitsVariableData.Volume_flow_american_gallon_per_min;
                Magnitude = 0;
            }
            else if (b == 0x26)
            {
                Units = UnitsVariableData.Volume_flow_american_gallon_per_h;
                Magnitude = 0;
            }
            else if (b == 0x27)
            {
                Units = UnitsVariableData.ReservedVIFE_FB_27;
                Magnitude = 0;
            }
            else if ((b >= 0x28) && (b <= 0x29))
            {
                Units = UnitsVariableData.Power_MW;
                Magnitude = (b & 0x01) - 1;
            }
            else if ((b >= 0x2a) && (b <= 0x2b))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_2a;
                Magnitude = b & 0x01;
            }
            else if ((b >= 0x2c) && (b <= 0x2f))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_2c;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x30) && (b <= 0x31))
            {
                Units = UnitsVariableData.Power_GJ_per_h;
                Magnitude = (b & 0x01) - 1;
            }
            else if ((b >= 0x32) && (b <= 0x57))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_32;
                Magnitude = b - 0x32;
            }
            else if ((b >= 0x58) && (b <= 0x5b))
            {
                Units = UnitsVariableData.FlowTemperature_F;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x5c) && (b <= 0x5f))
            {
                Units = UnitsVariableData.ReturnTemperature_F;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x60) && (b <= 0x63))
            {
                Units = UnitsVariableData.TemperatureDifference_F;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x64) && (b <= 0x67))
            {
                Units = UnitsVariableData.ExternalTemperature_F;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x68) && (b <= 0x6f))
            {
                Units = UnitsVariableData.ReservedVIFE_FB_68;
                Magnitude = b & 0x07;
            }
            else if ((b >= 0x70) && (b <= 0x73))
            {
                Units = UnitsVariableData.ColdWarmTemperatureLimit_F;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x74) && (b <= 0x77))
            {
                Units = UnitsVariableData.ColdWarmTemperatureLimit_C;
                Magnitude = (b & 0x03) - 3;
            }
            else if ((b >= 0x78) && (b <= 0x7f))
            {
                Units = UnitsVariableData.CumulCountMaxPower_W;
                Magnitude = (b & 0x07) - 3;
            }
            else
                throw new InvalidDataException();
        }
    }
}
