using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class RegisterMessageHandlersInContainer : IEndpointBehavior
    {
        private ContainerRegistar _containerRegistar;

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.QueryHandlerRegistered += OnQueryHandlerRegistered;
            endpointLifecycle.CommandHandlerRegistered += OnCommandHandlerRegistered;
            endpointLifecycle.EventHandlerRegistered += OnEventHandlerRegistered;
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e) => _containerRegistar = e.Registar;

        private void OnQueryHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e) =>
            RegisterMessageHandlerDependency(e.MessageHandlerType);

        private void OnCommandHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e) =>
            RegisterMessageHandlerDependency(e.MessageHandlerType);

        private void OnEventHandlerRegistered(object sender, MessageHandlerRegisteredEventArgs e) =>
            RegisterMessageHandlerDependency(e.MessageHandlerType);

        private void RegisterMessageHandlerDependency(Type messageHandlerType) =>
            _containerRegistar(RegistrationBuilder
                .RegisterType(messageHandlerType)
                .AsSelf());
    }
}
