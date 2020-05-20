using System;
using MassTransit;
using MassTransit.ConsumeConfigurators;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public sealed class ConsumerRegistrationFactory
    {
        private sealed class ConsumerRegistrationBuilder<TConsumer> : IConsumerRegistrationBuilder<TConsumer>
            where TConsumer : class, IConsumer
        {
            public IReceiveEndpointConfigurator Configurator { get; private set; }

            public Action<IConsumerConfigurator<TConsumer>> ConsumerConfigurator { get; private set; }

            public IConsumerRegistrationBuilder<TConsumer> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
                where TConfigurator : class, IReceiveEndpointConfigurator
            {
                Configurator = configurator;
                return this;
            }

            public IConsumerRegistrationBuilder<TConsumer> WithConsumerConfiguration(Action<IConsumerConfigurator<TConsumer>> configurator)
            {
                ConsumerConfigurator = configurator;
                return this;
            }

            public void Register(IContainerRegistration registar) => registar.Register(this);
        }

        public static IConsumerRegistrationBuilder<TConsumer> ForConsumer<TConsumer>()
            where TConsumer : class, IConsumer => new ConsumerRegistrationBuilder<TConsumer>();
    }
}
