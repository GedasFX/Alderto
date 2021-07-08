using System;
using System.Runtime.Serialization;

namespace Alderto.Domain.Exceptions
{
    public class ValidationDomainException : DomainException
    {
        public ValidationDomainException()
            : base(new ErrorState(ErrorStatusCode.BadRequest))
        {
        }

        public ValidationDomainException(string message)
            : base(new ErrorState(ErrorStatusCode.BadRequest), message)
        {
        }

        public ValidationDomainException(string message, Exception inner)
            : base(new ErrorState(ErrorStatusCode.BadRequest), message, inner)
        {
        }

        public ValidationDomainException(SerializationInfo info, StreamingContext context)
            : base(new ErrorState(ErrorStatusCode.BadRequest), info, context)
        {
        }
    }
}
