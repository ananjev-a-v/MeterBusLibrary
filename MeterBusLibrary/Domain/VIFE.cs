using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    class VIFE
    {
        public bool Extension { get; }
        public UnitsVariableData Units { get; }
        public int Magnitude { get; }

        public VIFE(byte b)
        {
            Extension = (b & 0x80) != 0;
            b &= 0x7f; // clear Extension bit
            if ((b >= 0) && (b <= 0x1f))
            {
                Units = UnitsVariableData.ErrorCodesVIFE;
                Magnitude = (b & 0x1f);
            }
            else if (b == 0x20)
                Units = UnitsVariableData.Per_second;
            else if (b == 0x21)
                Units = UnitsVariableData.Per_minute;
            else if (b == 0x22)
                Units = UnitsVariableData.Per_hour;
            else if (b == 0x23)
                Units = UnitsVariableData.Per_day;
            else if (b == 0x24)
                Units = UnitsVariableData.Per_week;
            else if (b == 0x25)
                Units = UnitsVariableData.Per_month;
            else if (b == 0x26)
                Units = UnitsVariableData.Per_year;
            else if (b == 0x27)
                Units = UnitsVariableData.Per_RevolutionMeasurement;
            else if (b == 0x28)
                Units = UnitsVariableData.Increment_per_inputPulseOnInputChannel0;
            else if (b == 0x29)
                Units = UnitsVariableData.Increment_per_inputPulseOnInputChannel1;
            else if (b == 0x2a)
                Units = UnitsVariableData.Increment_per_outputPulseOnOutputChannel0;
            else if (b == 0x2b)
                Units = UnitsVariableData.Increment_per_outputPulseOnOutputChannel1;
            else if (b == 0x2c)
                Units = UnitsVariableData.Per_liter;
            else if (b == 0x2d)
                Units = UnitsVariableData.Per_m3;
            else if (b == 0x2e)
                Units = UnitsVariableData.Per_kg;
            else if (b == 0x2f)
                Units = UnitsVariableData.Per_Kelvin;
            else if (b == 0x30)
                Units = UnitsVariableData.Per_kWh;
            else if (b == 0x31)
                Units = UnitsVariableData.Per_GJ;
            else if (b == 0x32)
                Units = UnitsVariableData.Per_kW;
            else if (b == 0x33)
                Units = UnitsVariableData.Per_KelvinLiter;
            else if (b == 0x34)
                Units = UnitsVariableData.Per_Volt;
            else if (b == 0x35)
                Units = UnitsVariableData.Per_Ampere;
            else if (b == 0x36)
                Units = UnitsVariableData.MultipliedBySek;
            else if (b == 0x37)
                Units = UnitsVariableData.MultipliedBySek_per_V;
            else if (b == 0x38)
                Units = UnitsVariableData.MultipliedBySek_per_A;
            else if (b == 0x39)
                Units = UnitsVariableData.StartDateTimeOf;
            else if (b == 0x3a)
                Units = UnitsVariableData.UncorrectedUnit;
            else if (b == 0x3b)
                Units = UnitsVariableData.AccumulationPositive;
            else if (b == 0x3c)
                Units = UnitsVariableData.AccumulationNegative;
            else if ((b >= 0x3d) && (b <= 0x3f))
            {
                Units = UnitsVariableData.ReservedVIFE_3D;
                Magnitude = (b - 0x3d);
            }
            else if ((b == 0x40) || (b == 0x48))
            {
                Units = UnitsVariableData.LimitValue;
                Magnitude = ((b & 0x08) >> 3);
            }
            else if ((b == 0x41) || (b == 0x49))
            {
                Units = UnitsVariableData.NrOfLimitExceeds;
                Magnitude = ((b & 0x08) >> 3);
            }
            else if ((b & 0x72) == 0x42)
            {
                Units = UnitsVariableData.DateTimeOfLimitExceed;
                Magnitude = (b & 0x0d);
            }
            else if ((b >= 0x50) && (b <= 0x5f))
            {
                Units = UnitsVariableData.DurationOfLimitExceed;
                Magnitude = (b & 0x0f);
            }
            else if ((b >= 0x60) && (b <= 0x67))
            {
                Units = UnitsVariableData.DurationOfLimitAbove;
                Magnitude = (b & 0x07);
            }
            else if ((b & 0x7a) == 0x68)
            {
                Units = UnitsVariableData.ReservedVIFE_68;
                Magnitude = (b - 0x05);
            }
            else if ((b & 0x7a) == 0x6a)
            {
                Units = UnitsVariableData.DateTimeOfLimitAbove;
                Magnitude = (b & 0x05);
            }
            else if ((b >= 0x70) && (b <= 0x77))
            {
                Units = UnitsVariableData.MultiplicativeCorrectionFactor;
                Magnitude = (b & 0x07) - 6;
            }
            else if ((b >= 0x78) && (b <= 0x7b))
            {
                Units = UnitsVariableData.AdditiveCorrectionConstant;
                Magnitude = (b & 0x03) - 3;
            }
            else if (b == 0x7c)
            {
                Units = UnitsVariableData.ReservedVIFE_7C;
            }
            else if (b == 0x7d)
            {
                Units = UnitsVariableData.MultiplicativeCorrectionFactor1000;
                Magnitude = 3;
            }
            else if (b == 0x7e)
            {
                Units = UnitsVariableData.ReservedVIFE_7E;
            }
            else if (b == 0x7f)
            {
                Units = UnitsVariableData.ManufacturerSpecific;
            }
            else
                throw new InvalidDataException();
        }
    }
}
