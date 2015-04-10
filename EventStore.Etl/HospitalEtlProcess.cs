using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommonDomain.Persistence.EventStore;

using Rhino.Etl.Core;

namespace EventStore.Etl
{
    public class HospitalEtlProcess : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new StepHospitalDataFileImport(@"../../../Data/Hospitals_2014.csv"));
            Register(new StepMapHospital());
            Register(new StepEventStoreWrite());
        }
    }
}
