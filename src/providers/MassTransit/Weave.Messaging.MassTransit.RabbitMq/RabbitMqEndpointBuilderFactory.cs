namespace Weave.Messaging.MassTransit.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public static class RabbitMqEndpointBuilderFactory
    {
        /// <summary>
        /// Builds endpoint using <see cref="RabbitMqEndpointBuilder"/>.
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static RabbitMqEndpointBuilder BuildUsingRabbitMq(this MassTransitEndpointFactory factory) =>
            new RabbitMqEndpointBuilder();
    }
}
