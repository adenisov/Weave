using System;
using System.Threading.Tasks;

namespace Weave.Messaging.MassTransit.Consumers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IBehavior<in TRequest, TResponse>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        Task<TResponse> HandleAsync(IIncomingMessage<TRequest> message, Func<Task<TResponse>> next);
    }
}
