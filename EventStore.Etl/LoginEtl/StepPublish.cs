using System.Collections.Generic;
using EventStore.Common;
using EventStore.ServiceBus;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.LoginEtl
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
                ConsoleSpammer.StepPublishLogin++;

                _Publisher.Publish((Login)row["login"]);
                yield return row;
            }
        }
    }
}
