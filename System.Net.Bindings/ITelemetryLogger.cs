using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Bindings
{
    public interface ITelemetryLogger
    {
        void Error(string message, string source, Exception ex);

        void Info(string message, string source);

        void Verbose(string message, string source);

        void Warning(string message, string source);
    }
}
