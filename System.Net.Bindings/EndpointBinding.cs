using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace System.Net.Bindings
{
    public abstract class EndpointBinding
    {
        protected readonly ITelemetryLogger _logger;
        protected readonly IPacketSerializer _serializer;
        protected readonly Subject<PacketStreamEvent> _packageStream = new Subject<PacketStreamEvent>();

        public IObservable<PacketStreamEvent> Stream
        {
            get
            {
                return _packageStream;
            }
        }

        public EndpointBinding(IPacketSerializer serializer, ITelemetryLogger logger)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}
