using System.Transactions;
using MassTransit;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    internal sealed class ExecuteHandlersInTransactionBehavior : IEndpointBehavior
    {
        private readonly IsolationLevel _isolationLevel;

        public ExecuteHandlersInTransactionBehavior(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
        {
            _isolationLevel = isolationLevel;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) => 
            e.Configurator.UseTransaction(c => c.IsolationLevel = _isolationLevel);
    }
}
