using System;
using Automatonymous;
using MassTransit;
using ISaga = MassTransit.Saga.ISaga;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    public sealed class SagaRegistrationFactory
    {
        private class StateMachineSagaRegistrationBuilder<TSagaInstance> : IStateMachineSagaRegistrationBuilder<TSagaInstance>
            where TSagaInstance : class, SagaStateMachineInstance
        {
            public IReceiveEndpointConfigurator Configurator { get; private set; }

            public Action<ISagaConfigurator<TSagaInstance>> SagaConfigurator { get; private set; }

            public IStateMachineSagaRegistrationBuilder<TSagaInstance> WithReceiveEndpointConfiguration<TConfigurator>(
                TConfigurator configurator)
                where TConfigurator : class, IReceiveEndpointConfigurator
            {
                Configurator = configurator;
                return this;
            }

            public IStateMachineSagaRegistrationBuilder<TSagaInstance> WithSagaConfigurator(
                Action<ISagaConfigurator<TSagaInstance>> configurator)
            {
                SagaConfigurator = configurator;
                return this;
            }

            public void Register(IContainerRegistration registar) => registar.Register(this);
        }

        private class SagaRegistrationBuilder<TSaga> : ISagaRegistrationBuilder<TSaga>
            where TSaga : class, ISaga
        {
            public IReceiveEndpointConfigurator Configurator { get; private set; }

            public ISagaRegistrationBuilder<TSaga> WithReceiveEndpointConfiguration<TConfigurator>(TConfigurator configurator)
                where TConfigurator : class, IReceiveEndpointConfigurator
            {
                Configurator = configurator;
                return this;
            }

            public void Register(IContainerRegistration registar) => registar.Register(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSagaInstance"></typeparam>
        /// <returns></returns>
        public static IStateMachineSagaRegistrationBuilder<TSagaInstance> ForStateMachineSaga<TSagaInstance>()
            where TSagaInstance : class, SagaStateMachineInstance => new StateMachineSagaRegistrationBuilder<TSagaInstance>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSaga"></typeparam>
        /// <returns></returns>
        public static ISagaRegistrationBuilder<TSaga> ForSaga<TSaga>()
            where TSaga : class, ISaga => new SagaRegistrationBuilder<TSaga>();
    }
}
