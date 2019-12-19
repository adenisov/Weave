namespace Weave.Messaging.MassTransit.RabbitMq
{
    /// <summary>
    ///
    /// </summary>
    public sealed class RabbitMqHostSettings
    {
        /// <summary>
        ///
        /// </summary>
        public string Host { get; set; } = "localhost";

        /// <summary>
        ///
        /// </summary>
        public int Port { get; set; } = 32769;

        /// <summary>
        ///
        /// </summary>
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        ///
        /// </summary>
        public string Username { get; set; } = "guest";

        /// <summary>
        ///
        /// </summary>
        public string Password { get; set; } = "guest";
    }
}
