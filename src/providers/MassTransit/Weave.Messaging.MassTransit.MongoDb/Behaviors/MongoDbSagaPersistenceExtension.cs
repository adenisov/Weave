using System;
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
        private readonly MongoDbHostSettings _mongoDbHostSettings;

        private Action<IContainerRegistar> _containerRegistar;

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
            _containerRegistar = e.Registar;

            var mongoDatabase = GetMongoDatabase();
            _containerRegistar(InstanceRegistrationBuilder<IMongoDatabase>
                .FromInstance(mongoDatabase));

            _containerRegistar(RegistrationBuilder
                .RegisterType<MongoDbSagaConsumeContextFactory>()
                .As<IMongoDbSagaConsumeContextFactory>()
                .Singleton());
        }

        private void OnSagaRegistered(object sender, SagaRegisteredEventArgs e)
        {
            var sagaRepository = typeof(MongoDbSagaRepository<>).MakeGenericType(e.SagaType.GetSagaDataType());
            _containerRegistar(
                RegistrationBuilder
                    .RegisterType(sagaRepository));
        }

        private IMongoDatabase GetMongoDatabase()
        {
            var client = new MongoClient(_mongoDbHostSettings.Host);
            return client.GetDatabase(_mongoDbHostSettings.Database);
        }
    }
}
