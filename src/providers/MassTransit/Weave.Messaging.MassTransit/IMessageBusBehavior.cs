using MassTransit;
using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;
using Weave.Messaging.Core.Queries;

namespace Weave.Messaging.MassTransit
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageBusBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        void HandleRequest<TRequest, TResponse>(
            IQueryMessage<TRequest, TResponse> query, SendContext<TRequest> context, QueryConfiguration configuration)
            where TRequest : class, IQueryMessage<TRequest, TResponse>
            where TResponse : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        void HandleSend<TRequest>(
            ICommandMessage<TRequest> command, SendContext<TRequest> context, CommandConfiguration configuration)
            where TRequest : class, ICommandMessage<TRequest>;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        void HandleSendWithResult<TRequest, TResponse>(
            ICommandMessage<TRequest, TResponse> command, SendContext<TRequest> context, CommandConfiguration configuration)
            where TRequest : class, ICommandMessage<TRequest, TResponse>
            where TResponse : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="event"></param>
        /// <param name="context"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TEvent"></typeparam>
        void HandlePublish<TEvent>(IEventMessage<TEvent> @event, PublishContext<TEvent> context, EventConfiguration configuration)
            where TEvent : class, IEventMessage<TEvent>;
    }
}
