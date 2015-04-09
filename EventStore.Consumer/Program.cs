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

namespace EventStore.Consumer
{
    class Program
    {
        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        static void Main(string[] args)
        {
            Console.WriteLine("MESSAGE CONSUMER START");
            Console.WriteLine("Waiting on incoming messages...");
            Console.WriteLine("---------------------------------------------------------------------");

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("rabbitmq://localhost/queue");
                sbc.UseRabbitMq();
                sbc.Subscribe(subs =>
                {
                    subs.Handler<TestMessage>(msg => ListenAndPersist(msg));
                });
            });
        }

        private static void ListenAndPersist(TestMessage msg)
        {
            var EventList = new List<TestMessage>();
            
            Guid StreamId = Guid.NewGuid();

            var store = Wireup.Init()
               .UsingInMemoryPersistence()
               .UsingSqlPersistence("EventStoreConnection") // Connection string is in app.config
                   .WithDialect(new MsSqlDialect())
                   .EnlistInAmbientTransaction() // two-phase commit
                   .InitializeStorageEngine()
                   .TrackPerformanceInstance("example")
                   .UsingJsonSerialization()
                       .Compress()
                       .EncryptWith(EncryptionKey)
               .HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
               .UsingSynchronousDispatchScheduler()
                   .DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
               .Build();

            using (store)
            {
                using (var stream = store.CreateStream(StreamId))
                {
                    stream.Add(new EventMessage()
                    {
                        Body = msg
                    });

                    stream.CommitChanges(StreamId);
                }

                using (var stream = store.OpenStream(StreamId, 0, int.MinValue))
                {
                    foreach (var evnt in stream.CommittedEvents)
                    {
                        Console.WriteLine(evnt.Body);
                    }
                }
            }
        }

        private static void DispatchCommit(ICommit commit)
        {
            // This is where we'd hook into our messaging infrastructure, such as NServiceBus,
            // MassTransit, WCF, or some other communications infrastructure.
            // This can be a class as well--just implement IDispatchCommits.
            //try
            //{
            //    foreach (var @event in commit.Events)
            //        Console.WriteLine(Resources.MessagesDispatched + ((SomeDomainEvent)@event.Body).Value);
            //}
            //catch (Exception)
            //{
            //    Console.WriteLine(Resources.UnableToDispatch);
            //}
        }
    }
}
