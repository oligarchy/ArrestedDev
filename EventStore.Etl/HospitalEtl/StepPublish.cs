using System.Collections.Generic;
using EventStore.Common;
using EventStore.ServiceBus;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.HospitalEtl
{
    public class StepPublish : AbstractOperation
    {
        public Publisher _Publisher;

        public StepPublish()
        {
            _Publisher = new Publisher();
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                ConsoleSpammer.StepPublish++;

                _Publisher.Publish((Hospital)row["hospital"]);
                yield return row;
            }
        }
    }
}
