using System;
using MassTransit;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.MassTransit
{
    internal sealed class ConfigurePredefinedMessageHeaders : MessageBusBehaviorBase
    {
        public override void HandleRequest<TRequest, TResponse>(
            IQueryMessage<TRequest, TResponse> query, SendContext<TRequest> context, QueryConfiguration configuration)
        {
            AppendPredefinedHeaders(configuration, context);
        }

        public override void HandleSend<TRequest>(
            ICommandMessage<TRequest> command, SendContext<TRequest> context, CommandConfiguration configuration)
        {
            AppendPredefinedHeaders(configuration, context);
        }

        public override void HandleSendWithResult<TRequest, TResponse>(
            ICommandMessage<TRequest, TResponse> command, SendContext<TRequest> context, CommandConfiguration configuration)
        {
            AppendPredefinedHeaders(configuration, context);
        }

        public override void HandlePublish<TEvent>(
            IEventMessage<TEvent> @event, PublishContext<TEvent> context, EventConfiguration configuration)
        {
            AppendPredefinedHeaders(configuration, context);
        }

        private static void AppendPredefinedHeaders(IMessageConfiguration messageConfiguration, SendContext sendContext)
        {
            sendContext.MessageId = messageConfiguration.Headers.MessageId ?? Guid.NewGuid();
            sendContext.CorrelationId = messageConfiguration.Headers.CorrelationId ?? NewId.NextGuid();
        }
    }
}
