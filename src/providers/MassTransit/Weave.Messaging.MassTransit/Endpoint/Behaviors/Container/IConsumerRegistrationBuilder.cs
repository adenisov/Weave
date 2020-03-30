using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public interface IConsumerRegistrationBuilder<TConsumer> : IReceiveEndpointConfiguration, IContainerRegistar 
        where TConsumer : class, IConsumer
    {
        IConsumerRegistrationBuilder<TConsumer> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;
    }
}
