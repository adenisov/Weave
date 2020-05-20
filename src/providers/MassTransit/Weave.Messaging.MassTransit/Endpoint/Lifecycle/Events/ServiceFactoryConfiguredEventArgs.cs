using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class ServiceFactoryConfiguredEventArgs
    {
        public ServiceFactoryConfiguredEventArgs(Func<IServiceFactory> serviceFactoryProvider)
        {
            ServiceFactoryProvider = serviceFactoryProvider;
        }

        public Func<IServiceFactory> ServiceFactoryProvider { get; }
    }
}
