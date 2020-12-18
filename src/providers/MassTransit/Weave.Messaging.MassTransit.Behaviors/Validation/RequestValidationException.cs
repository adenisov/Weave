using System;
using System.Collections.Generic;
using System.Linq;

namespace Weave.Messaging.MassTransit.Behaviors.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class RequestValidationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationErrors"></param>
        public RequestValidationException(IReadOnlyCollection<ValidationResult> validationErrors)
        {
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="validationErrors"></param>
        public RequestValidationException(string message, IReadOnlyCollection<ValidationResult> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<ValidationResult> ValidationErrors { get; }

        public override string ToString()
        {
            return ValidationErrors
                .Select(_ => _.ErrorMessage)
                .Aggregate((x, y) => string.Concat(x, Environment.NewLine, y));
        }
    }
}
