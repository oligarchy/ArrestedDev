using System.Collections.Generic;
using Rhino.Etl.Core;

namespace EventStore.Etl.LoginEtl
{
    public class LoginEtlProcess : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new StepLoginDataFileImport(
                new List<string>
                {
                    @"../../../Data/Logins.csv"
                }
            ));
            Register(new StepMapLogin());
            Register(new StepPublish());
        }
    }
}
