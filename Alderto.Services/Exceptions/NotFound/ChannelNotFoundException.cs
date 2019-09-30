using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.NotFound
{
    [Serializable]
    public class ChannelNotFoundException : Exception
    {
        public ChannelNotFoundException()
        {
        }

        public ChannelNotFoundException(string message) : base(message)
        {
        }

        public ChannelNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ChannelNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}