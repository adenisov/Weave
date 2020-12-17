using System;
using System.Diagnostics;
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
    public sealed class RequestIdBehavior<TRequest, TResponse> : IBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        public async Task<TResponse> HandleAsync(IIncomingMessage<TRequest> message, CancellationToken ct, Func<Task<TResponse>> next)
        {
            try
            {
                var requestId = message.Headers.MessageId ?? Guid.NewGuid();

                Trace.CorrelationManager.StartLogicalOperation(requestId);
                Trace.CorrelationManager.ActivityId = requestId;

                return await next().ConfigureAwait(false);
            }
            finally
            {
                Trace.CorrelationManager.ActivityId = Guid.Empty;
                Trace.CorrelationManager.StopLogicalOperation();
            }
        }
    }
}
