using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class ContainerRegisteredEventArgs
    {
        public ContainerRegisteredEventArgs(Action<IContainerRegistar> registar)
        {
            Registar = registar;
        }

        public Action<IContainerRegistar> Registar { get; }
    }
}
