using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Bindings
{
    public sealed class PacketStreamEvent : EventArgs
    {
        public Guid CorrelationId { get; private set; } = Guid.NewGuid();

        public INetworkPacket Package { get; private set; }

        public IEndPointBinding Binding { get; private set; }

        public PacketStreamEvent(INetworkPacket packet, IEndPointBinding binding = null)
        {
            this.Package = packet;
            this.Binding = binding;
        }
    }
}
