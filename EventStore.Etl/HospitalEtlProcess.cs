using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino.Etl.Core;

namespace EventStore.Etl
{
    public class HospitalEtlProcess : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new DataFileImport(@"../../../Data/Hospitals_2014.csv"));
            Register(new ConsoleWrite());
        }
    }
}
