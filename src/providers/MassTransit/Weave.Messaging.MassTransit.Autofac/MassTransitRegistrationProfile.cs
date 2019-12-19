using Autofac;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class MassTransitRegistrationProfile : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MassTransitMessageBus>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
