using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using EventStore.Common;
using EventStore.Data;
using EventStore.ServiceBus;

using Magnum;

using MassTransit;
using MassTransit.NinjectIntegration;

using Ninject;
using Ninject.Extensions.NamedScope;

using Topshelf;

namespace EventStore.Consumer
{
    public class ConsumerService : ServiceControl
    {
        public bool Start(HostControl hostControl)
        {
            var kernel = new StandardKernel();

            kernel.Bind<IServiceBus>().ToConstant(ServiceBusFactory.New(sbc =>
            {
                sbc.ReceiveFrom("rabbitmq://localhost/queue");
                sbc.UseRabbitMq();
                sbc.Subscribe(subs => subs.Consumer<HospitalConsumer>(kernel));
            }));

            kernel.Bind<HospitalConsumer>().ToSelf();
            kernel.Bind<DataManager<Hospital>>().ToSelf().InSingletonScope();

            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
