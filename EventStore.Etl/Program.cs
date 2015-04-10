using System;
using System.Diagnostics;
using System.Threading;

namespace EventStore.Etl
{
    class Program
    {
        private static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            ConsoleSpammer.CurrentFile = "";



            const int threadCount = 3;
            using (var countdownEvent = new CountdownEvent(threadCount))
            {
                ThreadPool.QueueUserWorkItem(
                    delegate
                        {
                            while (true)
                            {
                                Thread.Sleep(1000);
                                Console.Clear();
                                Console.WriteLine(
                                    "Publishing Messages\r\n" + "Time Spent: {0}ms\r\n" 
                                    + "File: {1}\r\n"
                                    + "Hospital Data File Import Progress: {2}\r\n"
                                    + "Hospital Mapping Progress: {3}\r\n" 
                                    + "Hospital Message Queue Progress: {4}\r\n"
                                    + "Login Data File Import Progress: {5}\r\n" 
                                    + "Login Map Progress: {6}\r\n"
                                    + "Login Message Queue Progress: {7}\r\n",
                                    sw.ElapsedMilliseconds,
                                    ConsoleSpammer.CurrentFile.Replace("..", "").Replace("/", ""),
                                    ConsoleSpammer.StepHospitalDataFileImport,
                                    ConsoleSpammer.StepMapHospital,
                                    ConsoleSpammer.StepPublish,
                                    ConsoleSpammer.StepLoginDataFileImport,
                                    ConsoleSpammer.StepMapLogin,
                                    ConsoleSpammer.StepPublishLogin);
                            }
                        });

                ThreadPool.QueueUserWorkItem(
                    delegate
                        {
                            var process1 = new LoginEtl.LoginEtlProcess();
                            process1.Execute();
                        });

                ThreadPool.QueueUserWorkItem(
                    delegate
                        {
                            var process2 = new HospitalEtl.HospitalEtlProcess();
                            process2.Execute();
                        });

                countdownEvent.Wait();
            }
            sw.Stop();
        }
    }
}
