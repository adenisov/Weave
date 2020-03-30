namespace Weave.Messaging.MassTransit.MongoDb
{
    public sealed class MongoDbHostSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string Host { get; set; } = "mongodb://localhost:32768";

        /// <summary>
        /// 
        /// </summary>
        public string Database { get; set; } = "sagas";
    }
}
