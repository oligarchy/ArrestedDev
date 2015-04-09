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
    public class StepConsoleWrite : AbstractOperation
    {
        private DataManager _Manager;

        public StepConsoleWrite()
        {
            _Manager = new DataManager();
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                var hospital = _Manager.Get<Hospital>(h => h.Name == ((Hospital)row["hospital"]).Name);
                var history = _Manager.GetHistory(hospital);

                var h1 = history.First();
                var h2 = history.Last();

                foreach (var diff in h1.GetDelta(h2))
                {
                    Console.WriteLine("{0}: {1} => {2}", diff.Prop, diff.valA, diff.valB);
                }
                
                yield return row;
            }
        }
    }
}
