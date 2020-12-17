using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    internal sealed class ProvideAccessToServiceLocatorBehavior : IEndpointBehavior
    {
        private Func<ConsumeContext, IServiceFactory> _serviceFactory;

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ServiceFactoryConfigured += OnServiceFactoryConfigured;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnServiceFactoryConfigured(object sender, ServiceFactoryConfiguredEventArgs e) =>
            _serviceFactory = e.ServiceFactoryProvider;

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.UseFilter(new ServiceLocatorInjector(_serviceFactory));

        private sealed class ServiceLocatorInjector : IFilter<ConsumeContext>
        {
            private readonly Func<ConsumeContext, IServiceFactory> _serviceFactory;

            public ServiceLocatorInjector(Func<ConsumeContext, IServiceFactory> serviceFactory)
            {
                _serviceFactory = serviceFactory;
            }

            [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
            public async Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
            {
                using var serviceFactory = _serviceFactory(context);

                context.AddOrUpdatePayload(
                    () => serviceFactory,
                    existing => existing ?? serviceFactory);

                await next.Send(context).ConfigureAwait(false);
            }

            public void Probe(ProbeContext context) => context.CreateScope("serviceLocator");
        }
    }
}
