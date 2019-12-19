using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageBusStartingEventArgs
    {
        public MessageBusStartingEventArgs(IBusControl nativeBus)
        {
            NativeBus = nativeBus;
        }

        public IBusControl NativeBus { get; }
    }
}
