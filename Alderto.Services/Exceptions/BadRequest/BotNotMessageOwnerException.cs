using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.BadRequest
{
    [Serializable]
    public class BotNotMessageOwnerException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BotNotMessageOwnerException()
        {
        }

        public BotNotMessageOwnerException(string message) : base(message)
        {
        }

        public BotNotMessageOwnerException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BotNotMessageOwnerException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}