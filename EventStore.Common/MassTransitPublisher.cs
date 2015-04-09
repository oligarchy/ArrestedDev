using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using NEventStore;
using NEventStore.Dispatcher;

namespace EventStore.Common
{
    public class MassTransitPublisher : IDispatchCommits
    {
        private readonly IServiceBus _Bus;

        public MassTransitPublisher(IServiceBus bus)
        {
            _Bus = bus;
        }

        void PublishEvent<T>(T message)
            where T : class
        {
            _Bus.Publish(message);
        }

        public void Dispose()
        {
            _Bus.Dispose();
        }

        public void Dispatch(ICommit commit)
        {
            // #TODO
            // nothing yet
        }
    }
}
