using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Commands;

namespace Weave.Messaging.MassTransit.Consumers
{
    public sealed class CommandHandlerConsumerAdapter<THandler, TRequest> : UnresponsiveConsumerBase<TRequest>
        where THandler : ICommandHandler<TRequest>
        where TRequest : class, ICommandMessage<TRequest>
    {
        private readonly THandler _commandHandler;

        public CommandHandlerConsumerAdapter(THandler commandHandler, IEnumerable<IBehavior<TRequest, VoidResponse>> behaviors)
            : base(behaviors)
        {
            _commandHandler = commandHandler;
        }

        protected override async Task HandleVoidResponseAsync(TRequest request, CancellationToken ct)
        {
            await _commandHandler.HandleAsync(request, ct).DropContext();
        }
    }

    public sealed class CommandHandlerConsumerAdapter<THandler, TRequest, TResponse> : ConsumerBase<TRequest, TResponse>
        where THandler : ICommandHandler<TRequest, TResponse>
        where TRequest : class, ICommandMessage<TRequest, TResponse>
        where TResponse : class
    {
        private readonly THandler _commandHandler;

        public CommandHandlerConsumerAdapter(THandler commandHandler, IEnumerable<IBehavior<TRequest, TResponse>> behaviors)
            : base(behaviors)
        {
            _commandHandler = commandHandler;
        }

        protected override async Task<TResponse> HandleInternalAsync(TRequest request, CancellationToken ct)
        {
            return await _commandHandler.HandleAsync(request, ct).DropContext();
        }
    }
}
