using System;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class MessageHandlerRegisteredEventArgs
    {
        public MessageHandlerRegisteredEventArgs(Type messageType, Type messageHandlerType)
        {
            MessageType = messageType;
            MessageHandlerType = messageHandlerType;
        }

        public Type MessageHandlerType { get; }

        public Type MessageType { get; }
    }
}