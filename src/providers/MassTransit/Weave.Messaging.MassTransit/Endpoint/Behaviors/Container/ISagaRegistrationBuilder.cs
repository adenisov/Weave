using Automatonymous;
using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public interface ISagaRegistrationBuilder<TSagaInstance> : IContainerRegistar
        where TSagaInstance : class, SagaStateMachineInstance
    {
        IReceiveEndpointConfigurator Configurator { get; }

        ISagaRegistrationBuilder<TSagaInstance> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
            where TConfigurator : class, IReceiveEndpointConfigurator;
    }
}
