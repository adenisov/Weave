using System;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Autofac;
using MassTransit;
using Module = Autofac.Module;

namespace Weave.Messaging.MassTransit.Autofac
{
    public sealed class AutofacContainerConfigurator : IContainerConfigurator
    {
        private readonly AutofacContainerRegistration _containerRegistration;
        private readonly ContainerBuilder _containerBuilder;
        private readonly Module[] _extensionModules;

        private IContainer _container;

        public AutofacContainerConfigurator(ContainerBuilder containerBuilder, params Module[] modules)
        {
            _containerBuilder = containerBuilder;
            _extensionModules = modules;
            _containerBuilder.RegisterBuildCallback(c => _container = c);
            _containerRegistration = new AutofacContainerRegistration(_containerBuilder);
            RegisterModules();
        }

        public void Configure(Func<IBusControl> busConfigureFactory) =>
            _containerBuilder.AddMassTransit(c => c.AddBus(componentContext => busConfigureFactory()));

        public Func<IServiceFactory> ServiceFactoryProvider
        {
            get
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
        }

        public IContainerRegistration ContainerRegistration => _containerRegistration;

        private void RegisterModules()
        {
            _containerBuilder.RegisterModule<MassTransitRegistrationModule>();
            foreach (var profile in _extensionModules)
            {
                _containerBuilder.RegisterModule(profile);
            }
        }
    }
}
