using System;
using System.Diagnostics;
using System.Threading;
using EventStore.ServiceBus;
using Topshelf;

namespace EventStore.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            ThreadPool.QueueUserWorkItem(
                delegate
                {
                    while (true)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine("Consuming Messages\r\n"
                                        + "Time Spent: {0}ms\r\n"
                                        + "Hospitals: {1}\r\n"
                                        + "Logins: {2}\r\n",
                                        sw.ElapsedMilliseconds,
                                        ConsoleSpammer.Hospitals,
                                        ConsoleSpammer.Logins);
                    }
                }
            );


            HostFactory.Run(x =>
            {
                x.SetServiceName("EventStoreService");
                x.Service(settings => new ConsumerService());
            });

            sw.Stop();
        }
    }
}
