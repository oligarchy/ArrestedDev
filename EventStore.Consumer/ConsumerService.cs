using EventStore.Common;
using EventStore.Data;
using EventStore.ServiceBus;
using MassTransit;
using Ninject;
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
                sbc.Subscribe(subs => subs.Consumer<LoginConsumer>(kernel));
                sbc.SetConcurrentConsumerLimit(1);
            }));

            kernel.Bind<HospitalConsumer>().ToSelf();
            kernel.Bind<DataManager<Hospital>>().ToSelf().InSingletonScope();
            kernel.Bind<DataManager<Login>>().ToSelf().InSingletonScope();

            return true;
        }


        public bool Stop(HostControl hostControl)
        {
            return true;
        }
    }
}
