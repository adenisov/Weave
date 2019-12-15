using System;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Commands
{
    /// <summary>
    /// Defines a top-level abstraction responsible for mapping the <see cref="ICommandMessage{TRequest}"/> or
    /// <see cref="ICommandMessage{TRequest,TRequest}"/> with the appropriate <see cref="ICommandHandler{TRequest, TResponse>"/>.
    /// </summary>
    public interface ICommandBus
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        Task<TResponse> SendAsync<TRequest, TResponse>(
            ICommandMessage<TRequest, TResponse> command, Action<CommandConfiguration> configuration = null)
            where TRequest : class, ICommandMessage<TRequest, TResponse>
            where TResponse : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="configuration"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <returns></returns>
        Task SendAsync<TRequest>(
            ICommandMessage<TRequest> command, Action<CommandConfiguration> configuration = null)
            where TRequest : class, ICommandMessage<TRequest>;
    }
}
