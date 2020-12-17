using System;
using System.Collections.Generic;
using System.Reflection;
using Automatonymous;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Sagas;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.RabbitMq.Extensions.Sagas
{
    internal sealed class RegisterSagasExtension : IEndpointExtension
    {
        private readonly ICollection<Type> _registeredSagas = new HashSet<Type>();
        private readonly IRabbitMqTopology _rabbitMqTopology;

        private ContainerRegistar _containerRegistar;
        private IRabbitMqBusFactoryConfigurator _configurator;

        public RegisterSagasExtension(IRabbitMqTopology rabbitMqTopology)
        {
            _rabbitMqTopology = rabbitMqTopology;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
            endpointLifecycle.SagaRegistered += OnSagaRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _containerRegistar = e.Registar;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            _configurator = (IRabbitMqBusFactoryConfigurator) e.Configurator;

            RegisterSagas(_registeredSagas);
        }

        private void RegisterSagas(IEnumerable<Type> sagas)
        {
            foreach (var sagaType in sagas)
            {
                // ReSharper disable once PossibleNullReferenceException
                typeof(RegisterSagasExtension).GetMethod(nameof(RegisterStateMachineSaga), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(sagaType, sagaType.GetSagaDataType())
                    .Invoke(this, null);
            }
        }

        private void RegisterStateMachineSaga<TSaga, TSagaData>()
            where TSaga : ISaga<TSagaData>
#pragma warning disable 618
            where TSagaData : class, SagaStateMachineInstance
#pragma warning restore 618
        {
        }

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e) => _registeredSagas.Add(e.SagaType);

        public void Dispose()
        {
        }
    }
}
