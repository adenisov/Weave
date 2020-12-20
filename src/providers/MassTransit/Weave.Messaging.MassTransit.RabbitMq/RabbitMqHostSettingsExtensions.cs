namespace Weave.Messaging.MassTransit.RabbitMq
{
    public static class RabbitMqHostSettingsExtensions
    {
        /// <summary>
        /// Allows to sign in using default guest/guest credentials
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="virtualHost"></param>
        public static void AsGuest(this RabbitMqHostSettings settings, string host, int port, string virtualHost = "/")
        {
            settings.Host = host;
            settings.Port = port;
            settings.VirtualHost = virtualHost;

            settings.Username = "guest";
            settings.Password = "guest";
        }
    }
}
