using System;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit.Topology;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    public sealed class RegisterEntityFormatter : IEndpointBehavior
    {
        private readonly Func<IEntityNameFormatter> _formatterProvider;

        public RegisterEntityFormatter(Func<IEntityNameFormatter> formatterProvider)
        {
            _formatterProvider = formatterProvider;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            e.Configurator.MessageTopology.SetEntityNameFormatter(_formatterProvider());
        }
    }
}
