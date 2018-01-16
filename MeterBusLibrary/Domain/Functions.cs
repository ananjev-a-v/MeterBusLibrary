using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Domain
{
    public enum Functions : byte
    {
        Instantaneous = 0, //Instantaneous value
        Maximum = 1, //Maximum value
        Minimum = 2, //Minimum value
        ValueDuringError = 3, //Value during error state
    }
}
