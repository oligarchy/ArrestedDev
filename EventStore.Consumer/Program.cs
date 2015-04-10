using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventStore.Common;
using EventStore.Common.Messages;
using EventStore.Data;
using EventStore.ServiceBus;

using MassTransit;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

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
                                        + "Hospitals: {1}\r\n",
                                        sw.ElapsedMilliseconds,
                                        ConsoleSpammer.Hospitals);
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
