using System;
using System.Linq;
using Autofac;
using Automatonymous;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class AutofacContainerRegistration : IContainerRegistration
    {
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;

        public AutofacContainerRegistration(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
            _containerBuilder.RegisterBuildCallback(c => _container = c);
        }

        public void Register<T>(IInstanceRegistrationSource<T> source) where T : class
        {
            var registrationBuilder = _containerBuilder.RegisterInstance(source.Instance);
            if (source.AsTypes != null && source.AsTypes.Any())
            {
                registrationBuilder.As(source.AsTypes);
            }
        }

        public void Register(RegistrationBuilder builder)
        {
            var registrationBuilder = _containerBuilder
                .RegisterType(builder.Type)
                .As(builder.Registrations.ToArray());

            switch (builder.LifetimeScope)
            {
                case LifetimeScope.Transient:
                case LifetimeScope.Dependency:
                    registrationBuilder.InstancePerDependency();
                    break;
                case LifetimeScope.ParentScope:
                    registrationBuilder.InstancePerLifetimeScope();
                    break;
                case LifetimeScope.Shared:
                    registrationBuilder.SingleInstance();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(builder.LifetimeScope));
            }
        }

        public void Register<TConsumer>(IConsumerRegistrationBuilder<TConsumer> consumerRegistrationBuilder)
            where TConsumer : class, IConsumer
        {
            consumerRegistrationBuilder.Configurator.Consumer<TConsumer>(_container);
        }

        public void Register<TSagaInstance>(ISagaRegistrationBuilder<TSagaInstance> sagaRegistrationBuilder)
            where TSagaInstance : class, SagaStateMachineInstance
        {
            sagaRegistrationBuilder.Configurator.StateMachineSaga<TSagaInstance>(_container);
        }
    }
}
