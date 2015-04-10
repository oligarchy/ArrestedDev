using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO.Csv;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.HospitalEtl
{
    public class StepHospitalDataFileImport : AbstractOperation
    {
        private readonly List<string> _filenames;

        public StepHospitalDataFileImport(List<string> filenames)
        {
            _filenames = filenames;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            Regex getYear = new Regex("[0-9]+");

            foreach (var filename in _filenames)
            {
                ConsoleSpammer.CurrentFile = filename;
                var reader = new CsvReader(File.OpenText(filename), true);
                int year = Convert.ToInt32(getYear.Match(filename).Value);

                Dictionary<string, int> ordinals = new Dictionary<string, int>();
                ordinals.Add("Provider ID", reader.GetFieldIndex("Provider ID"));
                ordinals.Add("Hospital Name", reader.GetFieldIndex("Hospital Name"));
                ordinals.Add("Address", reader.GetFieldIndex("Address"));
                ordinals.Add("City", reader.GetFieldIndex("City"));
                ordinals.Add("State", reader.GetFieldIndex("State"));
                ordinals.Add("ZIP Code", reader.GetFieldIndex("ZIP Code"));
                ordinals.Add("County Name", reader.GetFieldIndex("County Name"));
                ordinals.Add("Phone Number", reader.GetFieldIndex("Phone Number"));
                ordinals.Add("Hospital Type", reader.GetFieldIndex("Hospital Type"));
                ordinals.Add("Hospital Ownership", reader.GetFieldIndex("Hospital Ownership"));
                ordinals.Add("Emergency Services", reader.GetFieldIndex("Emergency Services"));

                while (reader.ReadNextRecord())
                {
                    ConsoleSpammer.StepHospitalDataFileImport++;

                    var row = new Row();
                    row["HospitalId"] = reader[ordinals["Provider ID"]];
                    row["Name"] = reader[ordinals["Hospital Name"]];
                    row["Address"] = reader[ordinals["Address"]];
                    row["City"] = reader[ordinals["City"]];
                    row["State"] = reader[ordinals["State"]];
                    row["ZipCode"] = reader[ordinals["ZIP Code"]];
                    row["CountyName"] = reader[ordinals["County Name"]];
                    row["PhoneNumber"] = reader[ordinals["Phone Number"]];
                    row["Type"] = reader[ordinals["Hospital Type"]];
                    row["Ownership"] = reader[ordinals["Hospital Ownership"]];
                    row["EmergencyServices"] = reader[ordinals["Emergency Services"]];
                    row["LastImportYear"] = year;

                    yield return row;
                }
            }
        }
    }
}
