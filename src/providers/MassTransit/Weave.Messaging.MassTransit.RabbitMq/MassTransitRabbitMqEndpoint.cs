using System;
using MassTransit;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class MassTransitRabbitMqEndpoint : MassTransitEndpointBase
    {
        public MassTransitRabbitMqEndpoint(params IEndpointExtension[] extensions)
            : base(extensions)
        {
        }

        protected override Func<Action<IBusFactoryConfigurator>, IBusControl> ConfigureFactory =>
            configure => Bus.Factory.CreateUsingRabbitMq(configure);
    }
}
