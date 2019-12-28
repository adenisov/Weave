using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageBusConfiguringEventArgs
    {
        public MessageBusConfiguringEventArgs(IBusFactoryConfigurator configurator)
        {
            Configurator = configurator;
        }

        public IBusFactoryConfigurator Configurator { get; }
    }
}
