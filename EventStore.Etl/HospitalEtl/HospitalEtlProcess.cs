using System.Collections.Generic;
using Rhino.Etl.Core;

namespace EventStore.Etl.HospitalEtl
{
    public class HospitalEtlProcess : EtlProcess
    {
        protected override void Initialize()
        {
            Register(new StepHospitalDataFileImport(
                new List<string>
                {
                    @"../../../Data/Hospitals_2005.csv",
                    @"../../../Data/Hospitals_2006.csv",
                    @"../../../Data/Hospitals_2007.csv",
                    @"../../../Data/Hospitals_2008.csv",
                    @"../../../Data/Hospitals_2009.csv",
                    @"../../../Data/Hospitals_2010.csv",
                    @"../../../Data/Hospitals_2011.csv",
                    @"../../../Data/Hospitals_2012.csv",
                    @"../../../Data/Hospitals_2013.csv",
                    @"../../../Data/Hospitals_2014.csv"
                }
            ));
            Register(new StepMapHospital());
            Register(new StepPublish());
        }
    }
}
