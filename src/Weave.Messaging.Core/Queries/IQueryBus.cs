using System;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Queries
{
    /// <summary>
    ///
    /// </summary>
    public interface IQueryBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        Task<TResponse> RequestAsync<TRequest, TResponse>(
            IQueryMessage<TRequest, TResponse> query, Action<QueryConfiguration> configuration = null)
            where TRequest : class, IQueryMessage<TRequest, TResponse>
            where TResponse : class;
    }
}
