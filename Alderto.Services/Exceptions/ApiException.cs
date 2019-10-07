using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions
{
    [Serializable]
    public class ApiException : Exception
    {
        public int ErrorCode { get; set; }

        public ApiException(int code)
        {
            ErrorCode = code;
        }

        public ApiException(string message) : base(message)
        {
        }

        public ApiException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ApiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}