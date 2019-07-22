using System.Threading.Tasks;
using Alderto.Bot.Services;
using Alderto.Data;
using Alderto.Tests.MockedEntities;
using Xunit;

namespace Alderto.Tests
{
    public class UserProviderTests
    {
        private readonly IGuildUserManager _manager;

        public UserProviderTests()
        {
            var context = new MockDbContext();
            _manager = new GuildUserManager(context);
        }

        [Fact]
        public async Task GetMember()
        {
            // Fail to get the entity when he doesn't exist.
            var member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1, addIfNonExistent: false);
            Assert.Null(member);

            // This time make sure to add the member.
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);

            // Check if internal extenstion .SingleOrDefaultAsync() does not crash
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);

            // Once more to see if it wasn't added a second time.
            member = await _manager.GetGuildMemberAsync(guildId: 1, memberId: 1);
            Assert.NotNull(member);
        }
    }
}
