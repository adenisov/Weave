namespace Weave.Messaging.MassTransit.RabbitMq
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMqTopologyFeaturesConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        InputSettings DirectInputSettings { get; }
        
        /// <summary>
        /// 
        /// </summary>
        InputSettings PublishSettings { get; }
    }
}