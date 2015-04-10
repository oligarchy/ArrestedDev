using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace EventStore.Common
{
    [Serializable]
    public class Hospital : AbstractEventObject
    {
        public string HospitalId { get; set; }

        public string Name { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string CountyName { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        public string Ownership { get; set; }
        public bool EmergencyServices { get; set; }

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
                        var index = indexes.CreateOneAsync(Builders<T>.IndexKeys.Ascending("HospitalId")).Result;
                        index = indexes.CreateOneAsync(Builders<T>.IndexKeys.Ascending("Name")).Result;
                        indexesUpdated = true;
                    }
                }
            }
        }
    }
}
