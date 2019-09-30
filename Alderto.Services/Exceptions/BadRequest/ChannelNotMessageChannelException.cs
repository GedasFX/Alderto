using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.BadRequest
{
    [Serializable]
    public class ChannelNotMessageChannelException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ChannelNotMessageChannelException()
        {
        }

        public ChannelNotMessageChannelException(string message) : base(message)
        {
        }

        public ChannelNotMessageChannelException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ChannelNotMessageChannelException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}