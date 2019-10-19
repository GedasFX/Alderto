using System;
using System.ComponentModel.DataAnnotations;
using Alderto.Data.Models;

namespace Alderto.Web.Models
{
    public class ApiManagedMessage
    {
        public ulong Id { get; set; }
        public ulong ChannelId { get; set; }
        public DateTimeOffset LastModified { get; set; }

        public ulong? ModeratorRoleId { get; set; }

        [StringLength(2000)]
        public string? Content { get; set; }

        private ApiManagedMessage() { }
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