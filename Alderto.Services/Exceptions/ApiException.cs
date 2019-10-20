using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        /// <summary>
        /// The Human-Machine readable API error message to be sent to the client.
        /// </summary>
        public ErrorMessage Error { get; set; }

        public ApiException(int code)
            : this(ErrorMessages.FromCode(code))
        {
        }

        public ApiException(int code, Exception inner)
            : this(ErrorMessages.FromCode(code), inner)
        {
        }

        public ApiException(ErrorMessage errorMessage) : base(errorMessage.Message)
        {
            Error = errorMessage;
        }

        public ApiException(ErrorMessage errorMessage, Exception inner) : base(errorMessage.Message, inner)
        {
            Error = errorMessage;
        }

        protected ApiException(
            ErrorMessage errorMessage,
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
            Error = errorMessage;
        }

        
    }
}