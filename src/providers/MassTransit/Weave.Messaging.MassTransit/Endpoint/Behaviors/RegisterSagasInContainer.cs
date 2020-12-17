using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class RegisterSagasInContainer : IEndpointBehavior
    {
        private readonly ISet<Type> _sagaTypes = new HashSet<Type>();

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.SagaRegistered += OnSagaRegistered;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            foreach (var sagaType in _sagaTypes)
            {
                e.Registar(
                    RegistrationBuilder
                        .RegisterType(sagaType)
                        .Singleton()
                );
            }
        }

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e) => _sagaTypes.Add(e.SagaType);
    }
}
