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

namespace EventStore.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MESSAGE CONSUMER START");
            Console.WriteLine("Waiting on incoming messages...");
            Console.WriteLine("---------------------------------------------------------------------");

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("rabbitmq://localhost/queue");
                sbc.UseRabbitMq();
                sbc.SupportBinarySerializer();
                sbc.Subscribe(subs => subs.Consumer<HospitalConsumer>());
            });
        }
    }
}
