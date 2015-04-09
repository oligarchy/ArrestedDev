using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl
{
    public class StepConsoleWrite : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                foreach (var column in row.Columns.OrderBy(s => s))
                {
                    Console.WriteLine(column + ": " + row[column]);
                }
                Console.WriteLine();
                yield return row;
            }
        }
    }
}
