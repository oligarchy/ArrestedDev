using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Common;
using EventStore.Common.Messages;
using EventStore.Data;
using EventStore.ServiceBus;

using MassTransit;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

using Topshelf;

namespace EventStore.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.SetServiceName("EventStoreService");
                x.Service(settings => new ConsumerService());
            });

            Console.Read();
        }
    }
}
