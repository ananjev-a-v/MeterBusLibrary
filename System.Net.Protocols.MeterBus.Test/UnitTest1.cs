using System;
using System.Net.Bindings.Tcp;
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
    }
}
