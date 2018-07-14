using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Bindings
{
    public interface IEndPointBinding
    {
        IObservable<PacketStreamEvent> Stream { get; }

        Task Send(INetworkPacket packet);

        Task StartReceiving();
    }
}
