using System;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;
using MassTransit.Topology;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    public sealed class RegisterEntityFormatter : IEndpointBehavior
    {
        private readonly Func<Type, string> _entityFormatter;

        public RegisterEntityFormatter(Func<Type, string> entityFormatter)
        {
            _entityFormatter = entityFormatter;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.MessageTopology.SetEntityNameFormatter(new InlineEntityFormatter(_entityFormatter));

        private sealed class InlineEntityFormatter : IEntityNameFormatter
        {
            private readonly Func<Type, string> _formatFunc;

            public InlineEntityFormatter(Func<Type, string> formatFunc)
            {
                _formatFunc = formatFunc;
            }

            public string FormatEntityName<T>() => _formatFunc(typeof(T));
        }
    }
}
