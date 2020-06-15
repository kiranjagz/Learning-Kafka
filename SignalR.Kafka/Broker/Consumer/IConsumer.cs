using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.Kafka.Broker.Consumer
{
    interface IConsumer
    {
        void ConsumerMessage();
    }
}
