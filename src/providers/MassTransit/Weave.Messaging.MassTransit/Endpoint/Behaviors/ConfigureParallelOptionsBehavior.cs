using System;
using GreenPipes;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class ConfigureParallelOptionsBehavior : IEndpointBehavior
    {
        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private static void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            e.Configurator.UseConcurrencyLimit(Environment.ProcessorCount * 10);
        }
    }
}
