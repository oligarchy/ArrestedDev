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
    public class StepEventStoreWrite2 : AbstractOperation
    {
        public DataManager _Manager;

        public StepEventStoreWrite2()
        {
            _Manager = new DataManager();
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                var hospital = (Hospital)row["hospital"];
                hospital.Name += "- ALTERED";
                _Manager.Insert(hospital);
                yield return row;
            }
        }
    }
}
