using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using EventStore.Common;

using MongoDB.Driver;

using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace EventStore.Data
{
    public class DataManager
    {
        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        private IStoreEvents _store;

        private IMongoDatabase _db; 

        public DataManager()
        {
            _store = Wireup.Init()
               .LogToOutputWindow()
               .UsingInMemoryPersistence()
               .UsingSqlPersistence("EventStoreConnection") // Connection string is in app.config
                   .WithDialect(new MsSqlDialect())
                   .EnlistInAmbientTransaction() // two-phase commit
                   .InitializeStorageEngine()
                   .UsingJsonSerialization()
                       .Compress()
                       .EncryptWith(EncryptionKey)
               .HookIntoPipelineUsing(new[] { new AuthorizationPipelineHook() })
               .UsingSynchronousDispatchScheduler()
                   .DispatchTo(new DelegateMessageDispatcher(DispatchCommit))
               .Build();

            var client = new MongoClient("mongodb://localhost");
            _db = client.GetDatabase("EventStore");
        }

        public void Insert<T>(T obj) where T : AbstractEventObject
        {
            var collection = _db.GetCollection<T>(typeof(T).ToString());
            collection.InsertOneAsync(obj);

            Guid streamId = obj.StreamId;
            using (var stream = _store.CreateStream(streamId))
            {
                stream.Add(new EventMessage { Body = obj });
                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public IEnumerable<T> GetHistory<T>(T obj) where T : AbstractEventObject
        {
            Guid streamId = obj.StreamId;
            using (var stream = _store.OpenStream(streamId, 0))
            {
                foreach (var evt in stream.CommittedEvents)
                {
                    yield return (T)evt.Body;
                }
            }
        }

        public T Get<T>(Expression<Func<T,bool>> query) where T : AbstractEventObject
        {
            var collection = _db.GetCollection<T>(typeof(T).ToString());
            return collection.Find(query).FirstAsync().Result;
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
