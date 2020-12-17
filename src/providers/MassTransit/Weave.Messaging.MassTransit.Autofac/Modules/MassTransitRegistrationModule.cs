using System;
using Autofac;
using Automatonymous;
using MassTransit;
using Weave.Messaging.MassTransit.Consumers;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

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

            /* ToDo: is that needed?
            builder.RegisterType<AutofacStateMachineActivityFactory>()
                .As<IStateMachineActivityFactory>()
                .SingleInstance();

            builder.RegisterType<AutofacSagaStateMachineFactory>()
                .As<ISagaStateMachineFactory>()
                .SingleInstance();
            */

            builder.Register(context =>
            {
                var consumeContext = context.Resolve<ConsumeContext>();
                if (consumeContext == null)
                {
                    throw new InvalidOperationException($"cannot resolve {nameof(consumeContext)} as it's not properly" +
                                                        "registered in Container.");
                }

                return consumeContext.GetServiceFactory();
            });
        }
    }
}
