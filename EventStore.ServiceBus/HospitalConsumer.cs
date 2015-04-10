using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Common;
using EventStore.Data;
using MassTransit;

namespace EventStore.ServiceBus
{
    public class HospitalConsumer : Consumes<Hospital>.All
    {
        private DataManager<Hospital> DataManager;

        public HospitalConsumer(DataManager<Hospital> dataManager)
        {
            DataManager = dataManager;
        }

        public void Consume(Hospital message)
        {
            ConsoleSpammer.Hospitals++;

            var oldRecord = DataManager.Get(h => h.HospitalId == message.HospitalId);
            if (oldRecord != null)
            {
                message.Id = oldRecord.Id;
                message.StreamId = oldRecord.StreamId;
            }

            DataManager.Insert(message);
        }
    }
}
