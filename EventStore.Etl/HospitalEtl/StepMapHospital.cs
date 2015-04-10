using System.Collections.Generic;
using EventStore.Common;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.HospitalEtl
{
    public class StepMapHospital : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                ConsoleSpammer.StepMapHospital++;

                var hospital = new Hospital();
                var rtn = new Row();

                hospital.HospitalId = row["HospitalId"].ToString();
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
                hospital.LastImportYear = (int)row["LastImportYear"];

                rtn["hospital"] = hospital;
                yield return rtn;
            }
        }
    }
}
