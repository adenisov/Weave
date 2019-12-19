using System.Threading.Tasks;
using MassTransit;
using Weave.Messaging.Core;
using Weave.Messaging.Core.Commands;

namespace Weave.Messaging.MassTransit
{
    public sealed class CommandHandlerConsumerAdapter<THandler, TRequest> : IConsumer<TRequest>
        where THandler : ICommandHandler<TRequest>
        where TRequest : class, ICommandMessage<TRequest>
    {
        private readonly THandler _commandHandler;

        public CommandHandlerConsumerAdapter(THandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            await _commandHandler.HandleAsync(context.Message, context.CancellationToken).DropContext();
        }
    }

    public sealed class CommandHandlerConsumerAdapter<THandler, TRequest, TResponse> : IConsumer<TRequest>
        where THandler : ICommandHandler<TRequest, TResponse>
        where TRequest : class, ICommandMessage<TRequest, TResponse>
        where TResponse : class
    {
        private readonly THandler _commandHandler;

        public CommandHandlerConsumerAdapter(THandler commandHandler)
        {
            _commandHandler = commandHandler;
        }

        public async Task Consume(ConsumeContext<TRequest> context)
        {
            var response = await _commandHandler.HandleAsync(context.Message, context.CancellationToken).DropContext();
            await context.RespondAsync(response).DropContext();
        }
    }
}
