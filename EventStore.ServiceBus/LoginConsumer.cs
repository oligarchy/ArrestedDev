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
    public class LoginConsumer : Consumes<Login>.All
    {
        private DataManager<Login> DataManager;

        public LoginConsumer(DataManager<Login> dataManager)
        {
            DataManager = dataManager;
        }

        public void Consume(Login message)
        {
            ConsoleSpammer.Logins++;

            var oldRecord = DataManager.Get(h => h.UserName == message.UserName);
            if (oldRecord != null)
            {
                message.Id = oldRecord.Id;
                message.StreamId = oldRecord.StreamId;
            }

            DataManager.Insert(message);
        }
    }
}
