namespace Weave.Messaging.MassTransit.RabbitMq
{
    public sealed class MessageAddress
    {
        public string ExchangeName { get; set; } = string.Empty;

        public string RoutingKey { get; set; } = string.Empty;

        public ExchangeType ExchangeType { get; set; } = ExchangeType.Fanout;

        public override string ToString() => $"{ExchangeName}:{RoutingKey}";
    }
}
