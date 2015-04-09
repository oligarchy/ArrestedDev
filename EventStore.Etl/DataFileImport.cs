using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LumenWorks.Framework.IO.Csv;

using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl
{
    public class DataFileImport : AbstractOperation
    {
        private readonly string _filename;

        public DataFileImport(string filename)
        {
            _filename = filename;
        }
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            var reader = new CsvReader(File.OpenText(_filename), true);
            
            while (reader.ReadNextRecord())
            {
                var row = new Row();
                row["Id"] = reader["Provider ID"];
                yield return row;
            }
        }
    }
}
