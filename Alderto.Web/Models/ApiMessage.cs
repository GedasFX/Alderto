using System;
using Discord;

namespace Alderto.Web.Models
{
    public class ApiMessage
    {
        public ulong Id { get; set; }

        public ulong ChannelId { get; set; }

        public string AuthorUsername { get; set; }
        public string AuthorAvatarId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? EditedAt { get; set; }

        public string Contents { get; set; }

#nullable disable
        public ApiMessage() { }
#nullable restore
        public ApiMessage(IMessage message)
        {
            Id = message.Id;
            ChannelId = message.Channel.Id;

            AuthorUsername = message.Author.Username;
            AuthorAvatarId = message.Author.AvatarId;

            CreatedAt = message.CreatedAt;
            EditedAt = message.EditedTimestamp;

            Contents = message.Content;
        }
    }
}