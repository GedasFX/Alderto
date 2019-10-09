using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.NotFound
{
    [Serializable]
    public class BankItemNotFoundException : ApiException
    {
        public BankItemNotFoundException() : base(2201)
        {
        }

        public BankItemNotFoundException(string message) : base(message)
        {
        }

        public BankItemNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BankItemNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}