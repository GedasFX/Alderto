using System;
using Discord;

namespace Alderto.Web.Models
{
    public class ApiMessage
    {
        public ulong Id { get; set; }

        public string AuthorUsername { get; set; }
        public string AuthorAvatarId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? EditedAt { get; set; }

        public string Contents { get; set; }

        public ApiMessage() { }
        public ApiMessage(IMessage message)
        {
            Id = message.Id;
            AuthorUsername = message.Author.Username;
            AuthorAvatarId = message.Author.AvatarId;
            CreatedAt = message.CreatedAt;
            EditedAt = message.EditedTimestamp;
            Contents = message.Content;
        }
    }
}