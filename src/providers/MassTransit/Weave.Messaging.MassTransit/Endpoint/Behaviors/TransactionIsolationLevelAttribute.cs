using System;
using System.Diagnostics;
using System.Transactions;

namespace Weave.Messaging.MassTransit.Endpoint.Behaviors
{
    /// <summary>
    /// Defines the <seealso cref="IsolationLevel"/> and the <seealso cref="Timeout"/>
    /// of transaction that wraps the message handler execution.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TransactionIsolationLevelAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <param name="timeout"></param>
        [DebuggerStepThrough]
        public TransactionIsolationLevelAttribute(IsolationLevel isolationLevel, long timeout)
        {
            IsolationLevel = isolationLevel;
            Timeout = timeout <= 0 ? throw new ArgumentException(nameof(timeout)) : TimeSpan.FromMilliseconds(timeout);
        }

        public IsolationLevel IsolationLevel { get; }

        public TimeSpan Timeout { get; }
    }
}
