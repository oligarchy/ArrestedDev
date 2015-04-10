using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Common;
using EventStore.Common.Messages;
using MassTransit;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql.SqlDialects;

namespace EventStore.Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
        }

        private static void Menu()
        {
            Console.WriteLine("Please select a menu option:");
            Console.WriteLine("1 - Send Message");
            string selection = Console.ReadLine();

            switch (selection)
            {
                case "1": 
                    ComposeMessage();
                    break;
                default :
                    return;
            };
        }

        private static void ComposeMessage()
        {
            var test = new Hospital()
            {
                Address = "Lothrop Street",
                City = "Pittsburgh",
                CountyName = "Alleghenny",
                EmergencyServices = true,
                Name = "UPMC Presby", 
                HospitalId = "PBY",
                Ownership = "All your city are belong to us",
                PhoneNumber = "412-555-1212",
                State = "PA",
                Type = "Hospital",
                ZipCode = "12345"
            };

            List<Hospital> events = new List<Hospital>()
            {
                test
            };

            var publisher = new ServiceBus.Publisher();

            foreach (var evt in events)
            {
                publisher.Publish(evt);
            }
            Menu();
        }
    }
}
