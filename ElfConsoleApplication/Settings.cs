using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElfConsoleApplication
{
    public sealed class Settings : MeterBusLibrary.SettingsSerial
    {
        public override string PortName { get { return "COM5"; } }

        public override int BaudRate { get { return 9600; } }
    }
}
