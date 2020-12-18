namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ValidationResult
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsValid { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
