using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MassTransit;

namespace EventStore.ServiceBus
{
    public class Publisher
    {
        private IServiceBus _bus;

        public Publisher()
        {
            _bus = ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("rabbitmq://localhost/return_queue");
                sbc.UseRabbitMq();
                sbc.SupportBinarySerializer();
            });
        }

        public void Publish(object message)
        {
            _bus.Publish(message);
        }

        ~Publisher()
        {
            _bus.Dispose();
        }
    }
}
