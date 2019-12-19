using System.Collections.Generic;
using System.Collections.Immutable;

namespace Weave.Messaging.MassTransit
{
    public abstract class MassTransitEndpointBuilder<TBuilder>
        where TBuilder : MassTransitEndpointBuilder<TBuilder>
    {
        private readonly ICollection<IEndpointExtension> _customExtensions;

        protected MassTransitEndpointBuilder()
        {
            _customExtensions = new HashSet<IEndpointExtension>();
        }

        protected IReadOnlyCollection<IEndpointExtension> CustomExtensions => _customExtensions.ToImmutableHashSet();
        protected IContainerConfigurator ContainerConfigurator;

        /// <summary>
        ///
        /// </summary>
        /// <param name="behavior"></param>
        /// <typeparam name="TExtension"></typeparam>
        /// <returns></returns>
        public TBuilder WithCustomExtension<TExtension>(TExtension behavior)
            where TExtension : class, IEndpointExtension
        {
            _customExtensions.Add(behavior);

            return (TBuilder) this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TExtension"></typeparam>
        /// <returns></returns>
        public TBuilder WithCustomExtension<TExtension>()
            where TExtension : class, IEndpointExtension, new()
        {
            _customExtensions.Add(new TExtension());

            return (TBuilder) this;
        }

        public TBuilder WithContainerConfigurator(IContainerConfigurator configurator)
        {
            ContainerConfigurator = configurator;

            return (TBuilder) this;
        }

        public abstract IMassTransitEndpoint Build();
    }
}
