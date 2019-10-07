﻿using System;
using System.Runtime.Serialization;

namespace Alderto.Services.Exceptions.NotFound
{
    [Serializable]
    public class GuildNotFoundException : ApiException
    {
        public GuildNotFoundException() : base(2100)
        {
        }

        public GuildNotFoundException(string message) : base(message)
        {
        }

        public GuildNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected GuildNotFoundException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}