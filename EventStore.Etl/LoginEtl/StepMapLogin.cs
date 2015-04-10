using System;
using System.Collections.Generic;
using EventStore.Common;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;

namespace EventStore.Etl.LoginEtl
{
    public class StepMapLogin : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                ConsoleSpammer.StepMapLogin++;

                var login = new Login();
                var rtn = new Row();

                login.UserName = row["UserName"].ToString();
                login.LastLogin = Convert.ToDateTime(row["LastLogin"]);

                rtn["login"] = login;
                yield return rtn;
            }
        }
    }
}
