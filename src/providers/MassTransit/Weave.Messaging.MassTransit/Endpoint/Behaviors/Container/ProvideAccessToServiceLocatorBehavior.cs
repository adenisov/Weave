using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors.Container
{
    internal sealed class ProvideAccessToServiceLocatorBehavior : IEndpointBehavior
    {
        private Func<IServiceFactory> _serviceFactory;

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ServiceFactoryConfigured += OnServiceFactoryConfigured;
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnServiceFactoryConfigured(object sender, ServiceFactoryConfiguredEventArgs e)
        {
            _serviceFactory = e.ServiceFactoryProvider;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            e.Configurator.UseFilter(new ServiceFactoryInjector(_serviceFactory));
        }

        private sealed class ServiceFactoryInjector : IFilter<ConsumeContext>
        {
            private readonly Func<IServiceFactory> _serviceFactory;

            public ServiceFactoryInjector(Func<IServiceFactory> serviceFactory)
            {
                _serviceFactory = serviceFactory;
            }

            public Task Send(ConsumeContext context, IPipe<ConsumeContext> next)
            {
                context.AddOrUpdatePayload(
                    () => _serviceFactory(),
                    existing => existing ?? _serviceFactory());

                return next.Send(context);
            }

            public void Probe(ProbeContext context)
            {
                context.Add("provider", "weave");
            }
        }
    }
}
