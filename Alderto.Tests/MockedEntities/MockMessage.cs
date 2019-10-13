using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;

#nullable disable

namespace Alderto.Tests.MockedEntities
{
    public class MockMessage : IMessage
    {
        public MockMessage(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Task DeleteAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public MessageType Type { get; set; }
        public MessageSource Source { get; set; }
        public bool IsTTS { get; set; }
        public bool IsPinned { get; set; }
        public string Content { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public DateTimeOffset? EditedTimestamp { get; set; }
        public IMessageChannel Channel { get; set; }
        public IUser Author { get; set; }
        public IReadOnlyCollection<IAttachment> Attachments { get; set; }
        public IReadOnlyCollection<IEmbed> Embeds { get; set; }
        public IReadOnlyCollection<ITag> Tags { get; set; }
        public IReadOnlyCollection<ulong> MentionedChannelIds { get; set; }
        public IReadOnlyCollection<ulong> MentionedRoleIds { get; set; }
        public IReadOnlyCollection<ulong> MentionedUserIds { get; set; }
        public MessageActivity Activity { get; set; }
        public MessageApplication Application { get; set; }
    }
}