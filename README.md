# Weave
[![Build status](https://ci.appveyor.com/api/projects/status/82qbqi3docftm75q/branch/master?svg=true)](https://ci.appveyor.com/project/adenisov/weave/branch/master)

The missing event-driven framework for .NET

Contains a high-level abstraction for the following [EIP](https://www.enterpriseintegrationpatterns.com/patterns/messaging/toc.html) patterns
1. [Point-to-point channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PointToPointChannel.html)
2. [Publish-subscribe channel](https://www.enterpriseintegrationpatterns.com/patterns/messaging/PublishSubscribeChannel.html)
3. [Message bus](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessageBus.html)
4. [Routing Slip](https://www.enterpriseintegrationpatterns.com/patterns/messaging/RoutingTable.html)


## Usage
Contains implementation based on [MassTransit](https://github.com/masstransit/).
Sample code is using RabbitMq transport.

### Create Endpoint
```c#
var endpoint = new RabbitMqEndpointBuilder()
                .WithContainerConfigurator(new AutofacContainerConfigurator(containerBuilder))
                .WithMessageTypeTopology("Test_Application_Name")
                .WithTopologyFeaturesConfiguration(new DefaultRabbitMqTopologyFeaturesConfiguration())
                .Build();
```

### Create Messaging Module
```c#
    public sealed class TestMessagingModule : MessagingModule
    {
        protected override void Load(IMessagingModuleBuilder builder)
        {
            // Query
            builder.WithHandler<TestQueryHandler>();
            // Command
            builder.WithHandler<TestCommandHandler>();
            // Event
            builder.WithHandler<TestEventHandler>();
            
            // Sagas
            builder.WithSaga<TestSaga>();
        }
    }
```

### Register Messaging Module
```c#
endpoint.RegisterMessagingModule(new TestMessagingModule());
```

### Finish Configuration
```c#
endpoint.Configure();

var messageBus = endpoint.CreateMessageBus();
```

### Run
```c#
// Query (fetch, direct, p2p)
await messageBus.RequestAsync(new TestQuery()).ConfigureAwait(false);

// Command (execute, direct/broadcast)
await messageBus.SendAsync(new TestCommand()).ConfigureAwait(false);

// Event (Pub-sub)
await messageBus.Publish(new TestEvent { OrderId = i }).ConfigureAwait(false);
```
