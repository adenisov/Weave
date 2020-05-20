using MassTransit;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.MassTransit
{
    public class MessageBusBehaviorBase : IMessageBusBehavior
    {
        protected MessageBusBehaviorBase()
        {
        }

        /// <inheritdoc />
        /// Base implementation does nothing
        public virtual void HandleRequest<TRequest, TResponse>(
            IQueryMessage<TRequest, TResponse> query, SendContext<TRequest> context, QueryConfiguration configuration)
            where TRequest : class, IQueryMessage<TRequest, TResponse>
            where TResponse : class
        {
        }

        /// <inheritdoc />
        /// Base implementation does nothing
        public virtual void HandleSend<TRequest>(
            ICommandMessage<TRequest> command, SendContext<TRequest> context, CommandConfiguration configuration)
            where TRequest : class, ICommandMessage<TRequest>
        {
        }

        /// <inheritdoc />
        /// Base implementation does nothing
        public virtual void HandleSendWithResult<TRequest, TResponse>(
            ICommandMessage<TRequest, TResponse> command, SendContext<TRequest> context, CommandConfiguration configuration)
            where TRequest : class, ICommandMessage<TRequest, TResponse>
            where TResponse : class
        {
        }

        /// <inheritdoc />
        /// Base implementation does nothing
        public virtual void HandlePublish<TEvent>(
            IEventMessage<TEvent> @event, PublishContext<TEvent> context, EventConfiguration configuration)
            where TEvent : class, IEventMessage<TEvent>
        {
        }
    }
}
