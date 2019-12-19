using System;
using System.Collections.Generic;
using Weave.Messaging.MassTransit.RabbitMq.Extensions;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RabbitMqEndpointBuilder : MassTransitEndpointBuilder<RabbitMqEndpointBuilder>
    {
        private readonly RabbitMqHostSettings _rabbitMqHostSettings = new RabbitMqHostSettings();

        private IRabbitMqTopology _rabbitMqTopology;
        private IRabbitMqTopologyFeaturesConfiguration _rabbitMqTopologyFeaturesConfiguration;

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
            _rabbitMqTopology = new MessageTypeTopology(applicationName);

            return this;
        }

        public RabbitMqEndpointBuilder WithTopology(IRabbitMqTopology topology)
        {
            _rabbitMqTopology = topology;

            return this;
        }

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
                new RegisterMessageHandlersExtension(
                    _rabbitMqHostSettings,
                    _rabbitMqTopology,
                    _rabbitMqTopologyFeaturesConfiguration)
            };

            extensions.AddRange(CustomExtensions);
            
            var endpoint = new MassTransitRabbitMqEndpoint(extensions.ToArray());
            endpoint.ConfigureContainer(ContainerConfigurator);
            
            return endpoint;
        }

        private void Validate()
        {
            if (_rabbitMqTopology == null)
            {
                // ToDo: throw ValidationException.
            }
        }
    }
}
