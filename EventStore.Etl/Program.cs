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
            var process = new HospitalEtlProcess();
            var sw = new Stopwatch();
            sw.Start();
            process.Execute();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);
            Console.Read();
        }
    }
}
