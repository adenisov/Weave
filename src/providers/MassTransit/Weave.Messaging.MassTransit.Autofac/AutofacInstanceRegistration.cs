using System.Linq;
using Autofac;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;

namespace Weave.Messaging.MassTransit.Autofac
{
    internal sealed class AutofacInstanceRegistration : IInstanceRegistration
    {
        private readonly ContainerBuilder _containerBuilder;
        private readonly IInstanceRegistrationSource _source;

        public AutofacInstanceRegistration(ContainerBuilder containerBuilder, IInstanceRegistrationSource source)
        {
            _containerBuilder = containerBuilder;
            _source = source;
        }

        public void Register<T>(T instance) where T : class
        {
            var registrationBuilder = _containerBuilder.RegisterInstance(instance);
            if (_source.AsTypes != null && _source.AsTypes.Any())
            {
                registrationBuilder.As(_source.AsTypes);
            }
        }
    }
}
