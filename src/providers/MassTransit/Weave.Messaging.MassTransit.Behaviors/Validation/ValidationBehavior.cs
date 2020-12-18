using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Weave.Messaging.MassTransit.Consumers.Behaviors;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public sealed class ValidationBehavior<TRequest, TResponse> : IBehavior<TRequest, TResponse>
        where TRequest : class
        where TResponse : class
    {
        private readonly IRequestValidatorResolver _validatorResolver;

        public ValidationBehavior(IRequestValidatorResolver validatorResolver)
        {
            _validatorResolver = validatorResolver;
        }

        public async Task<TResponse> HandleAsync(IIncomingMessage<TRequest> message, CancellationToken ct, Func<Task<TResponse>> next)
        {
            var validators = _validatorResolver.Resolve<TRequest>();

            var validationResults = await Task.WhenAll(
                    validators.Select(v => v.ValidateAsync(message.Body, ct)))
                .ConfigureAwait(false);

            var failures = validationResults.Where(v => !v.IsValid).ToList();
            if (failures.Any())
            {
                throw new RequestValidationException($"{typeof(TRequest)} validation failed.", failures);
            }

            return await next().ConfigureAwait(false);
        }
    }
}
