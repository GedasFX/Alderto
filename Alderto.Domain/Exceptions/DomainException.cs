using System;
using System.Runtime.Serialization;

namespace Alderto.Domain.Exceptions
{
    [Serializable]
    public class DomainException : Exception
    {
        /// <summary>
        /// The Human-Machine readable API error message to be sent to the client.
        /// </summary>
        public ErrorState ErrorState { get; }

        protected DomainException(ErrorState errorState)
        {
            ErrorState = errorState;
        }

        protected DomainException(ErrorState errorState, string message)
            : base(message)
        {
            ErrorState = errorState;
        }

        protected DomainException(ErrorState errorState, string message, Exception inner)
            : base(message, inner)
        {
            ErrorState = errorState;
        }

        protected DomainException(ErrorState errorState, SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            ErrorState = errorState;
        }
    }
}