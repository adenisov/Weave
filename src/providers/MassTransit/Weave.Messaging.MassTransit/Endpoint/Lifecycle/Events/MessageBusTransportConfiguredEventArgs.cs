using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageBusTransportConfiguredEventArgs
    {
        public MessageBusTransportConfiguredEventArgs(IHost host, IBusFactoryConfigurator configurator)
        {
            Host = host;
            Configurator = configurator;
        }

        public IHost Host { get; }

        public IBusFactoryConfigurator Configurator { get; }
    }
}
