using System.Collections.Generic;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    public sealed class RequestValidatorResolver : IRequestValidatorResolver
    {
        private readonly IServiceFactory _serviceFactory;

        public RequestValidatorResolver(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public IEnumerable<IRequestValidator<TRequest>> Resolve<TRequest>()
            where TRequest : class
        {
            return _serviceFactory.GetServices<IRequestValidator<TRequest>>();
        }
    }
}
