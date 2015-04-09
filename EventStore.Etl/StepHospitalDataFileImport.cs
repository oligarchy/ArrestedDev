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
    public class StepHospitalDataFileImport : AbstractOperation
    {
        private readonly string _filename;

        public StepHospitalDataFileImport(string filename)
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
                row["Name"] = reader["Hospital Name"];
                row["Address"] = reader["Address"];
                row["City"] = reader["City"];
                row["State"] = reader["State"];
                row["ZipCode"] = reader["ZIP Code"];
                row["CountyName"] = reader["County Name"];
                row["PhoneNumber"] = reader["Phone Number"];
                row["Type"] = reader["Hospital Type"];
                row["Ownership"] = reader["Hospital Ownership"];
                row["EmergencyServices"] = reader["Emergency Services"];

                yield return row;
            }
        }
    }
}
