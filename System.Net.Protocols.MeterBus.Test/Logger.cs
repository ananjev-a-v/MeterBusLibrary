using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Bindings;
using System.Text;

namespace System.Net.Protocols.MeterBus.Test
{
    public sealed class Logger : ITelemetryLogger
    {
        public void Error(string message, string source, Exception ex)
        {
            Debug.WriteLine(message);
        }

        public void Info(string message, string source)
        {
            Debug.WriteLine(message);
        }

        public void Verbose(string message, string source)
        {
            Debug.WriteLine(message);
        }

        public void Warning(string message, string source)
        {
            Debug.WriteLine(message);
        }
    }
}
