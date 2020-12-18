using System.Collections.Generic;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestValidatorResolver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <returns></returns>
        IEnumerable<IRequestValidator<TRequest>> Resolve<TRequest>()
            where TRequest : class;
    }
}
