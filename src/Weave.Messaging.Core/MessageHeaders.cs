using System;
using System.Collections.Generic;

namespace Weave.Messaging.Core
{
    public sealed class MessageHeaders
    {
        public const string CorrelationIdHeader = "CorrelationId";
        public const string MessageIdHeader = "MessageId";

        private readonly IDictionary<string, string> _headers;

        public MessageHeaders(IDictionary<string, string> headers)
        {
            _headers = headers;
        }

        public MessageHeaders()
            : this(new Dictionary<string, string>())
        {
        }

        public MessageHeaders WithMessageId(Guid messageId)
        {
            return WithHeader(MessageIdHeader, messageId.ToString("D").ToLowerInvariant());
        }

        public MessageHeaders WithCorrelationId(Guid correlationId)
        {
            return WithHeader(CorrelationIdHeader, correlationId.ToString("D").ToLowerInvariant());
        }

        public MessageHeaders WithHeader(string name, string value)
        {
            _headers[name] = value;

            return this;
        }

        public MessageHeaders WithHeaders(IDictionary<string, string> headers)
        {
            if (headers == null)
            {
                throw new ArgumentNullException(nameof(headers));
            }

            foreach (var (key, value) in headers)
            {
                WithHeader(key, value);
            }

            return this;
        }

        public string this[string key] => _headers[key];

        public Guid? MessageId
        {
            get
            {
                if (_headers.TryGetValue(MessageIdHeader, out var messageId))
                {
                    return Guid.Parse(messageId);
                }

                return null;
            }
        }

        public Guid? CorrelationId
        {
            get
            {
                if (_headers.TryGetValue(CorrelationIdHeader, out var correlationId))
                {
                    return Guid.Parse(correlationId);
                }

                return null;
            }
        }
    }
}
