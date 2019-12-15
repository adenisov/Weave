namespace Weave.Messaging.Core.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueryMessage : ISyncMessage
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IQueryMessage<TRequest, out TResponse> : IQueryMessage
        where TRequest : class, IQueryMessage<TRequest, TResponse>
        where TResponse : class
    {
    }
}
