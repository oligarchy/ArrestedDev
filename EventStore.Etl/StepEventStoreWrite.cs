using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.Common;
using EventStore.Data;
using EventStore.ServiceBus;

using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl
{
    public class StepEventStoreWrite : AbstractOperation
    {
        public Publisher _Publisher;

        public StepEventStoreWrite()
        {
            _Publisher = new Publisher();
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                _Publisher.Publish((Hospital)row["hospital"]);
                yield return row;
            }
        }
    }
}
