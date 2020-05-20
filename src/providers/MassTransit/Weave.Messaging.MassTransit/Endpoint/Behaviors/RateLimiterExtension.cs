using System;
using GreenPipes;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle;
using Weave.Messaging.MassTransit.Endpoint.Lifecycle.Events;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    /// <summary>
    /// Adds a message type agnostic rate limiter
    /// </summary>
    public sealed class RateLimiterExtension : IEndpointExtension
    {
        private readonly int _limit;
        private readonly TimeSpan _interval;

        /// <summary>
        /// Creates an instance of <see cref="RateLimiterExtension"/>
        /// </summary>
        /// <param name="limit">Number of messages that can be served during <param name="interval"></param></param>
        /// <param name="interval">Interval during which only a limited by <param name="limit"></param> messages can be consumed</param>
        public RateLimiterExtension(int limit, TimeSpan interval)
        {
            _limit = limit;
            _interval = interval;
        }

        public void Attach(IMassTransitEndpointLifecycle endpointLifecycle)
        {
            endpointLifecycle.MessageBusConfiguring += OnMessageBusConfiguring;
        }

        private void OnMessageBusConfiguring(object sender, MessageBusConfiguringEventArgs e) =>
            e.Configurator.UseRateLimit(_limit, _interval);

        public void Dispose()
        {
        }
    }
}
