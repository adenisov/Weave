using Autofac;
using Weave.Messaging.MassTransit.Consumers;

namespace Weave.Messaging.MassTransit.Autofac.Modules
{
    internal sealed class MassTransitRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MassTransitMessageBus>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(QueryHandlerConsumerAdapter<,,>))
                .AsSelf()
                .InstancePerMessage();

            builder.RegisterGeneric(typeof(CommandHandlerConsumerAdapter<,,>))
                .AsSelf()
                .InstancePerMessage();

            builder.RegisterGeneric(typeof(CommandHandlerConsumerAdapter<,>))
                .AsSelf()
                .InstancePerMessage();

            builder.RegisterGeneric(typeof(EventHandlerConsumerAdapter<,>))
                .AsSelf()
                .InstancePerMessage();
        }
    }
}
