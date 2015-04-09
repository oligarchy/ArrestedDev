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
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine(sw.ElapsedMilliseconds);
                    }
                }
            );
            process.Execute();
            sw.Stop();
            Console.Read();
        }
    }
}
