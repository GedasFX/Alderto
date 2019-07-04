using System;
using System.Threading.Tasks;
using Discord;

namespace Alderto.Tests.MockedEntities
{
    public class MockUser : IUser
    {
        public ulong Id { get; set; }

        public string AvatarId { get; set; }

        public string Discriminator { get; set; }

        public ushort DiscriminatorValue { get; set; }

        public bool IsBot { get; set; }

        public bool IsWebhook { get; set; }

        public string Username { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public string Mention => '@' + Username + '#' + Discriminator;

        public IActivity Activity { get; set; }

        public UserStatus Status { get; set; }

        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128) => throw new NotImplementedException();
        public string GetDefaultAvatarUrl() => throw new NotImplementedException();
        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null) => throw new NotImplementedException();
    }
}
