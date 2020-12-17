using System;
using MassTransit;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class MassTransitRabbitMqEndpoint : MassTransitEndpointBase
    {
        public MassTransitRabbitMqEndpoint(IContainerConfigurator containerConfigurator, params IEndpointExtension[] extensions)
            : base(containerConfigurator, extensions)
        {
        }

        protected override Func<IBusControl> GetConfigureFactory(Action<IBusFactoryConfigurator> configurator) =>
            () => Bus.Factory.CreateUsingRabbitMq(configurator);
    }
}
