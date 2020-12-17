using System;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    /// <summary>
    /// Suppresses the transactional behavior of the specific message handler
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SuppressTransactionAttribute : Attribute
    {
    }
}
