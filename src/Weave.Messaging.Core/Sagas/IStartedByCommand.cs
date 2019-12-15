using Weave.Messaging.Core.Commands;
using Weave.Messaging.Core.Events;

namespace Weave.Messaging.Core.Sagas
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IStartedByCommand<in TRequest> : ICommandHandler<TRequest>
        where TRequest : class, ICommandMessage<TRequest>
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IStartedByCommand<in TRequest, TResponse> : ICommandHandler<TRequest, TResponse>
        where TRequest : class, ICommandMessage<TRequest, TResponse>
        where TResponse : class
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    public interface IStartedByEvent<in TEvent> : IEventHandler<TEvent>
        where TEvent : IEventMessage<TEvent>
    {
    }
}
