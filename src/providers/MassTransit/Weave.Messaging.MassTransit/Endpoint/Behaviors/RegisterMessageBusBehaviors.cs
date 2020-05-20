using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class RegisterMessageBusBehaviors : IEndpointBehavior
    {
        private readonly IMessageBusBehavior[] _busBehaviors;

        public RegisterMessageBusBehaviors(params IMessageBusBehavior[] busBehaviors)
        {
            _busBehaviors = busBehaviors;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            foreach (var busBehavior in _busBehaviors)
            {
                e.Registar(InstanceRegistrationBuilder<IMessageBusBehavior>.FromInstance(busBehavior));
            }
        }
    }
}
