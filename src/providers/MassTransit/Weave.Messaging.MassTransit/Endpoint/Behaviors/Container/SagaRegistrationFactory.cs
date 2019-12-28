using Automatonymous;
using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public sealed class SagaRegistrationFactory
    {
        private class SagaRegistrationBuilder<TSagaInstance> : ISagaRegistrationBuilder<TSagaInstance>
            where TSagaInstance : class, SagaStateMachineInstance
        {
            public IReceiveEndpointConfigurator Configurator { get; private set; }

            public ISagaRegistrationBuilder<TSagaInstance> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
                where TConfigurator : class, IReceiveEndpointConfigurator
            {
                Configurator = configurator;
                return this;
            }

            public void Register(IContainerRegistration registar) => registar.Register(this);
        }

        public static ISagaRegistrationBuilder<TSagaInstance> ForSaga<TSagaInstance>()
            where TSagaInstance : class, SagaStateMachineInstance => new SagaRegistrationBuilder<TSagaInstance>();
    }
}
