using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeterBusLibrary.Helpers
{
    public static class BCD
    {
        public static string BCDToString(this IEnumerable<byte> bytes)
        {
            string result = String.Join(String.Empty, bytes.Reverse().Select(b => b.ToString("X2")));
            return result;
        }
    }
}
