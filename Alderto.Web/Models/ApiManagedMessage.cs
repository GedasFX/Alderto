using System;
using Alderto.Data.Models;

namespace Alderto.Web.Models
{
    public class ApiManagedMessage
    {
        public ulong Id { get; set; }
        public ulong ChannelId { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public ulong? ModeratorRoleId { get; set; }
        public string? Content { get; set; }

#nullable disable
        private ApiManagedMessage() { }
#nullable restore
        public ApiManagedMessage(GuildManagedMessage message)
        {
            Id = message.MessageId;
            ChannelId = message.ChannelId;

            ModeratorRoleId = message.ModeratorRoleId;

            Content = message.Content;
            LastModified = message.LastModified;
        }
    }
}