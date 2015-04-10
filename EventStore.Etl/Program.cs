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
            var sw = new Stopwatch();
            sw.Start();

            ConsoleSpammer.CurrentFile = "";

            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine("Publishing Messages\r\n"
                                        + "Time Spent: {0}ms\r\n" 
                                        + "File: {1}\r\n"
                                        + "Data File Import Progress: {2}\r\n" 
                                        + "Map Hospital Progress: {3}\r\n"
                                        + "Message Queue Progress: {4}\r\n",
                                        sw.ElapsedMilliseconds,
                                        ConsoleSpammer.CurrentFile.Replace("..","").Replace("/",""),
                                        ConsoleSpammer.StepHospitalDataFileImport,
                                        ConsoleSpammer.StepMapHospital,
                                        ConsoleSpammer.StepPublish);
                    }
                }
            );

            
            var process = new HospitalEtlProcess();
            process.Execute();

            sw.Stop();
        }
    }
}
