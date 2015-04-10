using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LumenWorks.Framework.IO.Csv;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.LoginEtl
{
    public class StepLoginDataFileImport : AbstractOperation
    {
        private readonly List<string> _filenames;

        public StepLoginDataFileImport(List<string> filenames)
        {
            _filenames = filenames;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var filename in _filenames)
            {
                ConsoleSpammer.CurrentFile = filename;
                var reader = new CsvReader(File.OpenText(filename), true);

                Dictionary<string, int> ordinals = new Dictionary<string, int>();
                ordinals.Add("username", reader.GetFieldIndex("username"));
                ordinals.Add("LoginDate", reader.GetFieldIndex("LoginDate"));

                while (reader.ReadNextRecord())
                {
                    ConsoleSpammer.StepLoginDataFileImport++;

                    var row = new Row();
                    row["UserName"] = reader[ordinals["username"]];
                    row["LastLogin"] = reader[ordinals["LoginDate"]];

                    yield return row;
                }
            }
        }
    }
}
