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
    public class DataManager<T> where T : AbstractEventObject
    {
        private static readonly byte[] EncryptionKey = new byte[]
        {
            0x0, 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xa, 0xb, 0xc, 0xd, 0xe, 0xf
        };

        private IStoreEvents _store;

        private IMongoDatabase _db;

        private IMongoCollection<T> _collection;

        public DataManager()
        {
            _store = Wireup.Init()
               .UsingSqlPersistence("EventStoreConnection") // Connection string is in app.config
                   .WithDialect(new MsSqlDialect())
                   .EnlistInAmbientTransaction() // two-phase commit
                   .InitializeStorageEngine()
                   .UsingBinarySerialization()
                       .Compress()
                       .EncryptWith(EncryptionKey)
               .HookIntoPipelineUsing(new IPipelineHook[] { new AuthorizationPipelineHook() })
               .Build();

            var client = new MongoClient("mongodb://localhost");
            _db = client.GetDatabase("EventStore");
            _collection = _db.GetCollection<T>(typeof(T).ToString());
        }

        public void Insert(T obj)
        {
            obj.UpdateIndexes(_collection.Indexes);
            var task = _collection.ReplaceOneAsync(x => x.Id == obj.Id, obj, new UpdateOptions { IsUpsert = true });
            task.Wait();

            Guid streamId = obj.StreamId;
            using (var stream = _store.OpenStream(streamId, 0))
            {
                stream.Add(new EventMessage { Body = obj });
                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public IEnumerable<T> GetHistory(T obj)
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

        public T Get(Expression<Func<T,bool>> query)
        {
            return _collection.Find(query).FirstOrDefaultAsync().Result;
        }
    }
}
