using System;

namespace Weave.Messaging.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class VoidResponse : IEquatable<VoidResponse>, IComparable<VoidResponse>, IMessage
    {
        private VoidResponse()
        {
        }

        public bool Equals(VoidResponse other) => true;

        public int CompareTo(VoidResponse other) => 0;

        /// <summary>
        ///
        /// </summary>
        public static readonly VoidResponse Value = new VoidResponse();
    }
}
