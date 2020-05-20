using System;
using System.Data;
using GreenPipes;
using GreenPipes.Configurators;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class MessageRetryBehavior : IEndpointBehavior
    {
        private const int RetryAttempts = 3;

        private static readonly Type[] HandledExceptions =
        {
            typeof(DBConcurrencyException)
        };

        private static readonly Type[] IgnoredExceptions =
        {
            typeof(ArgumentException),
            typeof(ArgumentNullException)
        };

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private static void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e)
        {
            static void Configure(IRetryConfigurator configurator)
            {
                var retryConfigurator = configurator.Immediate(RetryAttempts);
                retryConfigurator.Handle(HandledExceptions);
                retryConfigurator.Ignore(IgnoredExceptions);
            }

            e.Configurator.UseRetry(Configure);
        }
    }
}
