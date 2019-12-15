using System;
using System.Linq;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Autofac;

namespace Weave.Messaging.MassTransit.Autofac
{
    public sealed class AutofacContainerConfigurator : IContainerConfigurator
    {
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;

        public AutofacContainerConfigurator(ContainerBuilder containerBuilder)
        {
            _containerBuilder = containerBuilder;
        }

        public void Configure(IMassTransitEndpointLifecycle lifecycle)
        {
            lifecycle.EmitContainerProvided(RegisterService);
            lifecycle.MessageBusStarting += OnMessageBusStarting;
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

        private void OnMessageBusStarting(object sender, EventArgs e)
        {
            _container = _containerBuilder.Build();
        }

        private void RegisterService(RegistrationBuilder builder)
        {
            _containerBuilder.RegisterType(builder.Type).As(builder.Registrations.ToArray());
        }
    }
}
