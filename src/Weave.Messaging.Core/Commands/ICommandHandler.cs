using System;
using System.Threading;
using System.Threading.Tasks;

namespace Weave.Messaging.Core.Commands
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This is just a marker interface. It shouldn't be implemented.")]
    public interface ICommandHandler
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
#pragma warning disable 618
    public interface ICommandHandler<in TRequest, TResponse> : ICommandHandler
#pragma warning restore 618
        where TRequest : class, ICommandMessage<TRequest, TResponse>
        where TResponse : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<TResponse> HandleAsync(TRequest command, CancellationToken ct);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
#pragma warning disable 618
    public interface ICommandHandler<in TRequest> : ICommandHandler
        where TRequest : class, ICommandMessage<TRequest>
#pragma warning restore 618
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task HandleAsync(TRequest command, CancellationToken ct);
    }
}
