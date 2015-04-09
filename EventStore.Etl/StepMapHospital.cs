using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EventStore.Common;

using LumenWorks.Framework.IO.Csv;

using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl
{
    public class StepMapHospital : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                var hospital = new Hospital();
                var rtn = new Row();

                hospital.Id = row["Id"].ToString();
                hospital.Name = row["Name"].ToString();
                hospital.Address = row["Address"].ToString();
                hospital.City = row["City"].ToString();
                hospital.State = row["State"].ToString();
                hospital.ZipCode = row["ZipCode"].ToString();
                hospital.CountyName = row["CountyName"].ToString();
                hospital.PhoneNumber = row["PhoneNumber"].ToString();
                hospital.Type = row["Type"].ToString();
                hospital.Ownership = row["Ownership"].ToString();
                hospital.EmergencyServices = row["EmergencyServices"].ToString() == "Yes";

                rtn["hospital"] = hospital;
                yield return rtn;
            }
        }
    }
}
