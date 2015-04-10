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
        private string _filename;

        public HospitalEtlProcess(string filename)
        {
            _filename = filename;
        }
        protected override void Initialize()
        {
            Register(new StepHospitalDataFileImport(_filename));
            Register(new StepMapHospital());
            Register(new StepPublish());
        }
    }
}
