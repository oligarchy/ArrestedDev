using System;
using MongoDB.Driver;

namespace EventStore.Common
{
    [Serializable]
    public class Login : AbstractEventObject
    {
        public string UserName { get; set; }
        public DateTime LastLogin { get; set; }

        private static bool indexesUpdated = false;
        private static readonly object IndexLock = new object();

        public override void UpdateIndexes<T>(IMongoIndexManager<T> indexes)
        {
            if (!indexesUpdated)
            {
                lock (IndexLock)
                {
                    if (!indexesUpdated)
                    {
                        var index = indexes.CreateOneAsync(Builders<T>.IndexKeys.Ascending("UserName")).Result;
                        indexesUpdated = true;
                    }
                }
            }
        }
    }
}
