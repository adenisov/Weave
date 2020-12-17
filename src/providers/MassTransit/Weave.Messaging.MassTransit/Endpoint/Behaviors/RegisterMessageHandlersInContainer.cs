using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class RegisterMessageHandlersInContainer : IEndpointBehavior
    {
        private readonly ISet<Type> _messageHandlerTypes = new HashSet<Type>();

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageHandlerRegistered += OnMessageHandlerRegistered;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            foreach (var handlerType in _messageHandlerTypes)
            {
                e.Registar(
                    RegistrationBuilder.RegisterType(handlerType).AsSelf()
                );
            }
        }

        private void OnMessageHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e) =>
            _messageHandlerTypes.Add(e.MessageHandlerType);
    }
}
