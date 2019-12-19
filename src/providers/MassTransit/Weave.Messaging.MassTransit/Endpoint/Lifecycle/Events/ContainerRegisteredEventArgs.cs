using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class ContainerRegisteredEventArgs
    {
        public ContainerRegisteredEventArgs(Action<RegistrationBuilder> builder, Action<IInstanceRegistrationSource> instanceBuilder)
        {
            Builder = builder;
            InstanceBuilder = instanceBuilder;
        }

        public Action<RegistrationBuilder> Builder { get; }

        public Action<IInstanceRegistrationSource> InstanceBuilder { get; }
    }
}
