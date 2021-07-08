using System;

namespace Alderto.Application.Features.ManagedMessage.Dto
{
    public class ManagedMessageDto
    {
        public ulong MessageId { get; set; }
        public ulong ChannelId { get; set; }

        public DateTimeOffset LastModified { get; set; }

        public string? Content { get; set; }
    }
}
