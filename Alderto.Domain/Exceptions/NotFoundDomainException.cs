using System;
using System.Runtime.Serialization;

namespace Alderto.Domain.Exceptions
{
    public class NotFoundDomainException : DomainException
    {
        public NotFoundDomainException()
            : base(new ErrorState(ErrorStatusCode.NotFound))
        {
        }

        public NotFoundDomainException(string message)
            : base(new ErrorState(ErrorStatusCode.NotFound), message)
        {
        }

        public NotFoundDomainException(string message, Exception inner)
            : base(new ErrorState(ErrorStatusCode.NotFound), message, inner)
        {
        }

        public NotFoundDomainException(SerializationInfo info, StreamingContext context)
            : base(new ErrorState(ErrorStatusCode.NotFound), info, context)
        {
        }
    }
}
