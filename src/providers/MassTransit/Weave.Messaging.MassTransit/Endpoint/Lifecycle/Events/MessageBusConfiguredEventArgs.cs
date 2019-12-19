using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageBusConfiguredEventArgs
    {
        public MessageBusConfiguredEventArgs(IBusControl nativeBus)
        {
            NativeBus = nativeBus;
        }

        public IBusControl NativeBus { get; }
    }
}
