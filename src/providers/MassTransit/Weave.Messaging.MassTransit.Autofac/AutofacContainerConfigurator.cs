using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Autofac;
using MassTransit;
using MassTransit.AutofacIntegration;
using Weave.Messaging.MassTransit.Autofac.Modules;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class AutofacContainerConfigurator : IContainerConfigurator
    {
        internal const string ScopeName = "pipe";

        private readonly AutofacContainerRegistration _containerRegistration;
        private readonly ContainerBuilder _containerBuilder;
        private readonly Module[] _extensionModules;

        private IContainer _container;

        public AutofacContainerConfigurator(ContainerBuilder containerBuilder, params Module[] modules)
        {
            _containerBuilder = containerBuilder;
            _extensionModules = modules;
            _containerBuilder.RegisterBuildCallback(c => _container = (IContainer) c);
            _containerRegistration = new AutofacContainerRegistration(_containerBuilder);

            RegisterModules();
        }

        public void Configure(Func<IBusControl> busConfigureFactory) =>
            _containerBuilder.AddMassTransit(c => c.AddBus(context => busConfigureFactory()));

        public Func<ConsumeContext, IServiceFactory> ServiceFactoryProvider => context =>
        {
            EnsureContainerIsBuilt();

            if (context != null)
            {
                return new AutofacServiceFactory(
                    _container.BeginLifetimeScope(ScopeName, builder => builder.ConfigureScope(context)));
            }

            return new AutofacServiceFactory(_container.BeginLifetimeScope(ScopeName));
        };

        public IContainerRegistration ContainerRegistration => _containerRegistration;

        private void RegisterModules()
        {
            _containerBuilder.RegisterModule<MassTransitRegistrationModule>();
            RegisterExtensionModules();
        }

        private void RegisterExtensionModules()
        {
            foreach (var profile in _extensionModules)
            {
                _containerBuilder.RegisterModule(profile);
            }
        }

        private void EnsureContainerIsBuilt()
        {
            if (_container == null)
            {
                throw new InvalidOperationException("cannot provide service factory because container hasn't been build yet.");
            }
        }

        public void Dispose() => _container?.Dispose();
    }
}
