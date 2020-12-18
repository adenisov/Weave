using System.Threading;
using System.Threading.Tasks;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IRequestValidator<in TRequest>
        where TRequest : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ValidationResult Validate(TRequest request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<ValidationResult> ValidateAsync(TRequest request, CancellationToken ct);
    }
}
