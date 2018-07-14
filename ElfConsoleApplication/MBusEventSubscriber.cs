using MeterBusLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Bindings;
using System.Text;
using System.Threading.Tasks;

namespace ElfConsoleApplication
{
    public sealed class MBusEventSubscriber : IObserver<PacketStreamEvent>
    {
        public void OnCompleted()
        {
      
        }

        public void OnError(Exception error)
        {
        
        }

        public void OnNext(PacketStreamEvent value)
        {
            Debug.WriteLine($"{value.Package.GetType().Name} received from {value.Binding.ToString()}");
        }
    }
}
