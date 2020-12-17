using System;
using System.Collections.Generic;
using MassTransit.MongoDbIntegration.Saga;
using MassTransit.MongoDbIntegration.Saga.Context;
using MongoDB.Driver;
using Weave.Messaging.Core;
using Weave.Messaging.MassTransit.Endpoint.Behaviors.Container;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.MongoDb.Behaviors
{
    public sealed class MongoDbSagaPersistenceExtension : IEndpointExtension
    {
        private readonly ISet<Type> _sagaTypes = new HashSet<Type>();
        private readonly MongoDbHostSettings _mongoDbHostSettings;

        public MongoDbSagaPersistenceExtension(MongoDbHostSettings mongoDbHostSettings)
        {
            _mongoDbHostSettings = mongoDbHostSettings;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.ContainerRegistered += OnContainerRegistered;
            endpointLifecycle.SagaRegistered += OnSagaRegistered;
        }

        private void OnContainerRegistered(object sender, ContainerRegisteredEventArgs e)
        {
            var mongoDatabase = GetMongoDatabase();
            e.Registar(InstanceRegistrationBuilder<IMongoDatabase>
                .FromInstance(mongoDatabase));

            /* ToDo: breaking change
            e.Registar(RegistrationBuilder
                .RegisterType<MongoDbSagaConsumeContextFactory>()
                .As<IMongoDbSagaConsumeContextFactory>()
                .Singleton());
            */

            foreach (var sagaType in _sagaTypes)
            {
                var sagaRepositoryType = typeof(MongoDbSagaRepository<>).MakeGenericType(sagaType.GetSagaDataType());
                e.Registar(
                    RegistrationBuilder
                        .RegisterType(sagaRepositoryType));
            }
        }

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e) => _sagaTypes.Add(e.SagaType);

        private IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient(_mongoDbHostSettings.Host);
            return client.GetDatabase(_mongoDbHostSettings.Database);
        }

        public void Dispose()
        {
        }
    }
}
