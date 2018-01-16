using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    class VIFE_FD
    {
        public bool Extension { get; }
        public UnitsVariableData Units { get; }
        public int Magnitude { get; }

        public VIFE_FD(byte b)
        {
            Extension = (b & 0x80) != 0;
            b &= 0x7f; // clear Extension bit
            if ((b >= 0x00) && (b <= 0x03))
            {
                Units = UnitsVariableData.Credit;
                Magnitude = (b & 0x03) - 3;
            }
            else if((b >= 0x04) && (b <= 0x07))
            {
                Units = UnitsVariableData.Debit;
                Magnitude = (b & 0x03) - 3;
            }
            else if (b == 0x08)
            {
                Units = UnitsVariableData.AccessNumber;
                Magnitude = 0;
            }
            else if (b == 0x09)
            {
                Units = UnitsVariableData.Medium;
                Magnitude = 0;
            }
            else if (b == 0x0a)
            {
                Units = UnitsVariableData.Manufacturer;
                Magnitude = 0;
            }
            else if (b == 0x0b)
            {
                Units = UnitsVariableData.EnhancedIdentification;
                Magnitude = 0;
            }
            else if (b == 0x0c)
            {
                Units = UnitsVariableData.ModelVersion;
                Magnitude = 0;
            }
            else if (b == 0x0d)
            {
                Units = UnitsVariableData.HardwareVersionNr;
                Magnitude = 0;
            }
            else if (b == 0x0e)
            {
                Units = UnitsVariableData.FirmwareVersionNr;
                Magnitude = 0;
            }
            else if (b == 0x0f)
            {
                Units = UnitsVariableData.SoftwareVersionNr;
                Magnitude = 0;
            }
            else if (b == 0x10)
            {
                Units = UnitsVariableData.CustomerLocation;
                Magnitude = 0;
            }
            else if (b == 0x11)
            {
                Units = UnitsVariableData.Customer;
                Magnitude = 0;
            }
            else if (b == 0x12)
            {
                Units = UnitsVariableData.AccessCodeUser;
                Magnitude = 0;
            }
            else if (b == 0x13)
            {
                Units = UnitsVariableData.AccessCodeOperator;
                Magnitude = 0;
            }
            else if (b == 0x14)
            {
                Units = UnitsVariableData.AccessCodeSystemOperator;
                Magnitude = 0;
            }
            else if (b == 0x15)
            {
                Units = UnitsVariableData.AccessCodeDeveloper;
                Magnitude = 0;
            }
            else if (b == 0x16)
            {
                Units = UnitsVariableData.Password;
                Magnitude = 0;
            }
            else if (b == 0x17)
            {
                Units = UnitsVariableData.ErrorFlags;
                Magnitude = 0;
            }
            else if (b == 0x18)
            {
                Units = UnitsVariableData.ErrorMask;
                Magnitude = 0;
            }
            else if (b == 0x19)
            {
                Units = UnitsVariableData.ReservedVIFE_FD_19;
                Magnitude = 0;
            }
            else if (b == 0x1a)
            {
                Units = UnitsVariableData.DigitalOutput;
                Magnitude = 0;
            }
            else if (b == 0x1b)
            {
                Units = UnitsVariableData.DigitalInput;
                Magnitude = 0;
            }
            else if (b == 0x1c)
            {
                Units = UnitsVariableData.Baudrate;
                Magnitude = 0;
            }
            else if (b == 0x1d)
            {
                Units = UnitsVariableData.ResponseDelayTime;
                Magnitude = 0;
            }
            else if (b == 0x1e)
            {
                Units = UnitsVariableData.Retry;
                Magnitude = 0;
            }
            else if (b == 0x1f)
            {
                Units = UnitsVariableData.ReservedVIFE_FD_1f;
                Magnitude = 0;
            }
            else if (b == 0x20)
            {
                Units = UnitsVariableData.FirstStorageNr;
                Magnitude = 0;
            }
            else if (b == 0x21)
            {
                Units = UnitsVariableData.LastStorageNr;
                Magnitude = 0;
            }
            else if (b == 0x22)
            {
                Units = UnitsVariableData.SizeOfStorage;
                Magnitude = 0;
            }
            else if (b == 0x23)
            {
                Units = UnitsVariableData.ReservedVIFE_FD_23;
                Magnitude = 0;
            }
            else if ((b >= 0x24) && (b <= 0x27))
            {
                Units = UnitsVariableData.StorageInterval;
                Magnitude = b & 0x03;
            }
            else if (b == 0x28)
            {
                Units = UnitsVariableData.StorageIntervalMmnth;
                Magnitude = 0;
            }
            else if (b == 0x29)
            {
                Units = UnitsVariableData.StorageIntervalYear;
                Magnitude = 0;
            }
            else if (b == 0x2a)
            {
                Units = UnitsVariableData.ReservedVIFE_FD_2a;
                Magnitude = 0;
            }
            else if (b == 0x2b)
            {
                Units = UnitsVariableData.ReservedVIFE_FD_2b;
                Magnitude = 0;
            }
            else if ((b >= 0x2c) && (b <= 0x2f))
            {
                Units = UnitsVariableData.DurationSinceLastReadout;
                Magnitude = b & 0x03;
            }
            else if (b == 0x30)
            {
                Units = UnitsVariableData.StartDateTimeOfTariff;
                Magnitude = 0;
            }
            else if ((b >= 0x31) && (b <= 0x33))
            {
                Units = UnitsVariableData.DurationOfTariff;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x34) && (b <= 0x37))
            {
                Units = UnitsVariableData.PeriodOfTariff;
                Magnitude = b & 0x03;
            }
            else if (b == 0x38)
            {
                Units = UnitsVariableData.PeriodOfTariffMonths;
                Magnitude = 0;
            }
            else if (b == 0x39)
            {
                Units = UnitsVariableData.PeriodOfTariffYear;
                Magnitude = 0;
            }
            else if (b == 0x3a)
            {
                Units = UnitsVariableData.Dimensionless;
                Magnitude = 0;
            }
            else if (b == 0x3b)
            {
                Units = UnitsVariableData.Reserved_FD_3b;
                Magnitude = 0;
            }
            else if ((b >= 0x3c) && (b <= 0x3F))
            {
                Units = UnitsVariableData.Reserved_FD_3c;
                Magnitude = 0;
            }
            else if ((b >= 0x40) && (b <= 0x4F))
            {
                Units = UnitsVariableData.Volts;
                Magnitude = (b & 0x0f) - 9;
            }
            else if ((b >= 0x50) && (b <= 0x5F))
            {
                Units = UnitsVariableData.Ampers;
                Magnitude = (b & 0x0f) - 12;
            }
            else if (b == 0x60)
            {
                Units = UnitsVariableData.ResetCounter;
                Magnitude = 0;
            }
            else if (b == 0x61)
            {
                Units = UnitsVariableData.CumulationCounter;
                Magnitude = 0;
            }
            else if (b == 0x62)
            {
                Units = UnitsVariableData.ControlSignal;
                Magnitude = 0;
            }
            else if (b == 0x63)
            {
                Units = UnitsVariableData.DayOfWeek;
                Magnitude = 0;
            }
            else if (b == 0x64)
            {
                Units = UnitsVariableData.WeekNumber;
                Magnitude = 0;
            }
            else if (b == 0x65)
            {
                Units = UnitsVariableData.TimePointOfDayChange;
                Magnitude = 0;
            }
            else if (b == 0x66)
            {
                Units = UnitsVariableData.StateOfParameterActivation;
                Magnitude = 0;
            }
            else if (b == 0x67)
            {
                Units = UnitsVariableData.SpecialSupplierInformation;
                Magnitude = 0;
            }
            else if ((b >= 0x68) && (b <= 0x6b))
            {
                Units = UnitsVariableData.DurationSinceLastCumulation;
                Magnitude = b & 0x03;
            }
            else if ((b >= 0x6c) && (b <= 0x6f))
            {
                Units = UnitsVariableData.OperatingTimeBattery;
                Magnitude = b & 0x03;
            }
            else if (b == 0x70)
            {
                Units = UnitsVariableData.DateTimeOfBatteryChange;
                Magnitude = 0;
            }
            else if ((b >= 0x71) && (b <= 0x7f))
            {
                Units = UnitsVariableData.Reserved_FD_71;
                Magnitude = b - 0x71;
            }
            else
                throw new InvalidDataException();
        }
    }
}
