using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events
{
    public sealed class ContainerRegisteredEventArgs
    {
        public ContainerRegisteredEventArgs(ContainerRegistar registar)
        {
            Registar = registar;
        }

        public ContainerRegistar Registar { get; }
    }
}
