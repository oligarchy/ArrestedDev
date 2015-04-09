using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Common;
using EventStore.Common.Messages;
using MassTransit;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace EventStore.Publisher
{
    class Program
    {
        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        static void Main(string[] args)
        {
            TestMessage test = new TestMessage()
            {
                MessageText = "hello there"
            };

            List<TestMessage> events = new List<TestMessage>()
            {
                test
            };

            DispatchEvents<TestMessage>(events);
        }

        private static void DispatchEvents<T>(List<T> EventList) where T : class
        {
            Guid StreamId = Guid.NewGuid();

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("rabbitmq://localhost/queue");
                sbc.UseRabbitMq();
            });

            foreach (var msg in EventList)
            {
                bus.Publish(msg);
            }
        }
    }
}
