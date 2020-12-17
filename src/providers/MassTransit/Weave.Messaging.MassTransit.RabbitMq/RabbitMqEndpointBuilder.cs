using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.RabbitMq.Extensions;
using Weave.Messaging.MassTransit.RabbitMq.Extensions.Sagas;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RabbitMqEndpointBuilder : MassTransitEndpointBuilder<RabbitMqEndpointBuilder>
    {
        private readonly RabbitMqHostSettings _rabbitMqHostSettings = new RabbitMqHostSettings();

        private IRabbitMqTopology _rabbitMqTopology;

        private IRabbitMqTopologyFeaturesConfiguration _rabbitMqTopologyFeaturesConfiguration =
            new DefaultRabbitMqTopologyFeaturesConfiguration();

        internal RabbitMqEndpointBuilder()
        {
        }

        protected override void Validate()
        {
            base.Validate();

            if (_rabbitMqTopology == null)
            {
                throw new InvalidOperationException("Topology must be specified.");
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public RabbitMqEndpointBuilder WithConnectionInfo(Action<RabbitMqHostSettings> configuration)
        {
            configuration(_rabbitMqHostSettings);

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public RabbitMqEndpointBuilder WithMessageTypeTopology(string applicationName)
        {
            return WithTopology(new MessageTypeTopology(applicationName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topology"></param>
        /// <returns></returns>
        public RabbitMqEndpointBuilder WithTopology(IRabbitMqTopology topology)
        {
            _rabbitMqTopology = topology;

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="featuresConfiguration"></param>
        /// <returns></returns>
        public RabbitMqEndpointBuilder WithTopologyFeaturesConfiguration(IRabbitMqTopologyFeaturesConfiguration featuresConfiguration)
        {
            _rabbitMqTopologyFeaturesConfiguration = featuresConfiguration;

            return this;
        }

        public override IMassTransitEndpoint Build()
        {
            Validate();

            var extensions = new List<IEndpointExtension>
            {
                new RegisterRabbitMqTransportExtension(_rabbitMqHostSettings),
                new RegisterMessageHandlersExtension(_rabbitMqTopology, _rabbitMqTopologyFeaturesConfiguration),
                new RegisterSagasExtension(_rabbitMqTopology)
            };

            extensions.AddRange(CustomExtensions);

            return new MassTransitRabbitMqEndpoint(ContainerConfigurator, extensions.ToArray());
        }
    }
}
