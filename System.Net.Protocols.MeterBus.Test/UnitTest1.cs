using System;
using System.Collections.Generic;
using System.IO.Bindings.Serial;
using System.Net.Bindings.Tcp;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Net.Protocols.MeterBus.Test
{
    [TestClass]
    public sealed class UnitTest1
    {
        [TestMethod]
        public void TestSendSNDNKE()
        {
            var subscriber = new MBusEventSubscriber();

            var endpoint = new TcpEndpointBinding(new IPEndPoint(IPAddress.Parse("192.168.1.135"), 502), new MeterBusPacketSerializer(), new Logger());

            var subscription = endpoint
               .Stream
               .Subscribe(subscriber);

            endpoint.Send(new ShortMeterBusPackage(ControlCommand.SND_NKE, 0x0a));
        }

        [TestMethod]
        public void Test1()
        {
            var subscriber = new MBusEventSubscriber();

            var endpoint = new TcpEndpointBinding(new IPEndPoint(IPAddress.Parse("192.168.1.135"), 502), new MeterBusPacketSerializer(), new Logger());

            var subscription = endpoint
                .Stream
                .Subscribe(subscriber);

            endpoint.Connect();

            endpoint.Send(new ShortMeterBusPackage(ControlCommand.SND_NKE, 0x0a));

            //endpoint.Send(new ShortMeterBusPackage(ControlCommand.SND_UD, 0x0a)); // set address

            //endpoint.Send(new ShortMeterBusPackage(ControlCommand.REQ_UD2, 0x0a));

            //endpoint.Send(new ShortMeterBusPackage(ControlCommand.REQ_UD2, 0x0a));

            //endpoint.Send(new ShortMeterBusPackage(ControlCommand.REQ_UD2, 0x0a));

            //endpoint.Send(new LongMeterBusPackage(ControlMask.SND_NKE, ControlInformationMask.MBUS_CONTROL_INFO_APPLICATION_RESET, 0x0a, new byte[] { }));

            Console.WriteLine("Data sent");

            Console.ReadKey();

            var settings = new Settings();
            var addresses = new byte[] { 0x0a, 0x0b };
            var parsed = new List<System.Net.Protocols.MeterBus.Responses.Base>();

            using (var stream = new SerialPortEndpointBinding(settings))
            {
                foreach (byte address in addresses)
                {
                    //stream.Write(new byte[] { 0x7b, address });

                    //var buffer = stream.Read();

                    //parsed.Add(ResponseMessage.Parse(buffer));
                }
            }

            parsed.ForEach(p => Console.WriteLine(p.ToString()));
        }
    }
}
