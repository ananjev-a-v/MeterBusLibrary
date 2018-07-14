using MeterBusLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Bindings.Udp;
using System.Net.Protocols.MeterBus;
using System.Text;

namespace ElfConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var subscriber = new MBusEventSubscriber();

            var endpoint = new UdpEndpointBinding(new IPEndPoint(IPAddress.Parse("192.168.1.135"), 502), new MeterBusPacketSerializer(), new Logger());

            endpoint
                .Stream
                .Subscribe(subscriber);

            endpoint.Send(new ShortMeterBusPackage(ControlMask.SND_NKE, 0x0a));

            endpoint.Send(new LongMeterBusPackage(ControlMask.SND_NKE, ControlInformationMask.MBUS_CONTROL_INFO_APPLICATION_RESET, 0x0a, new byte[] { }));

            Console.WriteLine("Data sent");

            Console.ReadKey();

            var settings = new Settings();
            var numbers = new byte[] { 56, 57 };
            var parsed = new List<MeterBusLibrary.Responses.Base>();

            using (var stream = new MeterBusStream(settings))
            {
                foreach (byte nr in numbers)
                {
                    stream.Write(new byte[] { 0x7B, nr });

                    var buf = stream.Read();

                    parsed.Add(ResponseMessage.Parse(buf));
                }
            }

            parsed.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
