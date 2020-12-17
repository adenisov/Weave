using System;

namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class DefaultRabbitMqTopologyFeaturesConfiguration : IRabbitMqTopologyFeaturesConfiguration
    {
        public InputSettings DirectInputSettings { get; } = new InputSettings
        {
            AutoDelete = true,
            Durable = false,
            ConfigureConsumeTopology = true,
            Ttl = TimeSpan.FromMinutes(1),
            PrefetchCount = Environment.ProcessorCount * 10,
        };

        public InputSettings PublishSettings { get; } = new InputSettings
        {
            AutoDelete = false,
            Durable = true,
            ConfigureConsumeTopology = false,
            PrefetchCount = Environment.ProcessorCount * 10,
        };
    }
}
