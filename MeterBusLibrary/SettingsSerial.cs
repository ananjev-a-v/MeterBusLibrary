using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary
{
    public abstract class SettingsSerial : Settings
    {
        public abstract string PortName { get; }

        public virtual int BaudRate { get { return 2400; } }

        public virtual int DataBits { get { return 8; } }

        public virtual System.IO.Ports.Parity Parity { get { return System.IO.Ports.Parity.Even; } }

        public virtual System.IO.Ports.StopBits StopBits { get { return System.IO.Ports.StopBits.One; } }

        public override int GetHashCode()
        {
            return
                PortName.GetHashCode()
                ^
                BaudRate.GetHashCode()
                ^
                DataBits.GetHashCode()
                ^
                Parity.GetHashCode()
                ^
                StopBits.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is SettingsSerial)
            {
                SettingsSerial settings = (SettingsSerial)obj;
                return
                    (PortName == settings.PortName)
                    &&
                    (BaudRate == settings.BaudRate)
                    &&
                    (DataBits == settings.DataBits)
                    &&
                    (Parity == settings.Parity)
                    &&
                    (StopBits == settings.StopBits);
            }
            return base.Equals(obj);
        }
    }
}
