using System;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class ServiceFactoryConfiguredEventArgs
    {
        public ServiceFactoryConfiguredEventArgs(Func<ConsumeContext, IServiceFactory> serviceFactoryProvider)
        {
            ServiceFactoryProvider = serviceFactoryProvider;
        }

        public Func<ConsumeContext, IServiceFactory> ServiceFactoryProvider { get; }
    }
}
