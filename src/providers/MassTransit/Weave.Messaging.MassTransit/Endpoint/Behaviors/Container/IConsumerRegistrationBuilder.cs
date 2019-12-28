using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public interface IConsumerRegistrationBuilder<TConsumer> : IContainerRegistar 
        where TConsumer : class, IConsumer
    {
        IReceiveEndpointConfigurator Configurator { get; }

        IConsumerRegistrationBuilder<TConsumer> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;
    }
}
