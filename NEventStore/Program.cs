using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace NEventStore
{
    class Program
    {
        static void Main(string[] args)
        {
            EventStoreTest();
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        private static void EventStoreTest()
        {
            Guid StreamId = Guid.NewGuid();
            //var bus = ServiceBusFactory.New(sbc =>
            //    {

            //    });

            var store =  Wireup.Init()
               .LogToOutputWindow()
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
                    stream.Add(new EventMessage() { Body = "Hello World!" });
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
