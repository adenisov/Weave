namespace Weave.Messaging.MassTransit.RabbitMq
{
    public static class ExchangeTypeExtensions
    {
        private static readonly string[] RabbitMqTypes =
        {
            "direct",
            "fanout",
            "topic",
            "headers"
        };

        internal static string ToRabbitMqType(this ExchangeType exchangeType) => RabbitMqTypes[(int) exchangeType];
    }
}
