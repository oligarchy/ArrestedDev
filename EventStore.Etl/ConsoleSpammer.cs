using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace EventStore.Etl
{
    public static class ConsoleSpammer
    {
        public static string CurrentFile { get; set; }
        public static int StepHospitalDataFileImport { get; set; }
        public static int StepMapHospital { get; set; }
        public static int StepPublish { get; set; }
        public static int StepLoginDataFileImport { get; set; }
        public static int StepMapLogin { get; set; }
        public static int StepPublishLogin { get; set; }
    }
}
