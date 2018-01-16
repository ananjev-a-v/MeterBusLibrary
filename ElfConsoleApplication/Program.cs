using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace ElfConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings settings = new Settings();
            byte[] numbers = new byte[] { 56, 57 };
            List<MeterBusLibrary.Responses.Base> parsed = new List<MeterBusLibrary.Responses.Base>();
            using (var stream = new MeterBusLibrary.MeterBusStream(settings))
            {
                foreach (byte nr in numbers)
                {
                    stream.Write(new byte[] { 0x7B, nr });
                    byte[] buf = stream.Read();
                    parsed.Add(MeterBusLibrary.ResponseMessage.Parse(buf));
                }
            }
            parsed.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
