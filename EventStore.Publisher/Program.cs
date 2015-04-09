using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Common;
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
            
        }

        private static void DispatchEvents<T>(List<T> EventList) where T : new()
        {
            Guid StreamId = Guid.NewGuid();

            var bus = ServiceBusFactory.New(sbc =>
            {
                sbc.UseRabbitMq(r =>
                {
                    r.ConfigureHost(new Uri("rabbitmq://localhost/vhost/queue"), h =>
                    {
                        h.SetUsername("username");
                        h.SetPassword("password");
                    });
                });

            });

            var store = Wireup.Init()
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
                    foreach (var evnt in EventList)
                    {
                        stream.Add(new EventMessage() 
                        {
                            Body = evnt
                        });
                    }

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
