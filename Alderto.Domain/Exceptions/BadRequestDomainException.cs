using System;
using System.Runtime.Serialization;

namespace Alderto.Domain.Exceptions
{
    public class BadRequestDomainException : DomainException
    {
        public BadRequestDomainException()
            : base(new ErrorState(ErrorStatusCode.BadRequest))
        {
        }

        public BadRequestDomainException(string message)
            : base(new ErrorState(ErrorStatusCode.BadRequest), message)
        {
        }

        public BadRequestDomainException(string message, Exception inner)
            : base(new ErrorState(ErrorStatusCode.BadRequest), message, inner)
        {
        }

        public BadRequestDomainException(SerializationInfo info, StreamingContext context)
            : base(new ErrorState(ErrorStatusCode.BadRequest), info, context)
        {
        }
    }
}