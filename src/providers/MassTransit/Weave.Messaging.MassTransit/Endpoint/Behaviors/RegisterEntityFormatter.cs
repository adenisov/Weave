using System;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit.Topology;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class RegisterEntityFormatter : IEndpointBehavior
    {
        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private static void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.MessageTopology.SetEntityNameFormatter(
                new InlineEntityFormatter(t => MessageUrn.ForType(t).ToString()));

        private sealed class InlineEntityFormatter : IEntityNameFormatter
        {
            private readonly Func<Type, string> _formatFunc;

            public InlineEntityFormatter(Func<Type, string> formatFunc)
            {
                _formatFunc = formatFunc ?? throw new ArgumentNullException(nameof(formatFunc));
            }

            public string FormatEntityName<T>() => _formatFunc(typeof(T));
        }
    }
}
