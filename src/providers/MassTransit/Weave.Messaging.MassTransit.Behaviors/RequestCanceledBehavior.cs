using System;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.MassTransit.Consumers.Behaviors;

namespace Weave.Messaging.MassTransit.Behaviors
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public sealed class RequestCanceledBehavior<TRequest, TResponse> : IBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        public async Task<TResponse> HandleAsync(IIncomingMessage<TRequest> message, CancellationToken ct, Func<Task<TResponse>> next)
        {
            ct.ThrowIfCancellationRequested();
            
            return await next().ConfigureAwait(false);
        }
    }
}
