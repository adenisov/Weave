using System;
using System.Linq;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Autofac;
using MassTransit;

namespace Weave.Messaging.MassTransit.Autofac
{
    public sealed class AutofacContainerConfigurator : IContainerConfigurator
    {
        private readonly ContainerBuilder _containerBuilder;
        private readonly MassTransitRegistrationProfile _registrationProfile;

        private IContainer _container;

        public AutofacContainerConfigurator(ContainerBuilder containerBuilder)
            : this(containerBuilder, new MassTransitRegistrationProfile())
        {
        }

        private AutofacContainerConfigurator(ContainerBuilder containerBuilder, MassTransitRegistrationProfile registrationProfile)
        {
            _containerBuilder = containerBuilder;
            _registrationProfile = registrationProfile;
        }

        public void Configure(IMassTransitEndpointLifecycle lifecycle)
        {
            _containerBuilder.RegisterBuildCallback(c => _container = c);
            _containerBuilder.AddMassTransit(c => { c.Builder.RegisterModule(_registrationProfile); });

            lifecycle.EmitContainerProvided(RegisterService, RegisterInstance);
        }

        public Func<IServiceFactory> GetServiceFactoryProvider()
        {
            IServiceFactory ServiceFactoryProvider()
            {
                if (_container == null)
                {
                    throw new InvalidOperationException("cannot provide service factory because container hasn't been build yet.");
                }

                return new AutofacServiceFactory(_container.BeginLifetimeScope());
            }

            return ServiceFactoryProvider;
        }

        private void RegisterService(RegistrationBuilder builder)
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

        private void RegisterInstance(IInstanceRegistrationSource registrationSource)
        {
            registrationSource.Register(new AutofacInstanceRegistration(_containerBuilder, registrationSource));
        }
    }
}
