using System;
using System.Linq;
using Autofac;
using Automatonymous;
using Automatonymous.SagaConfigurators;
using MassTransit;
using MassTransit.AutofacIntegration.Registration;
using MassTransit.AutofacIntegration.ScopeProviders;
using MassTransit.Registration;
using MassTransit.Scoping;
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
            _containerBuilder.RegisterBuildCallback(c => _container = (IContainer) c);
        }

        public void Register<T>(IInstanceRegistrationSource<T> source)
            where T : class
        {
            EnsureContainerIsNotBuilt();

            var registrationBuilder = _containerBuilder.RegisterInstance(source.Instance);
            if (source.AsTypes.Any())
            {
                registrationBuilder.As(source.AsTypes);
            }
        }

        public void Register(RegistrationBuilder builder)
        {
            EnsureContainerIsNotBuilt();

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
            var scopeProvider = new AutofacConsumerScopeProvider(
                new MessageLifetimeScopeProvider(_container),
                "message",
                null);

            var consumerFactory = new ScopeConsumerFactory<TConsumer>(scopeProvider);

            builder.Configurator.Consumer(consumerFactory, builder.ConsumerConfigurator);
        }

        public void Register<TSagaInstance>(IStateMachineSagaRegistrationBuilder<TSagaInstance> builder)
            where TSagaInstance : class, SagaStateMachineInstance
        {
            var scope = _container.Resolve<ILifetimeScope>();

            var stateMachine = scope.Resolve<SagaStateMachine<TSagaInstance>>();
            ISagaRepositoryFactory repositoryFactory = new AutofacSagaRepositoryFactory(
                new MessageLifetimeScopeProvider(scope),
                "message");

            var repository = repositoryFactory.CreateSagaRepository<TSagaInstance>();
            var stateMachineConfigurator = new StateMachineSagaConfigurator<TSagaInstance>(stateMachine, repository, builder.Configurator);

            builder.SagaConfigurator?.Invoke(stateMachineConfigurator);
            builder.Configurator.AddEndpointSpecification(stateMachineConfigurator);
        }

        public void Register<TSaga>(ISagaRegistrationBuilder<TSaga> builder)
            where TSaga : class, ISaga
        {
            builder.Configurator.Saga<TSaga>(_container);
        }

        private void EnsureContainerIsNotBuilt()
        {
            if (_container != null)
            {
                throw new InvalidOperationException("cannot perform registration when Container is built.");
            }
        }
    }
}
