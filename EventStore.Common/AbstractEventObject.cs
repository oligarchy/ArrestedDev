using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace EventStore.Common
{
    [Serializable]
    public abstract class AbstractEventObject
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public Guid StreamId { get; set; }

        protected AbstractEventObject()
        {
            StreamId = Guid.NewGuid();
            Id = ObjectId.GenerateNewId().ToString();
        }

        private PropertyInfo[] _PropertyInfos = null;

        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = this.GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }

        public abstract void UpdateIndexes<T>(IMongoIndexManager<T> collection);

        public List<Variance> GetDelta<T>(T val2) where T : AbstractEventObject
        {
            var variances = new List<Variance>();
            FieldInfo[] fi = this.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (FieldInfo f in fi)
            {
                Variance v = new Variance
                                 {
                                     Prop = f.Name, 
                                     valA = f.GetValue(this), 
                                     valB = f.GetValue(val2)
                                 };

                if (!v.valA.Equals(v.valB))
                    variances.Add(v);
            }
            return variances;
        }

        public class Variance
        {
            public string Prop { get; set; }
            public object valA { get; set; }
            public object valB { get; set; }
        }
    }
}
