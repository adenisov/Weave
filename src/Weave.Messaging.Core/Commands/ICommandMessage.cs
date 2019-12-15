namespace Weave.Messaging.Core.Commands
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface ICommandMessage<TRequest, out TResponse> : ISyncMessage
        where TRequest : class, ICommandMessage<TRequest, TResponse>
        where TResponse : class
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface ICommandMessage<TRequest> : IAsyncMessage
        where TRequest : class, ICommandMessage<TRequest>
    {
    }
}
