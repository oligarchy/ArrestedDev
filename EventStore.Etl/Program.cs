using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventStore.Etl
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> filenames = new List<string>
            {
                @"../../../Data/Hospitals_2012.csv",
                @"../../../Data/Hospitals_2013.csv",
                @"../../../Data/Hospitals_2014.csv"
            };

            foreach (var filename in filenames)
            {
                var process = new HospitalEtlProcess(filename);
                process.Execute();
            }  
        }
    }
}
