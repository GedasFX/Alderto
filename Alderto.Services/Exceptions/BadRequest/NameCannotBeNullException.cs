using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.BadRequest
{
    [Serializable]
    public class NameCannotBeNullException : ApiException
    {
        public NameCannotBeNullException() : base(3300)
        {
        }

        public NameCannotBeNullException(string message) : base(message)
        {
        }

        public NameCannotBeNullException(string message, Exception inner) : base(message, inner)
        {
        }

        protected NameCannotBeNullException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}