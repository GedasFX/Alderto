using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.NotFound
{
    [Serializable]
    public class BankNotFoundException : ApiException
    {
        public BankNotFoundException() : base(2200)
        {
        }

        public BankNotFoundException(string message) : base(message)
        {
        }

        public BankNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected BankNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}