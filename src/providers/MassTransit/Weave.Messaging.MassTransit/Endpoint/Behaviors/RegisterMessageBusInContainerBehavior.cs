using System;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    public sealed class RegisterMessageBusInContainerBehavior : IEndpointBehavior
    {
        private Action<IInstanceRegistrationSource> _instanceBuilder;

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.MessageBusConfigured += OnMessageBusConfigured;
        }

        private void OnMessageBusConfigured(object sender, MessageBusConfiguredEventArgs e)
        {
            _instanceBuilder(InstanceRegistrationBuilder<IBusControl>.FromInstance(e.NativeBus));
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            _instanceBuilder = e.InstanceBuilder;
        }
    }
}
