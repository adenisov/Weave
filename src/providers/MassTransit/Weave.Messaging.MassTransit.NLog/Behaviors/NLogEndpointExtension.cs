using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit.NLogIntegration;
using NLog;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.NLog.Behaviors
{
    /// <summary>
    /// Configures the MassTransit pipeline to use NLog. 
    /// </summary>
    public sealed class NLogEndpointExtension : IEndpointExtension
    {
        private readonly LogFactory _logFactory;

        public NLogEndpointExtension(LogFactory logFactory)
        {
            _logFactory = logFactory;
        }

        public NLogEndpointExtension()
            : this(new LogFactory())
        {
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.UseNLog(_logFactory);

        public void Dispose() => _logFactory?.Dispose();
    }
}
