using System;
using System.Runtime.Serialization;

namespace Alderto.Data.Exceptions
{
    [Serializable]
    public class UniqueIndexViolationException : Exception
    {
        public UniqueIndexViolationException()
        {
        }

        public UniqueIndexViolationException(string message) : base(message)
        {
        }

        public UniqueIndexViolationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected UniqueIndexViolationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
