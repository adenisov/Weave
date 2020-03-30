using System;
using Weave.Messaging.Core;
using Weave.Messaging.MassTransit.MongoDb.Behaviors;

namespace Weave.Messaging.MassTransit.MongoDb
{
    public static class EndpointExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configurator"></param>
        /// <typeparam name="TBuilder"></typeparam>
        /// <returns></returns>
        public static MassTransitEndpointBuilder<TBuilder> StoreSagaInMongoDb<TBuilder>(
            this MassTransitEndpointBuilder<TBuilder> builder,
            Action<MongoDbHostSettings> configurator = null)
            where TBuilder : MassTransitEndpointBuilder<TBuilder>
        {
            var mongoDbHostSettings = ConfigurationFactory.Configure(configurator);
            return builder.WithCustomExtension(new MongoDbSagaPersistenceExtension(mongoDbHostSettings));
        }
    }
}
