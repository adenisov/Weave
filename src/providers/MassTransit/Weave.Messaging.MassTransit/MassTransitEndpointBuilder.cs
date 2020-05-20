using System;
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

        protected virtual void Validate()
        {
            if (ContainerConfigurator == null)
            {
                throw new InvalidOperationException($"{nameof(ContainerConfigurator)} must be specified.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="extension"></param>
        /// <typeparam name="TExtension"></typeparam>
        /// <returns></returns>
        public TBuilder WithCustomExtension<TExtension>(TExtension extension)
            where TExtension : class, IEndpointExtension
        {
            _customExtensions.Add(extension);

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
            return WithCustomExtension(new TExtension());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extensions"></param>
        /// <returns></returns>
        public TBuilder WithCustomExtensions(params IEndpointExtension[] extensions)
        {
            foreach (var extension in extensions)
            {
                WithCustomExtension(extension);
            }

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
