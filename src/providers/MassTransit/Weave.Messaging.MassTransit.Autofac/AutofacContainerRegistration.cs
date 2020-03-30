using System;
using System.Linq;
using Autofac;
using Automatonymous;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using ISaga = MassTransit.Saga.ISaga;

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

        public void Register<T>(IInstanceRegistrationSource<T> source)
            where T : class
        {
            var registrationBuilder = _containerBuilder.RegisterInstance(source.Instance);
            if (source.AsTypes != null && source.AsTypes.Any())
            {
                registrationBuilder.As(source.AsTypes);
            }
        }

        public void Register(RegistrationBuilder builder)
        {
            var registrationBuilder = _containerBuilder.RegisterType(builder.Type);
            {
                registrationBuilder = builder.Registrations.Any()
                    ? registrationBuilder.As(builder.Registrations)
                    : registrationBuilder.AsImplementedInterfaces();
            }

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

        public void Register<TConsumer>(IConsumerRegistrationBuilder<TConsumer> builder)
            where TConsumer : class, IConsumer
        {
            builder.Configurator.Consumer<TConsumer>(_container);
        }

        public void Register<TSagaInstance>(IStateMachineSagaRegistrationBuilder<TSagaInstance> builder)
            where TSagaInstance : class, SagaStateMachineInstance
        {
            builder.Configurator.StateMachineSaga<TSagaInstance>(_container);
        }

        public void Register<TSaga>(ISagaRegistrationBuilder<TSaga> builder)
            where TSaga : class, ISaga
        {
            builder.Configurator.Saga<TSaga>(_container);
        }
    }
}
