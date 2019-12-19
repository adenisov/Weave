using MassTransit;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageBusStartedEventArgs
    {
        public MessageBusStartedEventArgs(IBusControl nativeBus, IMassTransitMessageBus bus)
        {
            NativeBus = nativeBus;
            Bus = bus;
        }

        public IBusControl NativeBus { get; }

        public IMassTransitMessageBus Bus { get; }
    }
}
