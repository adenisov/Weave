using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    public class RequestValidator<TRequest> : AbstractValidator<TRequest>, IRequestValidator<TRequest>
        where TRequest : class
    {
        protected RequestValidator()
        {
        }

        ValidationResult IRequestValidator<TRequest>.Validate(TRequest request)
        {
            var validationResult = Validate(new ValidationContext<TRequest>(request));

            return new ValidationResult
            {
                IsValid = validationResult.IsValid,
                ErrorMessage = validationResult.ToString()
            };
        }

        async Task<ValidationResult> IRequestValidator<TRequest>.ValidateAsync(TRequest request, CancellationToken ct)
        {
            var validationResult = await ValidateAsync(new ValidationContext<TRequest>(request), ct)
                .ConfigureAwait(false);

            return new ValidationResult
            {
                IsValid = validationResult.IsValid,
                ErrorMessage = validationResult.ToString()
            };
        }

        protected sealed override void EnsureInstanceNotNull(object instanceToValidate) =>
            base.EnsureInstanceNotNull(instanceToValidate);

        protected sealed override bool PreValidate(ValidationContext<TRequest> context, FluentValidation.Results.ValidationResult result) =>
            base.PreValidate(context, result);

        public sealed override FluentValidation.Results.ValidationResult Validate(ValidationContext<TRequest> context) =>
            base.Validate(context);

        public sealed override IValidatorDescriptor CreateDescriptor() => base.CreateDescriptor();

        public sealed override Task<FluentValidation.Results.ValidationResult> ValidateAsync(
            ValidationContext<TRequest> context,
            CancellationToken cancellation = new CancellationToken()) => base.ValidateAsync(context, cancellation);

    }
}
