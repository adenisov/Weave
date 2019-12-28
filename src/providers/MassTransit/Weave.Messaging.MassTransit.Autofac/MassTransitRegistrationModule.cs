using Autofac;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class MassTransitRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // ToDo: should be part of MassTransitEndpointBase registration process
            builder.RegisterType<MassTransitMessageBus>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
