using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit.NLogIntegration;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.NLog.Behaviors
{
    /// <summary>
    /// Configures the MassTransit pipeline to use NLog. 
    /// </summary>
    public sealed class NLogEndpointExtension : IEndpointExtension
    {
        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private static void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) => e.Configurator.UseNLog();
    }
}
