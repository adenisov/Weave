using System;
using System.Threading;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Queries
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This is just a marker interface. It shouldn't be implemented.")]
    public interface IQueryHandler : IMessageHandler
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
#pragma warning disable 618
    public interface IQueryHandler<in TRequest, TResponse> : IQueryHandler
#pragma warning restore 618
        where TRequest : class, IQueryMessage<TRequest, TResponse>
        where TResponse : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TResponse> HandleAsync(TRequest query, CancellationToken ct);
    }
}
