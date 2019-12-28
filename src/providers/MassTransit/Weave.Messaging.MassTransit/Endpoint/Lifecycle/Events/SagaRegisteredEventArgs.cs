using System;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class SagaRegisteredEventArgs
    {
        public SagaRegisteredEventArgs(Type sagaType)
        {
            SagaType = sagaType;
        }

        public Type SagaType { get; }
    }
}
