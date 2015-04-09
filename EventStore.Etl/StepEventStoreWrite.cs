using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.Common;
using EventStore.Data;

using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl
{
    public class StepEventStoreWrite : AbstractOperation
    {
        public DataManager _Manager;

        public StepEventStoreWrite()
        {
            _Manager = new DataManager();
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                _Manager.Insert((Hospital)row["hospital"]);
                yield return row;
            }
        }
    }
}
