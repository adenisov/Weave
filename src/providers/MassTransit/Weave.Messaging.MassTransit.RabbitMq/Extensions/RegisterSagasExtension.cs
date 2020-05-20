using System;
using System.Collections.Generic;
using System.Reflection;
using Automatonymous;
using MassTransit.RabbitMqTransport;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.RabbitMq.Extensions
{
    public sealed class RegisterSagasExtension : IEndpointExtension
    {
        private readonly ICollection<Type> _registeredSagas = new HashSet<Type>();
        private readonly IRabbitMqTopology _rabbitMqTopology;

        private ContainerRegistar _containerRegistar;
        private IRabbitMqHost _host;
        private IRabbitMqBusFactoryConfigurator _configurator;

        public RegisterSagasExtension(IRabbitMqTopology rabbitMqTopology)
        {
            _rabbitMqTopology = rabbitMqTopology;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.MessageBusTransportConfigured += OnMessageBusTransportConfigured;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
            endpointLifecycle.SagaRegistered += OnSagaRegistered;
        }

        private void OnMessageBusTransportConfigured(object sender, MessageBusTransportConfiguredEventArgs e)
        {
            _host = (IRabbitMqHost) e.Host;
            _configurator = (IRabbitMqBusFactoryConfigurator) e.Configurator;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _containerRegistar = e.Registar;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            RegisterSagas(_registeredSagas);
        }

        private void RegisterSagas(IEnumerable<Type> sagas)
        {
            foreach (var sagaType in sagas)
            {
                // ReSharper disable once PossibleNullReferenceException
                typeof(RegisterSagasExtension).GetMethod(nameof(RegisterStateMachineSaga), BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(sagaType)
                    .Invoke(this, null);
            }
        }

        private void RegisterStateMachineSaga<TSaga>()
#pragma warning disable 618
            where TSaga : class, SagaStateMachineInstance
#pragma warning restore 618
        {
            _configurator.ReceiveEndpoint(
                _host,
                _rabbitMqTopology.GetLocalInputQueueName(typeof(TSaga)),
                c =>
                {
                    _containerRegistar(
                        SagaRegistrationFactory
                            .ForStateMachineSaga<TSaga>()
                            .WithReceiveEndpointConfiguration(c));
                });
        }

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e)
        {
            _registeredSagas.Add(e.SagaType);

            _containerRegistar(RegistrationBuilder.RegisterType(e.SagaType));
        }

        public void Dispose()
        {
        }
    }
}
