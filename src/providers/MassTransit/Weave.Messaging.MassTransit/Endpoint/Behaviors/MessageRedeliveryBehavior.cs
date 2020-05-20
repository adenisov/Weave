using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class MessageRedeliveryBehavior : IEndpointBehavior
    {
        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            e.Configurator.UseInMemoryOutbox();
        }
    }
}
