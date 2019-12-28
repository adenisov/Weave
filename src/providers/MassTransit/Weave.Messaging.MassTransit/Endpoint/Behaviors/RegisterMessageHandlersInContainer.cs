using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using Weave.Messaging.Core;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    public sealed class RegisterMessageHandlersInContainer : IEndpointBehavior
    {
        private readonly ICollection<Type> _messageHandlerTypes = new HashSet<Type>();

        private Action<IContainerRegistar> _registrationBuilder;

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.QueryHandlerRegistered += OnQueryHandlerRegistered;
            endpointLifecycle.CommandHandlerRegistered += OnCommandHandlerRegistered;
            endpointLifecycle.EventHandlerRegistered += OnEventHandlerRegistered;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.MessageBusConfigured += OnMessageBusConfigured;
        }

        private void OnMessageBusConfigured(object sender, MessageBusConfiguredEventArgs e)
        {
            // ToDo: consider registering consumer adapters here as well, because they are part of MassTransit ecosystem.
            // ToDo: register before container is built
            _messageHandlerTypes.ForEach(t =>
                _registrationBuilder(RegistrationBuilder.RegisterType(t).AsSelf()));
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _registrationBuilder = e.Registar;
        }

        private void OnQueryHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e)
        {
            _messageHandlerTypes.Add(e.MessageHandlerType);
        }

        private void OnCommandHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e)
        {
            _messageHandlerTypes.Add(e.MessageHandlerType);
        }

        private void OnEventHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e)
        {
            _messageHandlerTypes.Add(e.MessageHandlerType);
        }
    }
}
